import { Component } from '@angular/core';
import {Router} from "@angular/router";
import {WebsocketService} from "../../websocketService";
import {Service} from "../../service";
import {Order} from "../../interface";
import {FormControl, Validators} from "@angular/forms";

@Component({
  selector: 'app-home',
  template: `
    <ion-content style="--background: none; position: absolute; display: contents">
    </ion-content>
    <ion-item>
      <ion-button *ngIf="checkIfAdmin" (click)="openCreatePersonnel();">Opret nyt personale</ion-button>
      <ion-button (click)="navigateToOrderOption()">Ændre salat muligheder</ion-button>
      <ion-button (click)="removeToken()">Log ud</ion-button>
    </ion-item>

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
                        <ion-item><ion-checkbox (click)="updateOrderPayment(order, $event)" (ionChange)="logBox($event, order)" [checked]="order.Payment">Betalt?</ion-checkbox></ion-item>
                        <ion-item><ion-checkbox (click)="updateOrderDone(order, $event)" (ionChange)="logBox($event, order)" [checked]="order.Done">Færdig?</ion-checkbox></ion-item>
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


  removeToken() { // Method for logging out
    // Remove the token from the local storage
    localStorage.removeItem('token');
    this.router.navigate(['login-page']);
  }

  updateOrderPayment(order: Order, event: any) {
    console.log(event)
    this.websocketService.sendData({
      "action": "orderUpdatePaymentHandler",
      "Id": order.Id,
      "Payment": !order.Payment, // Negate the current state
    });
  }

  updateOrderDone(order: Order, event: any) {
    console.log(event)
    this.websocketService.sendData({
      "action": "orderUpdateDoneHandler",
      "Id": order.Id,
      "Done": !order.Done, // Negate the current state
    });
  }

  logBox(e: any, obj: any) {
    console.log(e)
    console.log(obj)
  }
}
