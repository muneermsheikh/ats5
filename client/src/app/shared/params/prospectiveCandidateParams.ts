export class prospectiveCandidateParams {
     id?=0;
     categoryRef?='';
     dateAdded?: Date;
     status= '';
     candidateNameLike?='';
     
     sort = "name";
     pageNumber = 1;
     pageSize = 10;
     search: string;
}