import {Component, Input, OnInit} from '@angular/core';
import {ModalController} from "@ionic/angular";
import {OrderOption} from "../../interface";
import {HttpClient} from "@angular/common/http";
import {WebsocketService} from "../../websocketService";
import {FormControl, Validators} from "@angular/forms";
import {body} from "ionicons/icons";

@Component({
    selector: 'app-update-order-option',
    template: `
        <ion-header>
            <ion-toolbar>
                <ion-title>Rediger indgrediens</ion-title>
                <ion-buttons slot="end">
                    <ion-button (click)="dismissModal()">Luk vindue</ion-button>
                </ion-buttons>
            </ion-toolbar>
        </ion-header>
        <ion-content>
            <form (ngSubmit)="submitForm()">
                <div style="margin: 1%;">
                    <ion-title>
                        <ion-input [formControl]="UpdatedOrderOptionName"></ion-input>
                    </ion-title>

                    <ion-title>Status: <ion-checkbox [formControl]="updatedOrderOptionActive" [(ngModel)]="orderOption.Active" checked></ion-checkbox></ion-title>

                    <br>

                    <ion-button type="submit">Gem ændringer</ion-button>
                </div>
            </form>
        </ion-content>

    `,
    styleUrls: ['./update-order-option.component.scss'],
})
export class UpdateOrderOptionComponent implements OnInit{
    @Input() orderOption!: OrderOption;
    UpdatedOrderOptionName = new FormControl('', Validators.compose([Validators.min(2), Validators.max(20), Validators.required]));
    updatedOrderOptionActive = new FormControl('', Validators.compose([Validators.required]));

    constructor(private modalController: ModalController, private webSocketService: WebsocketService) {}
    ngOnInit(): void {
        this.UpdatedOrderOptionName.setValue(this.orderOption.OptionName);
    }
    dismissModal() {
        this.modalController.dismiss();
    }

    submitForm() {
        // Call the updateOrderOption method when the form is submitted
        this.updateOrderOption();
    }

    updateOrderOption() {
        const body = {
            action: 'orderOptionUpdate',
            Id: this.orderOption.Id,
            OptionName: this.UpdatedOrderOptionName.value,
            Active: this.updatedOrderOptionActive.value
        };
        this.webSocketService.sendData(body);
        // Dismiss the modal after sending the update message
        this.modalController.dismiss({ updatedOrderOption: this.orderOption });
    }


}
