using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Palmmedia.ReportGenerator.Core.Common;
using Palmmedia.ReportGenerator.Core.Logging;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using Palmmedia.ReportGenerator.Core.Properties;

namespace Palmmedia.ReportGenerator.Core.Reporting.Builders
{
    /// <summary>
    /// Create a summary report in "cjson".
    /// This report format can be sent to Azure Devops to provide it code coverage information, which includes Pull Request line indicators.
    /// The "cjson" format is mentioned in https://learn.microsoft.com/en-us/azure/devops/pipelines/tasks/reference/publish-code-coverage-results-v2?view=azure-pipelines#remarks.
    /// </summary>
    public class CJsonReportBuilder : IReportBuilder
    {
        /// <summary>
        /// The Logger.
        /// </summary>
        private static readonly ILogger Logger = LoggerFactory.GetLogger(typeof(CJsonReportBuilder));

        /// <summary>
        /// Gets the report type.
        /// </summary>
        /// <value>
        /// The report type.
        /// </value>
        public string ReportType => "cjson";

        /// <summary>
        /// Gets or sets the report context.
        /// </summary>
        /// <value>
        /// The report context.
        /// </value>
        public IReportContext ReportContext { get; set; }

        /// <summary>
        /// Creates a class report.
        /// </summary>
        /// <param name="class">The class.</param>
        /// <param name="fileAnalyses">The file analyses that correspond to the class.</param>
        public void CreateClassReport(Class @class, IEnumerable<FileAnalysis> fileAnalyses)
        {
        }

        /// <summary>
        /// Creates the summary report.
        /// </summary>
        /// <param name="summaryResult">The summary result.</param>
        public void CreateSummaryReport(SummaryResult summaryResult)
        {
            if (summaryResult == null)
            {
                throw new ArgumentNullException(nameof(summaryResult));
            }

            string targetDirectory = this.ReportContext.ReportConfiguration.TargetDirectory;

            if (this.ReportContext.Settings.CreateSubdirectoryForAllReportTypes)
            {
                targetDirectory = Path.Combine(targetDirectory, this.ReportType);

                if (!Directory.Exists(targetDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(targetDirectory);
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorFormat(Resources.TargetDirectoryCouldNotBeCreated, targetDirectory, ex.GetExceptionMessageForDisplay());
                        return;
                    }
                }
            }

            var targetPath = Path.Combine(targetDirectory, "coverage.cjson");

            var fileCoverages = new Dictionary<string, FileCoverageInfo>();

            foreach (var assembly in summaryResult.Assemblies)
            {
                foreach (var @class in assembly.Classes)
                {
                    foreach (var file in @class.Files)
                    {
                        if (!fileCoverages.TryGetValue(file.Path, out var fileCoverageInfo))
                        {
                            fileCoverageInfo = new FileCoverageInfo { FilePath = file.Path, };

                            fileCoverages.Add(file.Path, fileCoverageInfo);
                        }

                        var lineNumber = 0;

                        foreach (var line in file.LineCoverage)
                        {
                            if (line != -1 && lineNumber != 0)
                            {
                                fileCoverageInfo.SetLineCoverageStatus(lineNumber, line != 0);
                            }

                            if (file.BranchesByLine != null && file.BranchesByLine.TryGetValue(lineNumber, out var branches))
                            {
                                fileCoverageInfo.SetBranchCoverageStatus(lineNumber, branches);
                            }

                            ++lineNumber;
                        }
                    }
                }
            }

            var fileCoveragesValues = fileCoverages.Values.ToList();

            Logger.InfoFormat(Resources.WritingReportFile, targetPath);

            using (var stream = new FileStream(targetPath, FileMode.Create))
            {
                JsonSerializer.Serialize(stream, fileCoveragesValues);
            }
        }

        private sealed class FileCoverageInfo
        {
            private const int CoveredValue = 0;
            private const int NotCoveredValue = 1;

            public string FilePath { get; set; }

            public IDictionary<int, int> LineCoverageStatus { get; } = new SortedDictionary<int, int>();

            public IDictionary<int, BranchCoverageStatistics> BranchCoverageStatus { get; } =
                new SortedDictionary<int, BranchCoverageStatistics>();

            public void SetLineCoverageStatus(int lineNumber, bool covered)
            {
                if (this.LineCoverageStatus.ContainsKey(lineNumber))
                {
                    return;
                }

                this.LineCoverageStatus.Add(lineNumber, covered ? CoveredValue : NotCoveredValue);
            }

            public void SetBranchCoverageStatus(int lineNumber, ICollection<Branch> branches)
            {
                if (this.BranchCoverageStatus.ContainsKey(lineNumber))
                {
                    return;
                }

                var branchCoverageStatistics = new BranchCoverageStatistics
                {
                    TotalBranches = branches.Count,
                    CoveredBranches = branches.Count(b => b.BranchVisits > 0)
                };

                this.BranchCoverageStatus.Add(lineNumber, branchCoverageStatistics);
            }
        }

        private sealed class BranchCoverageStatistics
        {
            public int TotalBranches { get; set; }

            public int CoveredBranches { get; set; }
        }
    }
}