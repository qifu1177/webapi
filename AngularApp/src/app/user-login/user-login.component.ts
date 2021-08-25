import { Component, OnInit } from '@angular/core';
import { UserLoginRequest } from 'src/models/requests/UserLoginRequest';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { FormErrorStateMatcher } from '../shared/form-error-state-matcher';
import { HttpBaseComponent } from '../shared/ui-base-http.component';
import { UserLoginResponse } from 'src/models/responses/UserLoginResponse';
import { TranslateService } from '@ngx-translate/core';


@Component({
  selector: 'app-page-user-login',
  templateUrl: './user-login.component.html'
})
export class UserLoginComponent extends HttpBaseComponent implements OnInit {
  title!: string;
  request!: UserLoginRequest;

  matcher: FormErrorStateMatcher = new FormErrorStateMatcher();
  response!: UserLoginResponse;
  errorMessage!: string;
  loginNameFormControl!: FormControl;
  passwordFormControl!: FormControl;

  ngOnInit() {
    this.request = { loginName: "", password: "" };
    this.loginNameFormControl = new FormControl(this.request.loginName, [Validators.required]);
    this.passwordFormControl = new FormControl(this.request.password, [Validators.required]);
  }
  login() {
    this._http.post<UserLoginResponse>(this.createUrl('Users/login'), this.request).subscribe({
      next: data => {
        this.response = data;
      },
      error: error => {
        this.errorMessage = error.message;
        console.error('There was an error!', error);
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
