import { Component, OnInit } from '@angular/core';
import { GlobalConstants } from 'src/common/global-constants';
import { TranslateService } from '@ngx-translate/core';
import { ConfigLoaderService } from 'projects/config-loader/';
import { Router } from '@angular/router';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass']
})
export class AppComponent implements OnInit {
  public title: string = "";
  public languages: string[] = [];
  public isLogin:boolean=false;
  constructor(private _configLoader: ConfigLoaderService, private _translate: TranslateService, private _router:Router) {
    this.init();
  }
  init() {
    this.loadConfig();
    this._translate.setDefaultLang(GlobalConstants.defaultLanguage);
  }
  loadConfig() {
    if (this._configLoader.getConfigObjectKey("apiUrl") != null)
      GlobalConstants.apiURL = this._configLoader.getConfigObjectKey("apiUrl");
    if (this._configLoader.getConfigObjectKey("title") != null)
      GlobalConstants.title = this._configLoader.getConfigObjectKey("title");
    if (this._configLoader.getConfigObjectKey("languageSetting") != null) {
      GlobalConstants.defaultLanguage = this._configLoader.getConfigObjectKey("languageSetting")["default"];
      GlobalConstants.languages = this._configLoader.getConfigObjectKey("languageSetting")["languages"];
    }
  }
  ngOnInit() {
    this.title = GlobalConstants.title;
    for (let item of GlobalConstants.languages)
      this.languages.push(item);
  }
  changeLanguage(ln:string){
    this._translate.setDefaultLang(ln);
  }
  goHome(){
    this._router.navigate(["home"]);
  }
  login(){
    this._router.navigate(["user-login"])
  }
  logout(){

  }
}
