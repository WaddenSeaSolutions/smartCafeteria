import { Component, OnInit } from '@angular/core';
import {AbstractControl, AsyncValidatorFn, FormControl, ValidationErrors, Validators} from "@angular/forms";
import {HttpClient, HttpResponse} from "@angular/common/http";
import {Router} from "@angular/router";
import {ToastController} from "@ionic/angular";
import {environment} from "../../environments/environment";
import {catchError, map, Observable, of} from "rxjs";
import {UsersRegister} from "../register/register.component";
import { webSocket, WebSocketSubject } from 'rxjs/webSocket';
import {WebsocketService} from "../../websocketService";


@Component({
  selector: 'app-register-personnel',
  template:`
    <div class="background"></div>
    <ion-content style="--background: none; position: absolute; top: 30%">
      <div style="margin-left: 25%; margin-right: 25%; padding: 2%; border: 1px solid grey ; text-align: center; ">
        <header style="text-align: center; font-size: 20px"> Registrer en person</header>
        <ion-item>
          <ion-input [debounce]="1500" style="text-align: center" placeholder="Brugernavn"
                     [formControl]="username"></ion-input>
        </ion-item>
        <div *ngIf="username.hasError('minlength')">
          <p>Brugernavn skal være mindst 5 tegn</p>
        </div>
        <div *ngIf="username.hasError('maxlength')">
          <p>Brugernavn skal være højest 20 tegn</p>
        </div>
        <div *ngIf="username.hasError('usernameExists')">
          <p>Dette brugernavn anvendes allerede</p>
        </div>
        <br>
        <ion-item>
          <ion-input type="password" style="text-align: center" placeholder="Kodeord"
                     [formControl]="password"></ion-input>
        </ion-item>
        <br>
        <ion-item>
          <ion-input type="password" style="text-align: center" placeholder="Gentag Kodeord"
                     [formControl]="password2"></ion-input>
        </ion-item>
        <div *ngIf="password.hasError('minlength')">
          <p>Kodeordet skal være minimum 8 lang</p>
        </div>
        <div *ngIf="password.hasError('maxlength')">
          <p>Kodeordet må maks være 30 tegn</p>
        </div>
        <div *ngIf="password2.hasError('passwordsNotMatch')">
          <p>De indtastede kodeord er ikke ens</p>
        </div>
        <br>
        <ion-button class="btnBackground" style="display: flex" (click)="registerUser()"
                    [disabled]="password2.value?.length! < 8 || password.value?.length! < 8 || password.value !== password2.value || username.value?.length! < 5">
          Registrer din konto
        </ion-button>
      </div>
    </ion-content>



  `,
  styleUrls: ['./register-personnel.component.scss'],
})
export class RegisterPersonnelComponent {

  formIsValid(): boolean {
    return this.username.valid && this.password.valid && this.password2.valid;
  }

  username = new FormControl('', {
    validators: [
      Validators.required,
      Validators.minLength(5),
      Validators.maxLength(20)],
    asyncValidators: [this.nameValidator()],
    updateOn: 'change' // Validation will be triggered after changes
  });
  password = new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(30)]);
  password2 = new FormControl('', [Validators.required, this.matchingPasswords.bind(this)]);

  myFormGroup = new FormControl({
    username: this.username,
    password: this.password,
  });

  public checkIfLoggedIn: boolean;

  // Method that validates that the password matches and ensures no typos in password
  matchingPasswords(control: FormControl): { [key: string]: boolean } | null {
    if (this.password && control.value !== this.password.value) {
      return {'passwordsNotMatch': true};
    }
    return null;
  }

  constructor(private websocketService: WebsocketService, private router: Router, private toastController: ToastController) {
    this.checkIfLoggedIn = localStorage.getItem('token') != null;
    // if (this.checkIfLoggedIn){
    //   this.router.navigate(['home'])
    // }
  }

  nameValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {
      return new Promise<ValidationErrors | null>((resolve) => {
        this.websocketService.sendData({action: 'checkUsername', username: control.value});

        this.websocketService.socket.onmessage = (event) => {
          const response = JSON.parse(event.data);

          if (response.action === 'checkUsername') {
            resolve(response.exists ? {usernameExists: true} : null);
          }
        }
      });
    };
  }

// Method to register the new user
  async registerUser() {
    const registrant = {
      username: this.username.value,
      password: this.password.value,
    }
    try {
      this.websocketService.sendData(registrant);

      this.websocketService.socket.onmessage = (event) => {
        const response = JSON.parse(event.data);

        if (response.ok) {
          this.okResponse("Din konto blev oprettet")
          // Proceed to login-page if the request was successful
          this.router.navigate(["login-page"]);
        } else {
          this.errorResponse("Noget gik galt")
        }
      }
    } catch (error) {
      this.errorResponse("Noget gik galt")
    }
  }


  async okResponse(message: string, duration: number = 2000) {
    const toast = await this.toastController.create({
      message: message,
      duration: duration,
      position: 'bottom', // Displays in the bottom
      color: 'success', // Green Color for ok response
      buttons: [
        {
          text: 'Close',
          role: 'cancel',
          handler: () => {
            console.log('Toast dismissed');
          }
        }
      ]
    });

    toast.present();
  }

  async errorResponse(message: string, duration: number = 2000) {
    const toast = await this.toastController.create({
      message: message,
      duration: duration,
      position: 'bottom', // Displays in the bottom
      color: 'warning', // Green Color for ok response
      buttons: [
        {
          text: 'Close',
          role: 'cancel',
          handler: () => {
            console.log('Toast dismissed');
          }
        }
      ]
    });

    toast.present();
  }
}
