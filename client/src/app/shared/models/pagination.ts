import { ICustomer } from "./customer";

export interface IPagination {
     pageIndex: number,
     pageSize: number,
     includeOfficials: boolean,
     includeIndustries: boolean,
     count: number,
     data: ICustomer[]
}