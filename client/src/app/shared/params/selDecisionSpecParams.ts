export class SelDecisionSpecParams
{
     orderItemId: number=0;
     categoryId: number=0;
     categoryName: string;
     orderId?: number=0;
     orderNo?: number=0;
     candidateId?: number=0;
     applicationNo?: number=0;
     id?: number=0;
     cVRefId?: number=0;
     cVRefIds: number[];
     includeEmploymentData: boolean=false;
     includeDeploymentData: boolean=false;
     
     pageIndex = 1;
     pageSize=5;
     sort = "";
     search: string;
}