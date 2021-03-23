using System.ComponentModel.DataAnnotations;

namespace App.Core.Models
{
    public class BaseEntity : IBaseEntity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool ActiveFlag { get; set; }
    }
}
