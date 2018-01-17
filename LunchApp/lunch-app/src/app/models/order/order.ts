import { UserLunch } from "app/models/userLunch/userLunch";

export class Order {
    orderId: number;
    orderName: string;
    submitted: boolean;
    menuId: number;
    lunches: UserLunch[];
    submitionDate: string;
}