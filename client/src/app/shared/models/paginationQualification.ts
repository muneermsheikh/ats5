import { IProfession } from "./profession";
import { IQualification } from "./qualification";

export interface IPaginationQualification {
     pageIndex: number;
     pageSize: number;
     count: number;
     data: IQualification[];
}

 
 export class PaginationQualification implements IPaginationQualification {
     pageIndex: number;
     pageSize: number;
     count: number;
     data: IQualification[]=[];
 }