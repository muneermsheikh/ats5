using System;

namespace core.ParamsAndDtos
{
    public class QualificationParams: ParamPages
    {
        public int QualificationId { get; set; }
        public string QualificationNameLike {get; set;}
        
    }
}