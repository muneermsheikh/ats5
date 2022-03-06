export class candidateHistoryParams {
     applicationNo?: number;
     aadharNo: string;
     phoneNo: string;
     id?: number;
     partyNameLike?: string;

     sort = "name";
     pageNumber = 1;
     pageSize = 10;
     search: string;
}