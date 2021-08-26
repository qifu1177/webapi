import { Component, OnInit } from '@angular/core';
import { UserLoginRequest } from 'src/models/requests/UserLoginRequest';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { FormErrorStateMatcher } from '../shared/form-error-state-matcher';
import { HttpBaseComponent } from '../shared/ui-base-http.component';
import { UserLoginResponse } from 'src/models/responses/UserLoginResponse';
import { Md5 } from 'ts-md5';


@Component({
  selector: 'app-page-user-login',
  templateUrl: './user-login.component.html'
})
export class UserLoginComponent extends HttpBaseComponent implements OnInit {
  title!: string;
  request!: UserLoginRequest;

  matcher: FormErrorStateMatcher = new FormErrorStateMatcher();
  response!: UserLoginResponse;
  loginNameFormControl!: FormControl;
  passwordFormControl!: FormControl;

  ngOnInit() {
    this.request = { loginName: "", password: "" };
    this.loginNameFormControl = new FormControl(this.request.loginName, [Validators.required]);
    this.passwordFormControl = new FormControl(this.request.password, [Validators.required]);
  }
  login() {
    let password=Md5.hashStr(this.passwordFormControl.value);
    let loginRequset:UserLoginRequest={loginName:this.loginNameFormControl.value,password:`${password}`};
    this._http.post<UserLoginResponse>(this.createUrl('Users/login'),loginRequset ).subscribe({
      next: data => {
        this.response = data;
      },
      error: error => {
        this.showError(error);        
      }
    });
  }
  register() {
    this._router.navigate(["user-register"]);
  }
  invalid():boolean{
    return this.loginNameFormControl.invalid || this.passwordFormControl.invalid;
  }
}
