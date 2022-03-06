import { IOrderItem } from "./orderItem";

export interface IOrder 
{
     id: number; 
     orderNo: number; 
     orderDate: Date; 
     customerId: number; 
     customerName: string;
     buyerEmail: string; 
     orderRef: string; 
     orderRefDate: Date;
     salesmanId: number; 
     salesmanName: string; 
     projectManagerId: number;
     medicalProcessInchargeEmpId: number; 
     visaProcessInchargeEmpId: number; 
     emigProcessInchargeId: number;
     travelProcessInchargeId: number;
     completeBy: Date; 
     country: string; 
     cityOfWorking: string; 
     contractReviewStatusId: number;
     estimatedRevenue: number;
     status: number; 
     forwardedToHRDeptOn?: Date; 
     orderItems: IOrderItem[];
     
}