using System;

namespace BLL.Models
{
    public class CvModel
    {
        public string Email { get; set; }
        public DateTime UploadDate { get; set; }
        public string FileName { get; set; }
        public Guid KeyName { get; set; }
    }
}
