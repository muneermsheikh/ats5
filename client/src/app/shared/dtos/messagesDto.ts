import { IMessage } from "../models/message";

export interface IMessagesDto
{
     messages: IMessage[];
     errorString: string;
}