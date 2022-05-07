import { IUserProfession } from "./userProfession";

export interface ICandidateBriefDto {
     checked: boolean;
     id: number;
     historyId: number;
     mobileNo: string;
     gender: string;
     applicationNo: number;
     aadharNo: string;
     fullName: string;
     city: string;
     referredById: number;
     referredByName: string;
     candidateStatusName: string;
     userProfessions: IUserProfession[];
}

export class CandidateBriefDto implements ICandidateBriefDto {
     checked: boolean;
     id: number;
     historyId: number;
     mobileNo: string;
     gender: string;
     applicationNo: number;
     aadharNo: string;
     fullName: string;
     city: string;
     referredById: number;
     referredByName: string;
     candidateStatusName: string;
     userProfessions: IUserProfession[];
}