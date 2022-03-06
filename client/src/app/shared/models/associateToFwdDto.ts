export interface IAssociateToFwdDto
{
     customerOfficialId: number;
     customerId: number;
     customerName: string;
     customerCity: string;
     title: string;
     customerOfficialName: string;
     officialEmailId: string;
     phoneNo: string;
     mobile: string;
     officialDesignation: string;
     checked: boolean;
     checkedPhone: boolean;
     checkedMobile: boolean;
}

export class AssociateToFwdDto implements IAssociateToFwdDto
{
     customerOfficialId: number;
     customerId: number;
     customerName: string;
     customerCity: string;
     title: string;
     customerOfficialName: string;
     officialEmailId: string;
     phoneNo: string;
     mobile: string;
     officialDesignation: string;
     checked: boolean;
     checkedPhone: boolean;
     checkedMobile: boolean;
}