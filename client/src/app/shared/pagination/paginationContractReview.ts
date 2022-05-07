import { IContractReview } from "../models/contractReview";


export interface IPaginationContractReview {
     pageIndex: number;
     pageSize: number;
     includeOfficials: boolean;
     count: number;
     data: IContractReview[];
}

 
 export class PaginationContractReview implements IPaginationContractReview {
     pageIndex: number;
     pageSize: number;
     includeOfficials: boolean;
     count: number;
     data: IContractReview[]=[];
 }