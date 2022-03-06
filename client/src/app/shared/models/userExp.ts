export interface IUserExp {
     id: number;
     candidateId: number;
     srNo: number;
     positionId: number;
     employer: string;
     position: string;
     salaryCurrency: string;
     monthlySalaryDrawn?: number;
     workedFrom: Date;
     workedUpto: Date;
}