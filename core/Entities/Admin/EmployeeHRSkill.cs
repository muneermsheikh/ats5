namespace core.Entities.Admin
{
    public class EmployeeHRSkill: BaseEntity
    {
        public int EmployeeId { get; set; }
        public int CategoryId { get; set; }        
        public int IndustryId {get; set;}
        public int SkillLevel {get; set;}
        //public Employee Employee {get; set;}
    }
}