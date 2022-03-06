export interface IApplicationTaskInBrief {
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

     orderId: number;
     orderNo: number;
     orderItemId: number;
     applicationNo: number;
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

     orderId: number;
     orderNo: number;
     orderItemId: number;
     applicationNo: number;
}