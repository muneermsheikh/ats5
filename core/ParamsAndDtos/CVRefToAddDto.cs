using System;

namespace core.ParamsAndDtos
{
    public class CVRefToAddDto
    {
        public int CandidateId { get; set; }
        public int OrderItemId { get; set; }
        public int Charges { get; set; }
        public DateTime ReferredOn {get; set;}
    }
}