import { SettingsService } from "src/app/infrastructure/settings.service";
import { Class } from "../data/class.class";
import { HistoricCoverage } from "../data/historic-coverage.class";
import { ElementBase } from "./elementbase.class";
import { Helper } from "./helper.class";

export class ClassViewModel extends ElementBase {
    reportPath: string = "";
    lineCoverageHistory: number[] = [];
    branchCoverageHistory: number[] = [];
    methodCoverageHistory: number[] = [];
    methodFullCoverageHistory: number[] = [];
    historicCoverages: HistoricCoverage[] = [];
    metrics: any;

    currentHistoricCoverage: HistoricCoverage|null = null;

    _coveredLines: number = 0;
    _uncoveredLines: number = 0;
    _coverableLines: number = 0;
    _totalLines: number = 0;

    _coveredBranches: number = 0;
    _totalBranches: number = 0;

    _coveredMethods: number = 0;
    _fullyCoveredMethods: number = 0;
    _totalMethods: number = 0;

    get coveredLines(): number {
        return this._coveredLines;
    }
    set coveredLines(value: number) {
        this._coveredLines = value;
    }

    get uncoveredLines(): number {
        return this._uncoveredLines;
    }
    set uncoveredLines(value: number) {
        this._uncoveredLines = value;
    }

    get coverableLines(): number {
        return this._coverableLines;
    }
    set coverableLines(value: number) {
        this._coverableLines = value;
    }

    get totalLines(): number {
        return this._totalLines;
    }
    set totalLines(value: number) {
        this._totalLines = value;
    }

    get coveredBranches(): number {
        return this._coveredBranches;
    }
    set coveredBranches(value: number) {
        this._coveredBranches = value;
    }

    get totalBranches(): number {
        return this._totalBranches;
    }
    set totalBranches(value: number) {
        this._totalBranches = value;
    }

    get coveredMethods(): number {
        return this._coveredMethods;
    }
    set coveredMethods(value: number) {
        this._coveredMethods = value;
    }

    get fullyCoveredMethods(): number {
        return this._fullyCoveredMethods;
    }
    set fullyCoveredMethods(value: number) {
        this._fullyCoveredMethods = value;
    }

    get totalMethods(): number {
        return this._totalMethods;
    }
    set totalMethods(value: number) {
        this._totalMethods = value;
    }

    constructor(
        clazz: Class,
        queryString: string) {
            super();
            this.name = clazz.name;
            this.reportPath = clazz.rp ? clazz.rp + queryString : clazz.rp;

            this.coveredLines = clazz.cl;
            this.uncoveredLines = clazz.ucl;
            this.coverableLines = clazz.cal;
            this.totalLines = clazz.tl;

            this.coveredBranches = clazz.cb;
            this.totalBranches = clazz.tb;

            this.coveredMethods = clazz.cm;
            this.fullyCoveredMethods = clazz.fcm;
            this.totalMethods = clazz.tm;

            this.lineCoverageHistory = clazz.lch;
            this.branchCoverageHistory = clazz.bch;
            this.methodCoverageHistory = clazz.mch;
            this.methodFullCoverageHistory = clazz.mfch;

            clazz.hc.forEach(element => {
                this.historicCoverages.push(new HistoricCoverage(element))
            });

            this.metrics = clazz.metrics;
    }

    override get coverage(): number {
        if (this.coverableLines === 0) {
            return NaN;
        }

        return Helper.roundNumber(100 * this.coveredLines / this.coverableLines);
    }

    visible(): boolean {
        if (SettingsService.instance.settings().filter !== "" && this.name.toLowerCase().indexOf(SettingsService.instance.settings().filter.toLowerCase()) === -1) {
            return false;
        }

        let coverageMin = this.coverage;
        let coverageMax = coverageMin;
        coverageMin = Number.isNaN(coverageMin) ? 0 : coverageMin;
        coverageMax = Number.isNaN(coverageMax) ? 100 : coverageMax;

        if (SettingsService.instance.settings().lineCoverageMin > coverageMin || SettingsService.instance.settings().lineCoverageMax < coverageMax) {
            return false;
        }

        let branchCoverageMin = this.branchCoverage;
        let branchCoverageMax = branchCoverageMin;
        branchCoverageMin = Number.isNaN(branchCoverageMin) ? 0 : branchCoverageMin;
        branchCoverageMax = Number.isNaN(branchCoverageMax) ? 100 : branchCoverageMax;
        
        if (SettingsService.instance.settings().branchCoverageMin > branchCoverageMin || SettingsService.instance.settings().branchCoverageMax < branchCoverageMax) {
            return false;
        }
        
        let methodCoverageMin = this.methodCoverage;
        let methodCoverageMax = methodCoverageMin;
        methodCoverageMin = Number.isNaN(methodCoverageMin) ? 0 : methodCoverageMin;
        methodCoverageMax = Number.isNaN(methodCoverageMax) ? 100 : methodCoverageMax;

        if (SettingsService.instance.settings().methodCoverageMin > methodCoverageMin || SettingsService.instance.settings().methodCoverageMax < methodCoverageMax) {
            return false;
        }

        let methodFullCoverageMin = this.methodFullCoverage;
        let methodFullCoverageMax = methodFullCoverageMin;
        methodFullCoverageMin = Number.isNaN(methodFullCoverageMin) ? 0 : methodFullCoverageMin;
        methodFullCoverageMax = Number.isNaN(methodFullCoverageMax) ? 100 : methodFullCoverageMax;

        if (SettingsService.instance.settings().methodFullCoverageMin > methodFullCoverageMin || SettingsService.instance.settings().methodFullCoverageMax < methodFullCoverageMax) {
            return false;
        }

        if (SettingsService.instance.settings().historyComparisionType === "" || this.currentHistoricCoverage === null) {
            return true;
        }

        if (SettingsService.instance.settings().historyComparisionType === "allChanges") {
            if (this.coveredLines === this.currentHistoricCoverage.cl
                && this.uncoveredLines === this.currentHistoricCoverage.ucl
                && this.coverableLines === this.currentHistoricCoverage.cal
                && this.totalLines === this.currentHistoricCoverage.tl
                && this.coveredBranches === this.currentHistoricCoverage.cb
                && this.totalBranches === this.currentHistoricCoverage.tb
                && this.coveredMethods === this.currentHistoricCoverage.cm
                && this.fullyCoveredMethods === this.currentHistoricCoverage.fcm
                && this.totalMethods === this.currentHistoricCoverage.tm) {
                return false;
            }
        } else if (SettingsService.instance.settings().historyComparisionType === "lineCoverageIncreaseOnly") {
            let coverage: number = this.coverage;
            if (isNaN(coverage) || coverage <= this.currentHistoricCoverage.lcq) {
                return false;
            }
        } else if (SettingsService.instance.settings().historyComparisionType === "lineCoverageDecreaseOnly") {
            let coverage: number = this.coverage;
            if (isNaN(coverage) || coverage >= this.currentHistoricCoverage.lcq) {
                return false;
            }
        } else if (SettingsService.instance.settings().historyComparisionType === "branchCoverageIncreaseOnly") {
            let branchCoverage: number = this.branchCoverage;
            if (isNaN(branchCoverage) || branchCoverage <= this.currentHistoricCoverage.bcq) {
                return false;
            }
        } else if (SettingsService.instance.settings().historyComparisionType === "branchCoverageDecreaseOnly") {
            let branchCoverage: number = this.branchCoverage;
            if (isNaN(branchCoverage) || branchCoverage >= this.currentHistoricCoverage.bcq) {
                return false;
            }
        } else if (SettingsService.instance.settings().historyComparisionType === "methodCoverageIncreaseOnly") {
            let methodCoverage: number = this.methodCoverage;
            if (isNaN(methodCoverage) || methodCoverage <= this.currentHistoricCoverage.mcq) {
                return false;
            }
        } else if (SettingsService.instance.settings().historyComparisionType === "methodCoverageDecreaseOnly") {
            let methodCoverage: number = this.methodCoverage;
            if (isNaN(methodCoverage) || methodCoverage >= this.currentHistoricCoverage.mcq) {
                return false;
            }
        } else if (SettingsService.instance.settings().historyComparisionType === "fullMethodCoverageIncreaseOnly") {
            let methodFullCoverage: number = this.methodFullCoverage;
            if (isNaN(methodFullCoverage) || methodFullCoverage <= this.currentHistoricCoverage.mfcq) {
                return false;
            }
        } else if (SettingsService.instance.settings().historyComparisionType === "fullMethodCoverageDecreaseOnly") {
            let methodFullCoverage: number = this.methodFullCoverage;
            if (isNaN(methodFullCoverage) || methodFullCoverage >= this.currentHistoricCoverage.mfcq) {
                return false;
            }
        }

        return true;
    }

    updateCurrentHistoricCoverage(historyComparisionDate: string): void {
        this.currentHistoricCoverage = null;

        if (historyComparisionDate !== "") {
            for (let i: number = 0; i < this.historicCoverages.length; i++) {

                if (this.historicCoverages[i].et === historyComparisionDate) {
                    this.currentHistoricCoverage = this.historicCoverages[i];
                    break;
                }
            }
        }
    }
}