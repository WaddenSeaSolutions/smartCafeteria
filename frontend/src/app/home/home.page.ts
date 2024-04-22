import { Component } from '@angular/core';
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {Service} from "../../service";

@Component({
  selector: 'app-home',
  template: `
    <ion-content style="--background: none; position: absolute;">
      <ion-card  *ngIf="checkIfAdmin">
        <ion-title>Modereringskontrol:</ion-title>
        <ion-button (click)="openCreatePersonnel();">Opret nyt personale</ion-button>
      </ion-card>
    </ion-content>



  `,
  styleUrls: ['home.page.scss'],
})
export class HomePage {

  public checkIfAdmin: boolean;

  constructor(private http: HttpClient, public service: Service, private router: Router) {
    //Checks if the user is an admin role, if not the user should not be shown the admin
    this.checkIfAdmin = localStorage.getItem('role') === 'admin';

  }

  async openCreatePersonnel(){
    this.router.navigate(['register-personnel'])
  }



}
