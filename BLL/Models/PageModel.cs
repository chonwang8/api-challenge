using System;

namespace BLL.Models
{
    public class PageModel
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string FullName { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
