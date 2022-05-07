import { ICustomerOfficialDto } from "./customerOfficialDto";
import { IOrder } from "./order";

export interface IOrderBriefDto
{
     id: number;
     orderNo: number;
     orderDate: Date;
     customerName: string;
     items: IOrderItemDto[];
}

export interface IOrderItemDto{
     checked: boolean;
     orderId: number;
     orderItemId: number;
     categoryRef: string;
     categoryName: string;
     quantity: number;
     status: string;
}

export class OrderBriefDto implements IOrderBriefDto
{
     id: number;
     orderNo: number;
     orderDate: Date;
     customerName: string;
     items: IOrderItemDto[];

}