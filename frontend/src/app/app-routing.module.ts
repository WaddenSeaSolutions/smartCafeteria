import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import {LoginPageComponent} from "./login-page/login-page.component";
import {RegisterPersonnelComponent} from "./register-personnel/register-personnel.component";
import {OrderOptionControlComponent} from "./order-option-control/order-option-control.component";

const routes: Routes = [
  {
    path: 'home',
    loadChildren: () => import('./home/home.module').then( m => m.HomePageModule)
  },
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full'
  },
  {
    path: 'login-page',
    component: LoginPageComponent
  },
  {
    path: 'register-personnel',
    component: RegisterPersonnelComponent
  },
  {
    path: 'order-option-control',
    component: OrderOptionControlComponent
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
