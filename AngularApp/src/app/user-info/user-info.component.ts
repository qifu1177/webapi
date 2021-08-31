import { Component, OnInit } from '@angular/core';
import { AutoToLoginComponent } from 'src/app/shared/ui-auto-to-login.component';
import { UserSessionResponse } from 'src/models/responses/UserSessionResponse';
import { UserRequest } from 'src/models/requests/UserRequest';
import { Store } from 'projects/store';

@Component({
  selector: 'app-page-user-info',
  templateUrl: './user-info.component.html'
})
export class UserInfoComponent extends AutoToLoginComponent implements OnInit {
  response!: UserSessionResponse;

  ngOnInit() {
    this.loadResponse();
  }
  loadResponse() {
    let sessionId=Store.get('user').sessionId;
    this.get<UserSessionResponse>(`Users/${sessionId}`, (data) => {
      this.response = data;
      Store.action('user', 'updateSession')(data);
    });
  }
}
