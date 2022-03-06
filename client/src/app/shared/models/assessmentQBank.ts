export interface IAssessmentQBank
{
     id: number;
     categoryId: number;
     categoryName: string;
     assessmentQBankItems: IAssessmentQBankItem[];
}

export interface IAssessmentQBankItem
{
     id: number;
     assessmentQBankId: number;
     assessmentParameter: string;
     qNo: number;
     isStandardQ: boolean;
     question: string;
     maxPoints: number;
}