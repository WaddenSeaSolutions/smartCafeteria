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
    this.sendOrderOptionReadRequest();
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
    console.log(this.service.orderOptions);

    this.socket.onerror = (err) => {
      console.error(err);
    }
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
  }

  handleErrorResponse(data: any): void {
    if (data.status === 'error' && data.InvalidToken) {
      localStorage.removeItem('token');
    }
  }

  sendOrderOptionReadRequest(): void {
    const request = {
      action: 'orderOptionRead'
    };
    this.sendData(request);
  }
  registerPersonnel(data: any) {
    if (data.response == 'ok') {

    }
  }
}
