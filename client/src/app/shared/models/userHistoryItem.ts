export interface IUserHistoryItem 
{
     id: number;
     userContactId: number;
     phoneNo: string;
     subject: string;
     categoryRef: string;
     dateOfContact: Date;
     loggedInUserId: number;
     loggedInUserName: string;
     contactResult: number;
     contactResultName: string;
     gistOfDiscussions: string;
}