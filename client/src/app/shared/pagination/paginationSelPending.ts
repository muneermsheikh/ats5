import { ISelPendingDto } from "../dtos/selPendingDto";

export interface IPaginationSelPending {
     pageIndex: number;
     pageSize: number;
     count: number;
     data: ISelPendingDto[];
}

 
 export class paginationSelPending implements IPaginationSelPending {
     pageIndex: number;
     pageSize: number;
     count: number;
     data: ISelPendingDto[]=[];
 }