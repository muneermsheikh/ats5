using System.ComponentModel.DataAnnotations;

namespace core.Entities.Orders
{
    public class JobDescription
    {
        [Required, MaxLength(250)]
        public string JobDescInBrief { get; set; }
        public string QualificationDesired { get; set; }
        [Range(0,40)]
        public int ExpDesiredMin { get; set; }
        [Range(0,40)]
        public int ExpDesiredMax { get; set; }
        [Range(18,80)]
        public int MinAge { get; set; }
        [Range(18,80)]
        public int MaxAge { get; set; }
        
    }
}