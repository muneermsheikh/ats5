export class contractReviewParams {
     orderId?: number;
     orderDate?: Date;
     customerId?: number;
     customerNameLike?: string;
     city? ='';
     categoryId? = 0;
     orderidInts: number[];
     orderids: string;
     sort = "name";
     pageNumber = 1;
     pageSize = 10;
     search: string;
}