namespace core.Entities.Admin
{
    public class EmployeeQualification: BaseEntity
    {
        public int EmployeeId { get; set; }
        public int QualificationId { get; set; }
        public bool IsMain { get; set; }
        //public Employee Employee {get; set;}
    }
}