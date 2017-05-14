import { Component, OnInit } from '@angular/core';
import { WebsiteInfoOutputDto } from "@generated_module/dtos/website-info-output-dto";
import { AppConfigService } from "@global_module/services/app-config-service";
import { AppTranslationService } from "@global_module/services/app-translation-service";
import { WebsiteManageService } from "@generated_module/services/website-manage-service";
import { AdminToastService } from "@admin_modules/admin_base_module/services/admin-toast-service";

@Component({
    moduleId: module.id,
    selector: 'admin-about-website',
    templateUrl: '../views/admin-about-website.html'
})
export class AdminAboutWebsiteComponent implements OnInit {
    language: string;
    timezone: string;
    apiUrlBase: string;
    websiteInfo: WebsiteInfoOutputDto;

    constructor(
        private appConfigService: AppConfigService,
        private appTranslationService: AppTranslationService,
        private websiteManagerService: WebsiteManageService,
        private adminToastService: AdminToastService) {
        this.websiteInfo = new WebsiteInfoOutputDto();
        this.websiteInfo.Plugins = [];
    }

    ngOnInit() {
        this.language = this.appTranslationService.translate(this.appConfigService.getLanguage());
        this.timezone = this.appTranslationService.translate(this.appConfigService.getTimezone());
        this.apiUrlBase = this.appConfigService.getApiUrlBase();
        this.websiteManagerService.GetWebsiteInfo().subscribe(
            s => this.websiteInfo = s,
            e => this.adminToastService.showToastMessage("error", e));
    }
}
