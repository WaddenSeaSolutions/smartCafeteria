import { Component, OnInit } from '@angular/core';
import {Service} from "../../service";
import {Router} from "@angular/router";
import {WebsocketService} from "../../websocketService";
import {FormControl, Validators} from "@angular/forms";
import {OrderOption} from "../../interface";

@Component({
  selector: 'app-order-option-control',
  template: `
    <div style="overflow-y: auto">
    <ion-item>
      <ion-input [formControl]="optionName" placeholder="Skriv navn på salat ingrediens her"></ion-input>
      <ion-button (click)="createMenuOption()"></ion-button>
    </ion-item>

  <div *ngFor="let orderOption of this.service.orderOptions">
  <ion-card>
    <p>{{orderOption.OptionName}}</p>
    <p *ngIf="orderOption.Active ? 'Aktiv' : 'ikke aktiv'">{{orderOption.Active ? 'Aktiv' : 'ikke aktiv'}}</p>
    <ion-item>
      <ion-button (click)="updateOrderOption(orderOption)">Opdater</ion-button>
      <ion-button (click)="deleteOrderOption(orderOption)">Slet</ion-button>
    </ion-item>
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
  constructor(public service: Service, private websocketService: WebsocketService)
  {

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

  updateOrderOption(orderOption: OrderOption) {
    const updateOrderOptionMessage = {
      action: 'orderOptionUpdated',
      Id: orderOption.Id,
      OptionName: orderOption.OptionName,
      Active: orderOption.Active,
    };
    console.log('Sending updateOrderOption message:', updateOrderOptionMessage);
    this.websocketService.sendData(updateOrderOptionMessage);
  }

  deleteOrderOption(orderOption: OrderOption) {
    const deleteOrderOptionMessage = {
      action: 'orderOptionDelete',
      Id: orderOption.Id,
    };
    this.websocketService.sendData(deleteOrderOptionMessage);
    }
  }
