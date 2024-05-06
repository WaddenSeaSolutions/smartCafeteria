import { Component, OnInit } from '@angular/core';
import {Service} from "../../service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-order-option-control',
  template: `

  <div *ngFor="let orderOption of service.orderOptions">
  <ion-card>
    <p>{{orderOption.OptionName}}</p>
    <p *ngIf="orderOption.active ? 'Aktiv' : 'ikke aktiv'">{{orderOption.active ? 'Aktiv' : 'ikke aktiv'}}</p>

  </ion-card>
  </div>


  `,
  styleUrls: ['./order-option-control.component.scss'],
})
export class OrderOptionControlComponent  implements OnInit {

  constructor(public service: Service)
  { }

  ngOnInit() {}

}
