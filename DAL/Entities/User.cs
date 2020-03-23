using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string ConfirmationCode { get; set; }

        public string Phone { get; set; }
        public string FullName { get; set; }
        public DateTime DateCreate { get; set; }

        public Guid PositionId { get; set; }
        public Position Position { get; set; }

        public Cv Cv { get; set; }
    }
}
