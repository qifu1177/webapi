import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';  
import { RouterModule, Routes } from '@angular/router';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { ConfigLoaderModule } from 'projects/config-loader/';

import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {MatNativeDateModule} from '@angular/material/core';
import {AppMaterialModule} from './material-module';

import {PageNotFoundComponent} from './page-not-found/page-not-found.component';
import {UserLoginComponent} from './user-login/user-login.component';
import { UserRegisterComponent } from './user-register/user-register.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  {path: 'page-not-fount', component: PageNotFoundComponent},
  {path: 'home', component: HomeComponent},
  {path: 'user-login', component: UserLoginComponent},
  {path: 'user-register', component: UserRegisterComponent},
  { path: '',   redirectTo: '/home', pathMatch: 'full' },
  { path: '**', component: PageNotFoundComponent }

];

export function HttpLoaderFactory(http: HttpClient): TranslateHttpLoader {
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations:[
    PageNotFoundComponent,
    HomeComponent,
    UserLoginComponent,
    UserRegisterComponent
  ],
  imports: [
    RouterModule.forRoot(routes),
    ConfigLoaderModule.forRoot(),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),
    FormsModule,
    ReactiveFormsModule,
    MatNativeDateModule,
    AppMaterialModule,
    CommonModule,
    HttpClientModule],
  exports: [
    RouterModule,
    ConfigLoaderModule,
    TranslateModule,
    FormsModule,
    ReactiveFormsModule,
    MatNativeDateModule,
    AppMaterialModule,
    CommonModule,
    HttpClientModule
  ]
})
export class AppRoutingModule { }
