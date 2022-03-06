import { IAddress } from "./address";
import { IEmployeeAddress } from "./employeeAddress";
import { IEmployeeHRSkill } from "./employeeHRSkill";
import { IEmployeeOtherSkill } from "./employeeOtherSkill";
import { IEmployeePhone } from "./employeePhone";
import { IEmployeeQualification } from "./employeeQualification";

export interface IEmployee {
     id: number;
     gender: string;
     firstName: string;
     secondName: string;
     familyName: string;
     knownAs: string;
     street: string;
     city: string;
     pin: string;
     state: string;
     displayName: string;
     userName: string;
     password: string;
     email: string;
     dateOfBirth: Date;
     dateOfJoining: Date;     

     placeOfBirth: string;
     department: string;
     aadharNo: string;
     nationality: string;
     userRole: string;     
     introduction: string;
     interests: string;
     position: string;
     loggedInAppUserId: number;
     appUserId: number;
     photoUrl: string;
     employeeAddresses: IEmployeeAddress[];
     employeeQualifications: IEmployeeQualification[];
     employeePhones: IEmployeePhone[];
     hrSkills: IEmployeeHRSkill[];
     otherSkills: IEmployeeOtherSkill[];
}    
