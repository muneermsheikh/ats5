import { IChecklistHRItem } from "./checklistHRItem";

export interface IChecklistHRDto{

     id: number;
     candidateId: number;
     applicationNo: number;
     candidateName: string;
     categoryRef: string;
     orderRef: string;
     orderItemId: number;
     userLoggedId: number;
     userLoggedName: string;
     checkedOn: Date;
     hrExecComments: string;
     charges: number;
     chargesAgreed: number;
     exceptionApproved: boolean;
     exceptionApprovedBy: string;
     exceptionApprovedOn: Date;
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
     userLoggedId: number;
     userLoggedName: string;
     checkedOn: Date;
     hrExecComments: string;
     charges: number;
     chargesAgreed: number;
     exceptionApproved: boolean;
     exceptionApprovedBy: string;
     exceptionApprovedOn: Date;
     checklistHRItems: IChecklistHRItem[];
}