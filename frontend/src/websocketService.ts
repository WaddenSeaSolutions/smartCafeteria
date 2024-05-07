import { Injectable } from '@angular/core';
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

      if (Array.isArray(response) && response.length > 0 && response[0].OptionName) {
        this.service.orderOptions = response;
        console.log(this.service.orderOptions);
      } else {
        if (response.hasOwnProperty('InvalidToken')) {
          localStorage.removeItem('token');
          this.router.navigate(['login-page']);
        } else if (response.hasOwnProperty('OptionName')) {
          const orderOption: OrderOption = response.OptionName;
          if (orderOption.isNew) {
            this.service.addOrderOption(orderOption);
          } else if (orderOption.IsUpdated) {
            this.service.updateOrderOption(orderOption);
          } else if (orderOption.IsDeleted) {
            this.service.deleteOrderOption(orderOption);
          }
        } else {
          console.log('Server response:', event.data);
        }
      }
    };
  }

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
