import { IProspectiveCandidate } from "../models/prospectiveCandidate";

export interface IPaginationProspectiveCandidates {
     pageIndex: number;
     pageSize: number;
     count: number;
     data: IProspectiveCandidate[];
}

export class PaginationProspectiveCandidates implements IPaginationProspectiveCandidates {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IProspectiveCandidate[]=[];
}
