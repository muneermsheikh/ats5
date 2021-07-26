namespace core.Entities.MasterEntities
{
    public class AssessmentQBank: BaseEntity
    {
        public int QNo { get; set; }
        public int CategoryId { get; set; }
        public bool IsStandardQ { get; set; }
        public string AssessmentParameter { get; set; }
        public string Question { get; set; }
        public int MaxPoints { get; set; }
    }
}