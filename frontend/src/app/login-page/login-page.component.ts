import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import {WebsocketService} from "../../websocketService";
import { ToastController } from '@ionic/angular';

@Component({
  selector: 'app-login-page',
  template: `
    <div class="background"></div>

    <ion-content style="--background: none; position: absolute; top: 20%">
      <div style="margin-left: 25%; margin-right: 25%; padding: 2%; border: 1px solid grey ; text-align: center; ">
        <strong><p>Login side</p></strong>
        <ion-item>
          <br>
          <ion-input style="text-align: center; " placeholder="Brugernavn" [formControl]="username"></ion-input>
        </ion-item>
        <br>
        <ion-item>
          <ion-input type="password" style="text-align: center;" placeholder="Kodeord" [formControl]="password"></ion-input>
        </ion-item>
        <br>
        <div style="display: flex; justify-content: center;">
          <ion-button class="btnBackground" style="flex: 1; margin: 3%" (click)="login()">Login</ion-button>

        </div>
      </div>
    </ion-content>


  `,
  styleUrls: ['./login-page.component.scss'],
})
export class LoginPageComponent {
  username = new FormControl('', Validators.compose([Validators.min(5), Validators.max(20), Validators.required]));
  password = new FormControl('', Validators.compose([Validators.min(8), Validators.max(30), Validators.required]));

  myFormGroup = new FormGroup({
    username: this.username,
    password: this.password,
  });

  constructor(private router: Router, private websocketService: WebsocketService, private toastController: ToastController) {
     const checkIfLoggedIn = localStorage.getItem('token') != null;
     if (checkIfLoggedIn) {
       this.router.navigate(['home']);
     }
  }


  login() {
    if (this.myFormGroup.valid) {
      const loginMessage = {
        action: 'login',
        Username: this.myFormGroup.value.username,
        Password: this.myFormGroup.value.password,
      };
      console.log('Sending login message:', loginMessage);

      this.websocketService.sendData(loginMessage);
    }
  }
}
