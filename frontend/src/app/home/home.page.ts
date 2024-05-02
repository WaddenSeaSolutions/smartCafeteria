import { Component } from '@angular/core';
import {Router} from "@angular/router";
import {WebsocketService} from "../../websocketService";

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

  constructor(private router: Router, private websocketService: WebsocketService) {
    //Checks if the user is an admin role, if not the user should not be shown the admin
    this.checkIfAdmin = localStorage.getItem('role') === 'admin';

    // Handle WebSocket messages
    this.websocketService.socket.onmessage = (event) => {
      const response = JSON.parse(event.data);
      // Handle the response here
    };
  }

  async openCreatePersonnel(){
    // Send a WebSocket message
    this.websocketService.sendData({action: 'openCreatePersonnel'});
    this.router.navigate(['register-personnel'])
  }
}
