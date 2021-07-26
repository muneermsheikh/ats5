using System;

namespace core.ParamsAndDtos
{
    public class CategoryParams: ParamPages
    {
        public int CategoryId { get; set; }
        public string CategoryNameLike {get; set;}
        //public int? IndustryId {get; set;}
        
    }
}