import {Injectable} from '@angular/core';
import {Service} from "./service";
import {ToastController} from "@ionic/angular";
import {Router} from "@angular/router";
import {Environment} from "@angular/cli/lib/config/workspace-schema";
import {environment} from "./environments/environment";

@Injectable({
  providedIn: 'root'
})
export class WebsocketService {
  public socket: WebSocket;

  constructor(public service: Service, public toast: ToastController, public router: Router) {
    this.socket = new WebSocket(environment.baseUrl);
    this.handleEventsEmittedByTheServer();

    this.socket.onopen = () => {
    this.authenticate();
    this.sendData({action: 'orderOptionRead'});
    this.sendData({action: 'orderReadHandler'})
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

  orderUpdated(data: any): void {
    console.log(data.order);
    const updatedOrder = data.order;
    // Find the index of the order to be updated
    const index = this.service.orders.findIndex(order => order.Id === updatedOrder.Id);
    // If the order is found, update its boolean fields
    if (index !== -1) {
      this.service.orders[index].Payment = updatedOrder.Payment;
      this.service.orders[index].Done = updatedOrder.Done;
    }
  }

    ordersRead(data: any): void {
    this.service.orders = data.orders;
  }

  orderOptions(data: any): void {
    this.service.orderOptions = data.orderOptions;
  }

  orderOptionCreated(data: any): void {
    this.service.addOrderOption(data.orderOption);
  }

  orderOptionDeleted(data: any): void {
    this.service.deleteOrderOption(data.orderOption);
  }

    orderOptionUpdated(data: any): void {
        this.service.updateOrderOption(data.orderOption);
    }

  successfulLogin(data: any) {
    localStorage.setItem('token', data.token);
    let payload = JSON.parse(atob(data.token.split(".")[1]))
    //Store the role, only allows for visual admin controls
    localStorage.setItem('role', payload.role)
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
