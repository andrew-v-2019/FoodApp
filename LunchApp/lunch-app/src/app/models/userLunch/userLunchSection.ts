import { UserLunchItem } from "app/models/userLunch/userLunchItem";

export class UserLunchSection {
    name: string;
    number:number;
    menuSectionId:number;
    menuId:number;
    items:UserLunchItem[];
    checked:boolean;
 }