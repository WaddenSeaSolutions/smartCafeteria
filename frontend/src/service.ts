import {Injectable} from "@angular/core";
import {Users} from "./interface";

@Injectable({
  providedIn: 'root'
})
export class Service{
  users: Users | undefined;
}
