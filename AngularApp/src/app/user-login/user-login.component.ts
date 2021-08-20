import { Component, OnInit } from '@angular/core';
import { GlobalConstants } from 'src/common/global-constants';

@Component({
  selector: 'app-page-user-login',
  templateUrl: './user-login.component.html'
})
export class UserLoginComponent implements OnInit{
    public title:string="";
    ngOnInit(){
        this.title=GlobalConstants.title;
    }
}
