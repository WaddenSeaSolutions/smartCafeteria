export interface Users{
  username: string
  password: string
}


export interface OrderOption {
  Id: number;
  OptionName: string;
  Active: boolean;
}

export interface Order {
  Id: number;
  Timestamp: string;
  Payment: boolean;
  Done: boolean;
  UserId: number;
  OrderOptions: OrderOption[];
}
