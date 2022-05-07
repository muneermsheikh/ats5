import { ICVRefAndDeployDto } from "../dtos/cvRefAndDeployDto";


export interface IPaginationDeploy {
     pageIndex: number;
     pageSize: number;
     count: number;
     data: ICVRefAndDeployDto[];
}

 
 export class PaginationDeploy implements IPaginationDeploy {
     pageIndex: number;
     pageSize: number;
     count: number;
     data: ICVRefAndDeployDto[]=[];
 }