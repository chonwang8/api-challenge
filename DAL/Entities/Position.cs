using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Position
    {
        [Key]
        public Guid PositionId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}