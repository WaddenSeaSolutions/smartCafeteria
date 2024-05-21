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
  id: number;
  timestamp: string;
  payment: boolean;
  done: boolean;
  userId: number;
  orderOptions: OrderOption[];
}
