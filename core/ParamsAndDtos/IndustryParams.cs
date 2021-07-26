using System;

namespace core.ParamsAndDtos
{
    public class IndustryParams: ParamPages
    {
        public int IndustryId { get; set; }
        public string IndustryNameLike {get; set;}
        public int ProfessionId {get; set;}

    }
}