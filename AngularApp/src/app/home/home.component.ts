import { Component,OnInit } from '@angular/core';
import {AutoToLoginComponent} from 'src/app/shared/ui-auto-to-login.component';

@Component({
  selector: 'app-page-home',
  templateUrl: './home.component.html'
})
export class HomeComponent extends AutoToLoginComponent implements OnInit {
  ngOnInit(){
    
  }
}
