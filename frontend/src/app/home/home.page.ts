import { Component } from '@angular/core';
import {Router} from "@angular/router";
import {WebsocketService} from "../../websocketService";
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
    <div>
      <ion-button (click)="navigateToOrderOption()">Ændre salat muligheder</ion-button>
    </div>

    <div style="display: flex; flex-wrap: wrap;">
        <div *ngFor="let order of this.service.orders.slice().reverse()" style="flex: 0 0 calc(33.33% - 10px); margin: 5px;">
            <ion-card style="box-shadow: 0px 0px 10px rgba(0,0,0,0.5);" [ngStyle]="{'border': order.Done ? '1px solid green' : '1px solid red'}">
                <div style="margin: 1%">
                    <ion-title>{{order.Id}}</ion-title>
                    <ion-title>{{order.Payment ? 'Betalt' : 'ikke Betalt'}}</ion-title>
                    <ion-title>Bestilt: {{order.Timestamp}}</ion-title>


                    <div *ngFor="let option of order.OrderOptions">
                        <ion-title>{{option.optionName}}</ion-title>
                    </div>
                </div>
            </ion-card>
        </div>
    </div>
  `,
  styleUrls: ['home.page.scss'],
})
export class HomePage {

  public checkIfAdmin: boolean;

  constructor(private router: Router, private websocketService: WebsocketService, public service: Service) {
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

  async navigateToOrderOption() {
    this.router.navigate(['order-option-control']);
  }


}
