export class interviewParams {
     id?: number;
     orderNo?: number;
     orderId?: number;
     customerId?: number;
     customerNameLike?: string;
     interviewVenue?: string;
     categoryId? = 0;
     
     sort = "name";
     pageNumber = 1;
     pageSize = 10;
     search: string;
}