export interface IOrderItemToFwdDto
{
     orderItemId: number;
     orderId: number;
     orderDate: Date;
     customerName: string;
     customerCity: string;
     projectManagerId: number;
     categoryRef: string;
     categoryId: number;
     categoryName: string;
     quantity: number;
     minCVs: number;
     ecnr: boolean;
     charges: number;
     salaryCurrency: string;
     basicSalary: number;
     remunerationURL: string;
     jobDescriptionURL: string;
}

export class OrderItemToFwdDto implements IOrderItemToFwdDto
{
     orderItemId: number;
     orderId: number;
     orderDate: Date;
     customerName: string;
     customerCity: string;
     projectManagerId: number;
     categoryId: number;
     categoryRef: string;
     categoryName: string;
     quantity: number;
     minCVs: number;
     ecnr: boolean;
     charges: number;
     salaryCurrency: string;
     basicSalary: number;
     remunerationURL: string;
     jobDescriptionURL: string;
}