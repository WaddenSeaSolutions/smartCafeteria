import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import {Service} from "./service";

@Injectable({
  providedIn: 'root'
})
export class WebsocketService {
  public socket: WebSocket;

  constructor(public service: Service) {
    this.socket = new WebSocket('ws://localhost:8181');
  }

  sendData(data: any): void {
    this.socket.send(JSON.stringify(data));
  }
}
