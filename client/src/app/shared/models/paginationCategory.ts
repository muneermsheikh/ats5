import { IProfession } from "./profession";

export interface IPaginationCategory {
     pageIndex: number;
     pageSize: number;
     count: number;
     data: IProfession[];
}

 
 export class PaginationCategory implements IPaginationCategory {
     pageIndex: number;
     pageSize: number;
     count: number;
     data: IProfession[]=[];
 }