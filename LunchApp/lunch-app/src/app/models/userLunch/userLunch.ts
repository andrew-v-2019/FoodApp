import { User } from "app/models/user/user";
import { UserLunchSection } from "app/models/userLunch/userLunchSection";

export class UserLunch {
    user: User;
    menuId:number;
    userLunchId:number;
    sections:UserLunchSection[];
    lunchDate:string;
    price: number;
    editable: boolean;
    selectedForOrder: boolean;
 }
