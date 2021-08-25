import { Directive } from '@angular/core';
import { HttpClient,HttpClientModule } from "@angular/common/http";
import { HttpComponent } from "./http.component";
import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';
@Directive()
export abstract class HttpBaseComponent extends HttpComponent{
    constructor(protected _http:HttpClient,protected _translate: TranslateService, protected _router:Router) {
        super(_http);
    }
    t(key:string):string{
        return this._translate.instant(key);
    }
} 