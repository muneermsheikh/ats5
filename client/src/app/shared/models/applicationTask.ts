import { ITaskItem } from "./taskItem";

export interface IApplicationTask {
     id: number;
     taskTypeId: number;
     taskDate: Date;
     taskOwnerId: number;
     assignedToId: number;
     taskDescription: string;
     completeBy: Date;
     taskStatus: string;
     completedOn: Date;
     postTaskAction: number;
     historyItemId: number;

     orderId: number;
     orderNo: number;
     orderItemId: number;
     applicationNo: number;
     candidateId: number;
     taskItems: ITaskItem[];
}
export class ApplicationTask {
     id: number;
     taskTypeId: number;
     taskDate: Date;
     taskOwnerId: number;
     assignedToId: number;
     taskDescription: string;
     completeBy: Date;
     taskStatus: string;
     completedOn: Date;
     postTaskAction: number;
     historyItemId: number;

     orderId: number;
     orderNo: number;
     orderItemId: number;
     applicationNo: number;
     candidateId: number;
     taskItems: ITaskItem[]=[];
}
