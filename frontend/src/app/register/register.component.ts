import { Component} from '@angular/core';
import {AbstractControl, FormControl, Validators} from "@angular/forms";
import {HttpClient, HttpResponse} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {navigate} from "ionicons/icons";
import {Router} from "@angular/router";
import {ToastController} from "@ionic/angular";
import {catchError, map, Observable, of} from "rxjs";




@Component({
  selector: 'app-register',
  template:`
      <div class="background"></div>
    <ion-content style="--background: none; position: absolute; top: 30%">
      <div style="margin-left: 25%; margin-right: 25%; padding: 2%; border: 1px solid grey ; text-align: center; ">
      <header style="text-align: center; font-size: 20px"> Registrer en konto</header>
          <ion-item>
            <ion-input [debounce]="1500" style="text-align: center" placeholder="Brugernavn" [formControl]="username"> </ion-input>
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
            <ion-input type="password" style="text-align: center" placeholder="Kodeord" [formControl]="password"> </ion-input>
          </ion-item>
          <br>
          <ion-item>
            <ion-input type="password" style="text-align: center" placeholder="Gentag Kodeord" [formControl]="password2"> </ion-input>
          </ion-item>
         <div *ngIf="password.hasError('minlength')" >
          <p>Kodeordet skal være minimum 8 lang</p>
         </div>
         <div *ngIf="password.hasError('maxlength')" >
          <p>Kodeordet må maks være 30 tegn</p>
         </div>
          <div *ngIf="password2.hasError('passwordsNotMatch')">
            <p>De indtastede kodeord er ikke ens</p>
          </div>
          <br>
         <ion-button class="btnBackground" style="display: flex" (click)="registerUser()" [disabled]="!formIsValid()">Registrer din konto</ion-button>
        </div>
      </ion-content>

    `,
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent{

  formIsValid(): boolean {
    // Here you can add other checks if necessary
    return this.username.valid && this.password.valid && this.password2.valid;
  }

  username = new FormControl('', {
    validators: [
      Validators.required,
      Validators.minLength(5),
      Validators.maxLength(20)],
    asyncValidators: [this.nameValidator(this.http)],
    updateOn: 'change' // Validation will be triggered after changes
  });
  password = new FormControl('',[Validators.required, Validators.minLength(8),Validators.maxLength(30)]);
  password2 = new FormControl('', [Validators.required, this.matchingPasswords.bind(this)]);

  myFormGroup = new FormControl({
    username: this.username,
    password: this.password,
  });

  public checkIfLoggedIn: boolean;

  // Method that validates that the password matches and ensures no typos in password
  matchingPasswords(control: FormControl): { [key: string]: boolean } | null {
    if (this.password && control.value !== this.password.value) {
      return { 'passwordsNotMatch': true };
    }
    return null;
  }

  constructor(private http : HttpClient, private router: Router, private toastController : ToastController) {
    this.checkIfLoggedIn = localStorage.getItem('token') != null;
    // if (this.checkIfLoggedIn){
    //   this.router.navigate(['home'])
    // }
  }

  nameValidator(http: HttpClient) {
    return (control: AbstractControl) => {
      return http.post<boolean>(`${environment.baseUrl}/checkUsername,${control.value}`, {})
        .pipe(
          map(result => {
            return (result === true ? { usernameExists: true } : null);
          }),
          catchError(() => of(null))
        );
    };
  }

// Method to register the new user
  async registerUser(){
    const registrant = {
      username: this.username.value,
      password: this.password.value,
    }
    try {
      let response = new HttpResponse();
      await this.http.post<UsersRegister>(environment.baseUrl + '/register', registrant).toPromise();

      if (response.ok)
      {
        this.okResponse("Din konto blev oprettet")
        // Proceed to login-page if the request was successful
        this.router.navigate(["login-page"]);
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



export interface UsersRegister {
  username: string;
  password: string;
}

