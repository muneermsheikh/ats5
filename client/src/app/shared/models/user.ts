export interface IUser {
     /*
     userType: string;
     username: string;
     knownAs: string;
     token: string;
     photoUrl: string;
     gender: string;
     */
     loggedinEmployeeId: number;
     username: string;
     email: string;
     displayName: string;
     token: string;
     roles: string[];
}