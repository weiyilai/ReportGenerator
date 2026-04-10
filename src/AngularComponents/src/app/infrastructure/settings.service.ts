import { Injectable, signal } from "@angular/core";
import { CoverageInfoSettings } from "../components/coverageinfo/data/coverageinfo-settings.class";

@Injectable({ providedIn: 'root' })
export class SettingsService {
  static instance: SettingsService;

  constructor() {
    SettingsService.instance = this;
  }

  settings = signal<CoverageInfoSettings>(new CoverageInfoSettings());

  updateSettings(newSettings: CoverageInfoSettings) {
    this.settings.set(newSettings);
  }
}