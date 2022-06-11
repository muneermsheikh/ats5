export interface IMessage {
     id: number;
     messageGroup: string;
     messageTypeId: number;
     senderId: number;
     senderEmailAddress: string;
     senderUserName: string;
     recipientUserName: string;
     recipientEmailAddress: string;
     ccEmailAddress: string;
     bccEmailAddress: string;
     subject: string;
     content: string;
     dateReadOn?: Date;
     messageSentOn?: Date;
     senderDeleted: boolean;
     recipientDeleted: boolean;
     recipientId: number;

}

export class message {
     id: number;
     messageGroup: string;
     messageTypeId: number;
     senderId: number;
     senderEmailAddress: string;
     senderUserName: string;
     recipientUserName: string;
     recipientEmailAddress: string;
     ccEmailAddress: string;
     bccEmailAddress: string;
     subject: string;
     content: string;
     dateReadOn?: Date;
     messageSentOn?: Date;
     senderDeleted: boolean;
     recipientDeleted: boolean;
     recipientId: number;
}
