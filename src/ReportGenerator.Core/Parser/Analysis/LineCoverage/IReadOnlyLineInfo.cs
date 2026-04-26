using System.Collections.Generic;

namespace Palmmedia.ReportGenerator.Core.Parser.Analysis.LineCoverage
{
    /// <summary>
    /// Read-only interface for line information. This interface is implemented by <see cref="ILineInfo{T}"/> and is used to expose line information in a read-only manner.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the line information.</typeparam>
    public interface IReadOnlyLineInfo<T> : IEnumerable<T>
    {
        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        long LongLength { get; }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The element at the specified index.</returns>
        T this[long index] { get; }
    }
}
