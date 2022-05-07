import { ISelectionDecision } from "../models/selectionDecision";

export interface IPaginationSelDecision {
     pageIndex: number;
     pageSize: number;
     count: number;
     data: ISelectionDecision[];
}

 
 export class paginationSelDecision implements IPaginationSelDecision {
     pageIndex: number;
     pageSize: number;
     count: number;
     data: ISelectionDecision[]=[];
 }