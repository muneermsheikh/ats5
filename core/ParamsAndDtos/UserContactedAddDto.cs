using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace core.ParamsAndDtos
{
    public class UserContactedAddDto
    {
        public int CandidateId { get; set; }
        [Required]
        public int ContactedById { get; set; }    
        public ICollection<UserContactedItemAddDto> UserContactedItems {get; set;}
    }
}