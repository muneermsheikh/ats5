
import { ICustomerOfficialDto } from "./customerOfficialDto";
import { IOrderItemToFwdDto } from "./orderItemToFwdDto";

export interface IOrderItemsAndAgentsToFwdDto
{
     items: IOrderItemToFwdDto[];
     agents: ICustomerOfficialDto[];
     dateForwarded: Date;
}

export class OrderItemsAndAgentsToFwdDto implements IOrderItemsAndAgentsToFwdDto
{
     items: IOrderItemToFwdDto[];
     agents: ICustomerOfficialDto[];
     dateForwarded: Date;
}