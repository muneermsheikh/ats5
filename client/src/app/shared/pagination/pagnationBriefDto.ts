import { IOrderBriefDto } from "../models/orderBriefDto";

export interface IPaginationOrderBrief {
     city: string;
     orderNo: number;
     //search: string;
     //sort: string;

     pageIndex: number;
     pageSize: number;
     includeOfficials: boolean;
     count: number;
     data: IOrderBriefDto[];
}

 
 export class PaginationOrderBrief implements IPaginationOrderBrief {
     city: string;
     orderNo: number;
     //search: string;
     //sort: string;
     
     pageIndex: number;
     pageSize: number;
     includeOfficials: boolean;
     count: number;
     data: IOrderBriefDto[]=[];
 }