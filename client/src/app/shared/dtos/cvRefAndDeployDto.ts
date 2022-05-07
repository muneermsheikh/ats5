
export interface ICVRefAndDeployDto {
     id: number;
     checked: boolean;
     cvRefId: number;
     customerName: string;
     orderId: number;
     orderNo: number;
     orderDate: Date;
     orderItemId: number;
     categoryName: string;
     categoryRef: string;
     customerId: number;
     candidateId: number;

     applicationNo: number;
     candidateName: string;
     referredOn: Date;
     selectedOn: Date;
     refStatus: number;
     deployStageName: string;
     deployStageDate: Date;
     nextStageId: number;
     nextStageDate: Date;
}