import { IMessage } from "../models/message";
import { IEmploymentDto } from "./employmentDto";

export interface ISelectionMsgsAndEmploymentsDto
{
     emailMessages: IMessage[];
     employmentDtos: IEmploymentDto[];

}