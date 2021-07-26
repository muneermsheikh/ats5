using System.ComponentModel.DataAnnotations;

namespace core.Entities.Orders
{
    public class JobDescription
    {
          public JobDescription()
          {
          }

          public JobDescription(string jobDescInBrief, string qualificationDesired, int expDesiredMin, int expDesiredMax, int minAge, int maxAge)
          {
               JobDescInBrief = jobDescInBrief;
               QualificationDesired = qualificationDesired;
               ExpDesiredMin = expDesiredMin;
               ExpDesiredMax = expDesiredMax;
               MinAge = minAge;
               MaxAge = maxAge;
          }

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