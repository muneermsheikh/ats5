export interface IUserAttachment {
     id: number;
     candidateId: number;
     appUserId: number;
     attachmentType: string;
     attachmentSizeInBytes: number;
     url: string;
     dateUploaded: Date;
     uploadedByEmployeeId: number;
}