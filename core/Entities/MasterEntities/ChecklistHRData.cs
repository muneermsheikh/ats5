using System.ComponentModel.DataAnnotations;

namespace core.Entities.MasterEntities
{
    public class ChecklistHRData: BaseEntity
    {
        [Range(1,1000)]
        public int SrNo {get; set;}
        public string Parameter { get; set; }
    }
}