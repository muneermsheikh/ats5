import { IChecklistHRItem } from "./checklistHRItem";

export interface IChecklistHR{

     id: number;
     candidateId: number;
     orderItemId: number;
     userId: number;
     checkedOn: Date;
     hrExecRemarks: string;
     checklistHRItems: IChecklistHRItem[];
}