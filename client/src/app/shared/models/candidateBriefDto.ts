import { IUserProfession } from "./userProfession";

export interface ICandidateBriefDto {
     checked: boolean;
     id: number;
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