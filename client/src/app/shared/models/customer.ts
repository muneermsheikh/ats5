import { IAgencySpecialty } from "./agencySpecialty";
import { ICustomerIndustry } from "./customerIndustry";
import { ICustomerOfficial } from "./customerOfficial";

export interface ICustomer {
     id: number;
     customerType: string;
     customerName: string;
     knownAs: string;
     add: string;
     add2: string;
     city: string;
     pin: string;
     district: string;
     state: string;
     country: string;
     email: string;
     website: string;
     phone: string;
     phone2: string;
     logoUrl?: string;
     customerStatus: number;
     createdOn: Date;
     introduction: string;
     customerIndustries: ICustomerIndustry[];
     customerOfficials: ICustomerOfficial[];
     agencySpecialties: IAgencySpecialty[];
 }
