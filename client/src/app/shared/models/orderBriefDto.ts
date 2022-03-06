import { IOrder } from "./order";

export interface IOrderBriefDto
{
     orderNo: number;
     orderDate: Date;
     customerName: string;
}

export class OrderBriefDto implements IOrderBriefDto
{
     orderNo: number;
     orderDate: Date;
     customerName: string;
}