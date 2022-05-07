export interface ISelectionDecision
{
     checked: boolean;
     id: number;
     cVRefId: number;
     orderItemId: number;
     categoryId: number;
     categoryRef: string;
     orderNo: number;
     customerName: string;
     applicationNo: number;
     candidateId: number;
     candidateName: string;
     decisionDate: Date;
     selectionStatusId: number;
     selectedOn: number;
     employment: IEmployment;
     remarks: string;
}

export interface IEmployment
{
     cVRefId: number;
     selectionDecisionId: number;
     selectedOn: Date;
     charges: number;
     salaryCurrency: string;
     salary: number;
     contractPeriodInMonths: number;
     housingProvidedFree: boolean;
     housingAllowance: number;
     foodProvidedFree: boolean;
     foodAlowance: number;
     transportProvidedFree: boolean;
     transportAllowance: number;
     otherAllowance: number;
     leavePerYearInDays: number;
     leaveAirfareEntitlementAfterMonths: number;
     offerAcceptedOn: Date;
     remarks: string;
}
