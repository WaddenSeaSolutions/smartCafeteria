export interface Users{
  username: string
  password: string
}


export interface OrderOption {
  id: number;
  optionName: string;
  active: boolean;
}

export interface Order {
  Id: number;
  Timestamp: string;
  Payment: boolean;
  Done: boolean;
  UserId: number;
  OrderOptions: OrderOption[];
}
