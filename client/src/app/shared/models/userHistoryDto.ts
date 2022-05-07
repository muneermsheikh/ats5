export interface IUserHistoryDto
{
     id: number;
     personType: string;
     personName: string;
     personId: number;
     applicationNo: number;
     emailId: string;
     mobileNo: string;
     createNewIfNull: boolean;
}