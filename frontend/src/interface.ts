export interface Users{
  username: string
  password: string
}


export interface OrderOption {
  Id: number;
  OptionName: string;
  active: boolean;
  IsUpdated: boolean;
  isNew: boolean;
  IsDeleted: boolean;
}
