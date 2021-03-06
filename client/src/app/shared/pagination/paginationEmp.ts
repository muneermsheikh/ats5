import { IEmployeeBrief } from "../models/employeeBrief";


export interface IPaginationEmployee {
     pageIndex: number;
     pageSize: number;
     count: number;
     data: IEmployeeBrief[];
}

export class PaginationEmployee implements IPaginationEmployee {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IEmployeeBrief[]=[];
}
