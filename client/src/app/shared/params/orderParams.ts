export class orderParams {
     orderNo?: number;
     orderDate?: Date;
     customerId?: number;
     customerNameLike?: string;
     city? ='';
     categoryId? = 0;
     
     sort = "name";
     pageNumber = 1;
     pageSize = 10;
     search: string;
}