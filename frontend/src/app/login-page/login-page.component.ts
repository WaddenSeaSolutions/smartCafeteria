import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {environment} from "../../environments/environment";

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
  username = new FormControl('', Validators.compose([Validators.min(5), Validators.max(20), Validators.required]))
  password = new FormControl('', Validators.compose([Validators.min(8), Validators.max(30), Validators.required]))

  public checkIfLoggedIn: boolean;

  myFormGroup = new FormGroup({
    username: this.username,
    password: this.password,
  })

  constructor(private http: HttpClient, private router: Router) {
    this.checkIfLoggedIn = localStorage.getItem('token') != null;

  //   if (this.checkIfLoggedIn){
  //     this.router.navigate(['home'])
  //   }
  }

  login() {
    if (this.myFormGroup.valid || true) {
      this.http.post(environment.baseUrl + '/login', this.myFormGroup.value, {responseType: 'text'})
        .subscribe({
          next: (response) => {
            if (response) {
              // store token
              localStorage.setItem('token', response);
              let payload = JSON.parse(atob(response.split(".")[1]))
              //Store the role, only allows for visual admin controls
              localStorage.setItem('role', payload.role)
              //Go to homepage after successful login
              this.router.navigate(["home"])
              location.reload();
            }
          },
          error: (err) => {
            console.error(err);
          }
        });
    }
  }

  async registerNewUser() {
    await this.router.navigate(['register'])
  }
}

