namespace core.Entities.HR
{
    public class ChecklistItemHR: BaseEntity
    {
        public ChecklistItemHR()
          {
          }

        public ChecklistItemHR(int srNo, string parameter)
          {
               SrNo = srNo;
               Parameter = parameter;
          }
          
          public ChecklistItemHR(int srNo, string parameter, string response, string exceptions)
          {
               SrNo = srNo;
               Parameter = parameter;
               Response = response;
               Exceptions = exceptions;
          }

        public int ChecklistHRId { get; set; }
        public int SrNo {get; set;}
        public string Parameter {get; set;}
        public bool Accepts {get; set;}=false;
        public string Response {get; set;}
        public string Exceptions {get; set;}
        public bool MandatoryTrue {get; set;}
        public ChecklistHR ChecklistHR {get; set;}
    }
}