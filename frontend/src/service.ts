import {Injectable} from "@angular/core";
import {Order, OrderOption, Users} from "./interface";

@Injectable({
  providedIn: 'root'
})
export class Service{
  users: Users | undefined;
  orderOptions: OrderOption[] = [];
  orders: Order[] = [];

  addOrderOption(orderOption: OrderOption): void {
    this.orderOptions.push(orderOption);
  }

  updateOrderOption(orderOption: OrderOption): void {
    const index = this.orderOptions.findIndex(option => option.Id === orderOption.Id);
    if (index !== -1) {
      this.orderOptions[index] = orderOption;
    }
  }

  deleteOrderOption(orderOption: OrderOption): void {
    const index = this.orderOptions.findIndex(option => option.Id === orderOption.Id);
    if (index !== -1) {
      this.orderOptions.splice(index, 1);
    }
  }

}


