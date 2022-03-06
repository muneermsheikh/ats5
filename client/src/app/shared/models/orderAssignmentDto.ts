export interface IOrderAssignmentDto
{
     orderId: number;
     orderNo: number;
     orderDate: Date;
     cityOfWorking: string;
     projectManagerId: number;
     projectManagerPosition: string;
     orderItemId: number;
     hrExecId: number;
     categoryRef: string;
     categoryName: string;
     categoryId: number;
     customerId: number;
     customerName: string;
     quantity: number;
     completeBy: Date;
     postTaskAction: number;
}

export class orderAssignmentDto implements IOrderAssignmentDto
{
     orderId: number;
     orderNo: number;
     orderDate: Date;
     cityOfWorking: string;
     projectManagerId: number;
     projectManagerPosition: string;
     orderItemId: number;
     hrExecId: number;
     categoryRef: string;
     categoryName: string;
     categoryId: number;
     customerId: number;
     customerName: string;
     quantity: number;
     completeBy: Date;
     postTaskAction: number;
}