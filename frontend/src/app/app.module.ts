import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import {LoginPageComponent} from "./login-page/login-page.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {HttpClientModule} from "@angular/common/http";
import {RegisterPersonnelComponent} from "./register-personnel/register-personnel.component";
import {OrderOptionControlComponent} from "./order-option-control/order-option-control.component";
import {UpdateOrderOptionComponent} from "./update-order-option/update-order-option.component";


@NgModule({
  declarations: [AppComponent, LoginPageComponent, RegisterPersonnelComponent, OrderOptionControlComponent, UpdateOrderOptionComponent],
    imports: [BrowserModule, IonicModule.forRoot(), AppRoutingModule, ReactiveFormsModule, HttpClientModule, FormsModule],
  providers: [{ provide: RouteReuseStrategy, useClass: IonicRouteStrategy }],
  bootstrap: [AppComponent],
})
export class AppModule {}
