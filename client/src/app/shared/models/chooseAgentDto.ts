export interface IChooseAgentDto
{
     customerId: number;
     customerName: string;
     city: string;
     officialId: number;
     officialName: string;
     designation: string;
     title: string;
     email: string;
     phoneNo: string;
     mobile: string;
     value: number;
     checked: boolean;
     checkedPhone: boolean;
     checkedMobile: boolean; 
     isBlocked: boolean;
}

export class chooseAgentDto implements IChooseAgentDto
{
     customerId: number;
     customerName: string;
     city: string;
     officialId: number;
     officialName: string;
     designation: string;
     title: string;
     email: string;
     phoneNo: string;
     mobile: string;
     value: number;
     checked: boolean;
     checkedPhone: boolean;
     checkedMobile: boolean; 
     isBlocked: boolean;
}