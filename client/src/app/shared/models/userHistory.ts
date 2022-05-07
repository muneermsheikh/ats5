import { IUserHistoryItem } from "./userHistoryItem";

export interface IUserHistory 
{
     id: number;
     personType: string;
     personId: number;
     applicationNo: number;
     personName: string;
     phoneNo: string;
     emailId: string;
     createdOn: Date;
     userHistoryItems: IUserHistoryItem[];
}