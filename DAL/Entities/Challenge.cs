using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Challenge
    {
        [Key]
        public Guid ChallengeId { get; set; }
        public string Name { get; set; }

        [Required]
        public string Content { get; set; }
        public Guid PositionId { get; set; }
        public Position Position { get; set; }
    }
}
