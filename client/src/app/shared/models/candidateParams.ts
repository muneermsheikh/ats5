export class candidateParams {
     candidateId?: number;
     applicationNoFrom?: number;
     applicationNoUpto?: number;
     registeredFrom?: Date;
     registeredUpto?: number;
     professionId? = 0;
     agentId?: number;
     industryId?: number;
     associateId?: number;
     appUserId?: number;
     city ='';
     disrict: string;
     state: string;
     email: '';
     mobile: '';

     passportNo?: string;

     sort = "name";
     pageNumber = 1;
     pageSize = 10;
     search: string;
}

