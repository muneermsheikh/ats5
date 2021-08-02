using System.Runtime.Serialization;

namespace core.Entities.Process
{
    public enum EnumDeployStatus
    {
        [EnumMember(Value="Selected")]
        Selected = 1000,
        [EnumMember(Value="Offer Letter Accepted")]
        OfferLetterAccepted = 1050,
        [EnumMember(Value="Referred for Medical Test")]
        ReferredForMedical=1100,
        [EnumMember(Value="Medically Declared Fit")]
        MedicallyFit=1200,
        [EnumMember(Value="Medically Unfit")]
        MedicallyUnfit=1300,
        [EnumMember(Value="Visa Documents submitted")]
        VisaDocsSubmitted=1500,
        [EnumMember(Value="Visa Query Raised")]
        VisaQueryRaised=1600,
        [EnumMember(Value="Visa Denied")]
        VisaDenied=1700,
        [EnumMember(Value="Visa Received")]
        VisaReceived=1800,
        [EnumMember(Value="Emigration Docs Lodged Online")]
        EmigDocsLodgedOnLine=1900,
        [EnumMember(Value="Emigration - Query Received")]
        EmigDocsQueryRecd=2000,
        [EnumMember(Value="PP Submitted to Emigration office")]
        EmigDocsPPSubmitted=2100,
        [EnumMember(Value="Emigration Denied")]
        EmigrationDenied=2200,
        [EnumMember(Value ="Emigration Query raised")]
        EmigrationQueryRaised, 
        [EnumMember(Value="Emigration Granted")]
        EmigrationGranted=2300,
        [EnumMember(Value="Travel Ticket booked")]
        TravelTicketBooked=2400,
        [EnumMember(Value = "Travel Canceled")]
        TravelCanceled=2500,
        [EnumMember(Value="Traveled")]
        Traveled=2600,
        [EnumMember(Value="Arrival acknowledged by client")]
        ArrivalAcknowledgedByClient=2700,
        [EnumMember(Value="Concluded")]
        Concluded=10000
    }
}