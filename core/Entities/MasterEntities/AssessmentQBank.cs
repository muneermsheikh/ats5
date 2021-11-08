using System.ComponentModel.DataAnnotations;

namespace core.Entities.MasterEntities
{
    public class AssessmentQBank: BaseEntity
    {
        public int QNo { get; set; }
        public int CategoryId { get; set; }
        public bool IsStandardQ { get; set; }
        public string AssessmentParameter { get; set; }
        [MinLength(20), MaxLength(400), Required]
        public string Question { get; set; }
        [Range(0, 100)]
        public int MaxPoints { get; set; }
    }
}