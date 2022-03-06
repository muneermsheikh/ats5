import { IOrder } from "./order";

export interface IOrderItemBriefDto
{
     checked: boolean;
     orderDate: Date;
     customerName: string;
     orderItemId: number;
     categoryRef: string;
     categoryName: string;
     categoryRefAndName: string;
     quantity: number;
     status: string;
}

export class OrderItemBriefDto implements IOrderItemBriefDto
{
     checked: boolean = false;
     orderDate: Date;
     customerName: string;
     orderItemId: number;
     categoryId: number;
     categoryRef: string;
     categoryName: string;
     categoryRefAndName: string;
     quantity: number;
     status: string;
}