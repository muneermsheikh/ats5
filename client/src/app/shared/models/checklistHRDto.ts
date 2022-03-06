import { IChecklistHRItem } from "./checklistHRItem";

export interface IChecklistHRDto{

     id: number;
     candidateId: number;
     applicationNo: number;
     candidateName: string;
     categoryRef: string;
     orderRef: string;
     orderItemId: number;
     userLoggedIn: string;
     checkedOn: Date;
     hrExecComments: string;
     checklistHRItems: IChecklistHRItem[];
}

export class ChecklistHRDto implements IChecklistHRDto{

     id: number;
     candidateId: number;
     applicationNo: number;
     orderItemId: number;
     categoryRef: string;
     candidateName: string;
     orderRef: string;
     userLoggedIn: string;
     checkedOn: Date;
     hrExecComments: string;
     checklistHRItems: IChecklistHRItem[];
}