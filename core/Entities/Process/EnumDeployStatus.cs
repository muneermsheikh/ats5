using System.Runtime.Serialization;

namespace core.Entities.HR
{
    public enum EnumDeployStatus
    {
        [EnumMember(Value="Selected")]
        Selected,
        [EnumMember(Value="Offer Letter Accepted")]
        OfferLetterAccepted,
        [EnumMember(Value="Referred for Medical Test")]
        ReferredForMedical,
        [EnumMember(Value="Medically Declared Fit")]
        MedicallyFit,
        [EnumMember(Value="Medically Unfit")]
        MedicallyUnfit,
        [EnumMember(Value="Visa Documents submitted")]
        VisaDocsSubmitted,
        [EnumMember(Value="Visa Query Raised")]
        VisaQueryRaised,
        [EnumMember(Value="Visa Denied")]
        VisaDenied,
        [EnumMember(Value="Visa Received")]
        VisaReceived,
        [EnumMember(Value="Emigration Docs Lodged Online")]
        EmigDocsLodgedOnLine,
        [EnumMember(Value="PP Submitted to Emigration office")]
        EmigDocsPPSubmitted,
        [EnumMember(Value="Emigration Granted")]
        EmigrationGranted,
        [EnumMember(Value="Emigration Denied")]
        EmigrationDenied,
        [EnumMember(Value ="Emigration Query raised")]
        EmigrationQueryRaised, 
        [EnumMember(Value="Travel Ticket booked")]
        TravelTicketBooked,
        [EnumMember(Value="Traveled")]
        Traveled,
        [EnumMember(Value="Arrival acknowledged by client")]
        ArrivalAcknowledgedByClient,
        [EnumMember(Value="Concluded")]
        Concluded
    }
}