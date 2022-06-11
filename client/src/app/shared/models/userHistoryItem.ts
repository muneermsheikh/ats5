export interface IUserHistoryItem 
{
     id: number;
     personId: number;
     personType: string;
     phoneNo: string;
     subject: string;
     categoryRef: string;
     dateOfContact: Date;
     loggedInUserId: number;
     loggedInUserName: string;
     contactResultId: number;
     contactResultName: string;
     gistOfDiscussions: string;
     composeEmailMessage: boolean;
}