using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Entities.HR;

namespace core.ParamsAndDtos
{
    public class ChecklistDto
    {
        public int Id { get; set; }
        public int CandidateId { get; set; }
        public int ApplicationNo { get; set; }
        public string CandidateName { get; set; }
        public string CategoryRef { get; set; }
        public string OrderRef {get; set;}
        public int OrderItemId { get; set; }
        public string UserLoggedIn { get; set; }
        public DateTime CheckedOn {get; set;}
        public string HrExecComments { get; set; }
        public ICollection<ChecklistHRItem> ChecklistHRItems {get; set;}
    }
}