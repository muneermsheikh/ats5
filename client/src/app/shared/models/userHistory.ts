import { IUserHistoryItem } from "./userHistoryItem";

export interface IUserHistory 
{
     id: number;
     customerOfficialId: number;
     candidateId: number;
     partyName: string;
     aadharNo: string;
     phoneNo: string;
     emailId: string;
     applicationNo: number;
     createdOn: Date;
     userHistoryItems: IUserHistoryItem[];
}