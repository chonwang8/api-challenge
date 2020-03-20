using System.Collections.Generic;

namespace BLL.Models
{
    public class AdminPageModel
    {
        public const string INSTRUCTION = "For paging  ->  /GET base/(items per page)/(destination page)/(sorting criteria)\n " +
                "Available sorting criterias : (default)'email', 'date', 'fullname'\n\n " +
                "To get Candidate's Cv  ->  /GET base/(user Id)/(user Email)\n\n\n";
        public string Instruction { get; set; }
        public List<PageModel> UserList { get; set; }

        public AdminPageModel()
        {
            Instruction = INSTRUCTION;
        }
    }
}
