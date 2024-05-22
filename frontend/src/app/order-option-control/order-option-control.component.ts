import { Component, OnInit } from '@angular/core';
import {Service} from "../../service";
import {Router} from "@angular/router";
import {WebsocketService} from "../../websocketService";
import {FormControl, Validators} from "@angular/forms";
import {OrderOption} from "../../interface";
import {home} from "ionicons/icons";
import {UpdateOrderOptionComponent} from "../update-order-option/update-order-option.component";
import {ModalController} from "@ionic/angular";

@Component({
  selector: 'app-order-option-control',
  template: `
    <div style="overflow-y: auto">
      <ion-button (click)="this.navigateToHome()" style="margin: 1%">
      <ion-icon icon="home" style="font-size: 300% "></ion-icon>
      </ion-button>
    <ion-item>
      <ion-input style="flex: 1" [formControl]="optionName" placeholder="Skriv navn på salat ingrediens her"></ion-input>
      <ion-button style="flex: 1" (click)="createMenuOption()">Opret ny ingrediens</ion-button>
    </ion-item>
  <div *ngFor="let orderOption of this.service.orderOptions">
  <ion-card>
    <div style="margin: 1%">
    <ion-title>{{orderOption.optionName}}</ion-title>
    <ion-title *ngIf="orderOption.active ? 'Aktiv' : 'ikke aktiv'">Status: {{orderOption.active ? 'Aktiv' : 'ikke aktiv'}}</ion-title>

      <ion-item>
      <ion-button (click)="updateOrderOption(orderOption)">Opdater</ion-button>
      <ion-button (click)="deleteOrderOption(orderOption)">Slet</ion-button>
    </ion-item>
    </div>
  </ion-card>
  </div>
    </div>


  `,
  styleUrls: ['./order-option-control.component.scss'],
})
export class OrderOptionControlComponent {
  optionName = new FormControl('', Validators.compose([Validators.min(2), Validators.max(20), Validators.required]));

  myFormGroup = new FormControl({
    OptionName: this.optionName,
  });

  constructor(public service: Service, private websocketService: WebsocketService, private router: Router, private modalController: ModalController) {

  }


  createMenuOption() {
    if (this.myFormGroup.valid) {
      const createMenuOptionMessage = {
        action: 'orderOptionCreate',
        OptionName: this.optionName.value,
      };
      console.log('Sending createMenuOption message:', createMenuOptionMessage);
      this.websocketService.sendData(createMenuOptionMessage);
    }
  }

  deleteOrderOption(orderOption: OrderOption) {
    const deleteOrderOptionMessage = {
      action: 'orderOptionDelete',
      Id: orderOption.id,
    };
    this.websocketService.sendData(deleteOrderOptionMessage);
  }

  navigateToHome() {
    this.router.navigate(['home']);
  }

  protected readonly home = home;


    async updateOrderOption(orderOption: OrderOption) {
        const modal = await this.modalController.create({
            component: UpdateOrderOptionComponent,
            componentProps: {
                orderOption: orderOption
            }
        });

        await modal.present();

        // Wait for the modal to be dismissed
        await modal.onWillDismiss();
    }
}
