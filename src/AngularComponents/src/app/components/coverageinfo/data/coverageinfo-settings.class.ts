import { SettingsService } from "src/app/infrastructure/settings.service";
import { Metric } from "./metric.class";

export class CoverageInfoSettings {
    showLineCoverage: boolean = true;
    showBranchCoverage: boolean = true;
    showMethodCoverage: boolean = true;
    showFullMethodCoverage: boolean = true;
    visibleMetrics: Metric[] = [];

    _groupingMaximum: number = 0;
    get groupingMaximum(): number {
        return this._groupingMaximum;
    }
    set groupingMaximum(value: number) {
        this._groupingMaximum = value;
        this.updateSettings();
    }
    _grouping: number = 0;
    get grouping(): number {
        return this._grouping;
    }
    set grouping(value: number) {
        this._grouping = value;
        this.updateSettings();
    }
    _historyComparisionDate: string = "";
    get historyComparisionDate(): string {
        return this._historyComparisionDate;
    }
    set historyComparisionDate(value: string) {
        this._historyComparisionDate = value;
        this.updateSettings();
    }
    _historyComparisionType: string = "";
    get historyComparisionType(): string {
        return this._historyComparisionType;
    }
    set historyComparisionType(value: string) {
        this._historyComparisionType = value;
        this.updateSettings();
    }

    _filter: string = "";
    get filter(): string {
        return this._filter;
    }
    set filter(value: string) {
        this._filter = value;
        this.updateSettings();
    }

    _lineCoverageMin: number = 0;
    get lineCoverageMin(): number {
        return this._lineCoverageMin;
    }
    set lineCoverageMin(value: number) {
        this._lineCoverageMin = value;
        this.updateSettings();
    }

    _lineCoverageMax: number = 100;
    get lineCoverageMax(): number {
        return this._lineCoverageMax;
    }
    set lineCoverageMax(value: number) {
        this._lineCoverageMax = value;
        this.updateSettings();
    }

    _branchCoverageMin: number = 0;
    get branchCoverageMin(): number {
        return this._branchCoverageMin;
    }
    set branchCoverageMin(value: number) {
        this._branchCoverageMin = value;
        this.updateSettings();
    }

    _branchCoverageMax: number = 100;
    get branchCoverageMax(): number {
        return this._branchCoverageMax;
    }
    set branchCoverageMax(value: number) {
        this._branchCoverageMax = value;
        this.updateSettings();
    }

    _methodCoverageMin: number = 0;
    get methodCoverageMin(): number {
        return this._methodCoverageMin;
    }
    set methodCoverageMin(value: number) {
        this._methodCoverageMin = value;
        this.updateSettings();
    }

    _methodCoverageMax: number = 100;
    get methodCoverageMax(): number {
        return this._methodCoverageMax;
    }
    set methodCoverageMax(value: number) {
        this._methodCoverageMax = value;
        this.updateSettings();
    }

    _methodFullCoverageMin: number = 0;
    get methodFullCoverageMin(): number {
        return this._methodFullCoverageMin;
    }
    set methodFullCoverageMin(value: number) {
        this._methodFullCoverageMin = value;
        this.updateSettings();
    }

    _methodFullCoverageMax: number = 100;
    get methodFullCoverageMax(): number {
        return this._methodFullCoverageMax;
    }
    set methodFullCoverageMax(value: number) {
        this._methodFullCoverageMax = value;
        this.updateSettings();
    }

    sortBy: string = "name";
    sortOrder: string = "asc";

    collapseStates: boolean[] = [];

    private updateSettings() {
        const udpatedSettings: CoverageInfoSettings = new CoverageInfoSettings();
        Object.assign(udpatedSettings, this);
        SettingsService.instance.updateSettings(udpatedSettings);
    }
}
