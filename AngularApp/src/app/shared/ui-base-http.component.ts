import { Directive } from '@angular/core';
import { HttpClient,HttpClientModule } from "@angular/common/http";
import { HttpComponent } from "./http.component";
import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { ErrorDialog } from './error-dialog/error-dialog.component';

@Directive()
export abstract class HttpBaseComponent extends HttpComponent{
    constructor(protected _http:HttpClient,protected _translate: TranslateService, protected _router:Router,public dialog: MatDialog) {
        super(_http);
    }
    t(key:string):string{
        return this._translate.instant(key);
    }
    showError(error:any,width:number=360):void{
        const dialogRef = this.dialog.open(ErrorDialog, {
            width: `${width}px`,
            data: error
          });
      
          dialogRef.afterClosed().subscribe(result => {            
          });
    }
} 


  