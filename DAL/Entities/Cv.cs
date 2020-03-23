using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Cv
    {
        [Key]
        public Guid CvId { get; set; }
        public DateTime UploadDate { get; set; }
        public string FileName { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
