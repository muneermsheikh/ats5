export interface IMessage {
     id: number;
     senderId: number;
     senderUsername: string;
     senderPhotoUrl: string;
     recipientId: number;
     recipientUsername: string;
     recipientPhotoUrl: string;
     content: string;
     dateRead: Date;
     messageSent: Date;
     senderDeleted: boolean;
     recipientDeleted: boolean;
}

export class message {
     id: number;
     senderId: number;
     senderUsername: string;
     senderPhotoUrl: string;
     recipientId: number;
     recipientUsername: string;
     recipientPhotoUrl: string;
     content: string;
     dateRead: Date;
     messageSent: Date;
     senderDeleted: boolean;
     recipientDeleted: boolean;
}
