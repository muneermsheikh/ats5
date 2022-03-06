import { IOrderBrief } from "./orderBrief";

export interface IPaginationOrder {
     pageIndex: number;
     pageSize: number;
     includeOfficials: boolean;
     count: number;
     data: IOrderBrief[];
}

 
 export class PaginationOrder implements IPaginationOrder {
     pageIndex: number;
     pageSize: number;
     includeOfficials: boolean;
     count: number;
     data: IOrderBrief[]=[];
 }