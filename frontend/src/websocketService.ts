import {Injectable} from '@angular/core';
import {Observable, Subject} from 'rxjs';
import {Service} from "./service";
import {ToastController} from "@ionic/angular";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class WebsocketService {
  public socket: WebSocket;

  constructor(public service: Service, public toast: ToastController, public router: Router) {
    this.socket = new WebSocket('ws://localhost:8181');
    this.handleEventsEmittedByTheServer();

    this.socket.onopen = () => {
    this.authenticate();
    this.sendData({action: 'orderOptionRead'});
    }
  }

  sendData(data: any): void {
    this.socket.send(JSON.stringify(data));
  }

  handleEventsEmittedByTheServer() {
    this.socket.addEventListener('message', (event) => {
      const data = JSON.parse(event.data) as any;
      console.log("Received: " + JSON.stringify(data));
      this.handleErrorResponse(data);
      //@ts-ignore
      this[data.eventType]?.call(this, data);
    });
    this.socket.onerror = (err) => {
      console.error(err);
    }
  }

  ordersRead(data: any): void {
    console.log(data.orderOptions);
    this.service.orders = data.orders;
  }

  orderOptions(data: any): void {
    console.log(data.orderOptions); // Add this line to check the received data
    this.service.orderOptions = data.orderOptions;
  }

  orderOptionCreated(data: any): void {
    console.log(data.orderOption.OptionName)
    this.service.addOrderOption(data.orderOption);
  }

  orderOptionDeleted(data: any): void {
    console.log(data.orderOption)
    this.service.deleteOrderOption(data.orderOption);
  }

    orderOptionUpdated(data: any): void {
        console.log(data.orderOption)
        this.service.updateOrderOption(data.orderOption);
    }

  successfulLogin(data: any) {
    console.log(data)
    localStorage.setItem('token', data.token);
    this.router.navigate(['home']);
  }


async errorResponse(data: any) {
    const toast = await this.toast.create({
      message: 'Something went wrong'
    });
    toast.present();

};
  authenticate(): void {
    const token = localStorage.getItem('token');
    if (token !== null) {
      const authMessage = {
        action: 'authentication',
        token: token
      };
      this.sendData(authMessage);
    }
    else {
      this.router.navigate(['login-page']);
    }
  }

  handleErrorResponse(data: any): void {
    if (data.status === 'error' && data.InvalidToken) {
      localStorage.removeItem('token');
    }
  }
  registerPersonnel(data: any) {
    if (data.response == 'ok') {

    }
  }
}
