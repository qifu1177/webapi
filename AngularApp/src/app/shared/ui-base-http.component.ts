import { Directive } from '@angular/core';
import { HttpClient, HttpClientModule } from "@angular/common/http";
import { HttpComponent } from "./http.component";
import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ErrorDialog } from './error-dialog/error-dialog.component';

export interface callBack<T>{(params:T):void};
@Directive()
export abstract class HttpBaseComponent extends HttpComponent {
    waiting:boolean=false;
    constructor(protected _http: HttpClient, protected _translate: TranslateService, protected _router: Router,protected _location:Location, public dialog: MatDialog) {
        super(_http);
    }
    t(key: string, value: any = null): string {
        if (value)
            return this._translate.instant(key, value);
        else
            return this._translate.instant(key);
    }
    showError(error: any, width: number = 360): void {
        if(error.error==null){
            error.error={};            
        }        
        const dialogRef = this.dialog.open(ErrorDialog, {
            width: `${width}px`,
            data: error
        });

        dialogRef.afterClosed().subscribe(result => {
        });
    }
    back() {
        this._location.back();
    }
    get<T>(url:string,callback:callBack<T>){
        this.waiting=true;
        this._http.get<T>(this.createUrl(url)).subscribe({
            next: data => {
                this.waiting=false;
                callback(data);
            },
            error: error => {
                this.waiting=false;
                this.showError(error);
            }
        });
    }
    post<T>(url:string,request:any,callback:callBack<T>){
        this.waiting=true;
        this._http.post<T>(this.createUrl(url), request).subscribe({
            next: data => {
                this.waiting=false;
                callback(data);
            },
            error: error => {
                this.waiting=false;
                this.showError(error);
            }
        });
    }
    put<T>(url:string,request:any,callback:callBack<T>){
        this.waiting=true;        
        this._http.put<T>(this.createUrl(url), request).subscribe({
            next: data => {
                this.waiting=false;
                callback(data);
            },
            error: error => {
                this.waiting=false;
                this.showError(error);
            }
        });
    }
    delete<T>(url:string,callback:callBack<T>){
        this.waiting=true;
        this._http.delete<T>(this.createUrl(url)).subscribe({
            next: data => {
                this.waiting=false;
                callback(data);
            },
            error: error => {
                this.waiting=false;
                this.showError(error);
            }
        });
    }
}


