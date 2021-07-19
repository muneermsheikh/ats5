namespace core.Specifications
{
    public class CandidateSpecParams: CommonSpecParams
    {

        public int? ProfessionId { get; set; }
        public int? IndustryId { get; set; }
        public int? CompanyId {get; set;}   //assocaite id
        public string City {get; set;}
        public string District {get; set;}
        public string State {get; set;}
        public string Email {get; set;}
        public string Mobile {get; set;}

    }
}