using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core.ParamsAndDtos
{
    public class OrderItemBriefDto
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryRef {get; set;}
        public string CategoryRefAndName {get; set;}
        public int Quantity { get; set; }
        public int Status { get; set; }
    }
}