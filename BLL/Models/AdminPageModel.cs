using System.Collections.Generic;

namespace BLL.Models
{
    public class AdminPageModel
    {
        public const string PAGING = "For paging  ->  /GET base/(items per page)/(destination page)/(sorting criteria)";
        public const string CRITERIAS = "Available sorting criterias : (default)'email', 'date', 'fullname', 'position'\n\n";

        public const string POS_SORT = "For position filtering  ->  /GET base/(items per page)/(destination page)/(sorting criteria)";
        public const string CVURL = "To get Candidate's Cv  ->  /GET base/(user Id)/(user Email)";
        public List<string> Instruction { get; set; }
        public List<PageModel> UserList { get; set; }

        public AdminPageModel()
        {
            Instruction = new List<string>
            { 
                PAGING,
                CRITERIAS,
                POS_SORT,
                CVURL
            };
        }
    }
}
