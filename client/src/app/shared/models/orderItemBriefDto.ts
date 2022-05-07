import { IOrder } from "./order";

export interface IOrderItemBriefDto
{
     checked: boolean;
     orderNo: number;
     orderDate: Date;
     customerName: string;
     orderItemId: number;
     requireInternalReview: boolean;
     categoryRef: string;
     categoryName: string;
     categoryRefAndName: string;
     quantity: number;
     status: string;
     assessmentQDesigned: boolean;
}

export class OrderItemBriefDto implements IOrderItemBriefDto
{
     checked: boolean = false;
     orderNo: number;
     orderDate: Date;
     customerName: string;
     orderItemId: number;
     requireInternalReview: boolean;
     categoryId: number;
     categoryRef: string;
     categoryName: string;
     categoryRefAndName: string;
     quantity: number;
     status: string;
     assessmentQDesigned: boolean;
}