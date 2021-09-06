namespace core.Entities.MasterEntities
{
    public class ReviewItemData: BaseEntity
    {
        public ReviewItemData()
        {
        }

        public ReviewItemData(int srNo, string reviewParameter)
        {
            SrNo = srNo;
            ReviewParameter = reviewParameter;
        }

        public int SrNo { get; set; }
        public string ReviewParameter { get; set; }
        public bool Response {get; set;}
        public bool IsMandatoryTrue {get; set;}
    }
}