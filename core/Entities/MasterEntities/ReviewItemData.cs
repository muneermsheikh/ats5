namespace core.Entities.MasterEntities
{
    public class ReviewItemData: BaseEntity
    {
          public ReviewItemData()
          {
          }

          public ReviewItemData(int srNo, string reviewDescription)
          {
               SrNo = srNo;
               ReviewDescription = reviewDescription;
          }

          public int SrNo { get; set; }
        public string ReviewDescription { get; set; }
    }
}