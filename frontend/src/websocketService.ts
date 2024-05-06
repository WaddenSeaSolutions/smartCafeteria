import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import {Service} from "./service";
import {OrderOption} from "./interface";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class WebsocketService {
  public socket: WebSocket;

  constructor(public service: Service, private router: Router) {
    this.socket = new WebSocket('ws://localhost:8181');
    this.socket.onopen = () => {
      this.authenticate();
      this.sendOrderOptionReadRequest();
    };

    this.socket.onmessage = (event) => {
      const response = JSON.parse(event.data);
      const property = Object.keys(response)[0];

      switch (property) {
        case 'InvalidToken':
          localStorage.removeItem('token');
          router.navigate(['login-page']);
          break;
        case 'OptionName':
          const orderOption: OrderOption = response.data;
          if (orderOption.isNew) {
            this.service.addOrderOption(orderOption);
          } else if (orderOption.IsUpdated) {
            this.service.updateOrderOption(orderOption);
          } else if (orderOption.IsDeleted) {
            this.service.deleteOrderOption(orderOption);
          }
          break;
        default:
          console.log('Server response:', event.data);
      }
    };
  }

  authenticate(): void {
    const token = localStorage.getItem('token');
    if (token) {
      const authMessage = {
        action: 'authenticate',
        token: token
      };
      this.sendData(authMessage);
    }
  }

  sendData(data: any): void {
    this.socket.send(JSON.stringify(data));
  }

  sendOrderOptionReadRequest(): void {
    const request = {
      action: 'orderOptionRead'
    };
    this.sendData(request);
  }


}
