using System.Collections.Generic;

namespace BLL.Models
{
    public class AdminPageModel
    {
        public const string PAGING = "For paging  ->  /GET api/admin/getuserlistorderbydate/(items per page)/(destination page)/(sorting criteria)";
        public const string CRITERIAS = "Available sorting criterias : (default)'Date', 'Email', 'FullName', 'Position'\nPlease input these indexes correctly\n";

        public const string POS_SORT = "For position filtering  ->  /GET base/(items per page)/(destination page)/(sorting criteria)";
        public const string CVURL = "To get Candidate's Cv  ->  /GET base/(user Id)/(user Email)";
        public List<string> Instructions { get; set; }
        public List<PageModel> UserList { get; set; }

        public AdminPageModel()
        {
            Instructions = new List<string>
            { 
                PAGING,
                CRITERIAS,
                POS_SORT,
                CVURL
            };
        }
    }
}
