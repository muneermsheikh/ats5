import { IMessage } from "../models/message";

export interface IMessageDto
{
     emailMessage: IMessage;
     errorMessage: string;
}