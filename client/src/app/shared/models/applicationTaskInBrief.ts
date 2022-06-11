export interface IApplicationTaskInBrief {
     id: number;
     taskTypeId: number;

     taskDate: Date;
     taskOwnerId: number;
     assignedToId: number;
     
     orderId: number;
     orderNo: number;
     orderItemId: number;
     applicationNo: number;
     candidateId: number;
     taskDescription: string;
     completeBy: Date;
     taskStatus: string;
     completedOn: Date;
     postTaskActionName: string;
     historyItemId: number;

     //additional fields
     taskTypeName: string;
     taskOwnerName: string;
     assignedToName: string;
     

     
}

export class ApplicationTaskInBrief implements IApplicationTaskInBrief {
     id: number;
     candidateId: number;
     taskTypeId: number;
     taskTypeName: string;
     taskDate: Date;
     taskOwnerId: number;
     taskOwnerName: string;
     assignedToId: number;
     assignedToName: string;
     taskDescription: string;
     completeBy: Date;
     taskStatus: string;
     completedOn: Date;
     postTaskActionName: string;
     historyItemId: number;

     orderId: number;
     orderNo: number;
     orderItemId: number;
     applicationNo: number;
}