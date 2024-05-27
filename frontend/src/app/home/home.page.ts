import { Component } from '@angular/core';
import {Router} from "@angular/router";
import {WebsocketService} from "../../websocketService";
import {Service} from "../../service";

@Component({
  selector: 'app-home',
  template: `
    <ion-content style="--background: none; position: absolute; display: contents">
      <ion-card  *ngIf="checkIfAdmin">
        <ion-title>Modereringskontrol:</ion-title>
        <ion-button (click)="openCreatePersonnel();">Opret nyt personale</ion-button>
      </ion-card>
    </ion-content>
    <div>
      <ion-button (click)="navigateToOrderOption()">Ændre salat muligheder</ion-button>
      <ion-button (click)="removeToken()">Log ud</ion-button>
    </div>

    <div style="overflow-y: auto">
    <ion-grid>
      <ion-row>
        <ion-col size="3" *ngFor="let order of this.service.orders" style="margin: 5px;">
            <ion-card style="box-shadow: 0px 0px 10px rgba(0,0,0,0.5);" [ngStyle]="{'border': order.Done ? '1px solid green' : '1px solid red'}">
                <div style="margin: 1%; display: contents">
                    <ion-title>Odre Id: {{order.Id}}</ion-title>
                    <ion-title>{{order.Payment ? 'Betalt' : 'ikke Betalt'}}</ion-title>
                    <ion-title>Bestilt: {{order.Timestamp}}</ion-title>
                    <br>
                    <ion-title>Indhold: </ion-title>
                    <div style="border-top: 2px solid grey">
                    <div *ngFor="let option of order.OrderOptions">
                        <ion-title>{{option.OptionName}}</ion-title>
                    </div>
                      <div>
                        <ion-item><ion-checkbox>Betalt?</ion-checkbox></ion-item>
                        <ion-item><ion-checkbox>Færdig?</ion-checkbox></ion-item>
                        <ion-item><ion-button style="flex: auto">Opdater</ion-button></ion-item>
                      </div>
                </div>
                </div>
            </ion-card>
        </ion-col>
      </ion-row>
    </ion-grid>
    </div>
  `,
  styleUrls: ['home.page.scss'],
})
export class HomePage {
  public checkIfAdmin: boolean;

  constructor(private router: Router, private websocketService: WebsocketService, public service: Service) {


    //Checks if the user is an admin role, if not the user should not be shown the admin options
    this.checkIfAdmin = localStorage.getItem('role') === 'admin';
  }

  async openCreatePersonnel(){
    // Send a WebSocket message
    this.websocketService.sendData({action: 'openCreatePersonnel'});
    this.router.navigate(['register-personnel'])
  }

  async navigateToOrderOption() {
    this.router.navigate(['order-option-control']);
  }


  removeToken() {
    // Remove the token from the local storage
    localStorage.removeItem('token');
    this.router.navigate(['login-page']);
  }
}
