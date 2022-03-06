import { IAssociateToFwdDto } from "./associateToFwdDto";
import { IOrderItemToFwdDto } from "./orderItemToFwdDto";

export interface IOrderItemsAndAgentsToFwdDto
{
     items: IOrderItemToFwdDto[];
     agents: IAssociateToFwdDto[];
     dateForwarded: Date;
}

export class OrderItemsAndAgentsToFwdDto implements IOrderItemsAndAgentsToFwdDto
{
     items: IOrderItemToFwdDto[];
     agents: IAssociateToFwdDto[];
     dateForwarded: Date;
}