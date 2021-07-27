using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace core.Entities.Process
{
    public class DeployStatus
    {
        public int Id { get; set; }
        [Required]
        public string StatusName { get; set; }
    }
}