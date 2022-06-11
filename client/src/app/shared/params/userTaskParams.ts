export class userTaskParams {
     taskId?: number;
     taskDate?: Date;
     candidateId: number;
     personType: string;
     assignedToId?: number;
     assignedToNameLike?: string;
     taskStatus? ='';
     orderId? = 0;
     
     sort = "taskDate";
     pageNumber = 1;
     pageSize = 10;
     search: string;
}