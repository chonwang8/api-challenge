using System.Collections.Generic;

namespace BLL.Models
{
    public class AdminPageModel
    {
        public const string SPACE = "";
        public const string CRITERIAS = "Available {OrderBy} values: (default)'DateCreate', 'Email', 'FullName', 'Position'";
        public const string REMINDER = "Please input these indexes correctly";

        public const string ADMIN = "Main Page  ->  /GET admin";
        public const string AdminDefault = "Default : itemPerPage(All)  -  DestinationPage(1)  -  OrderBy(DateCreate)";

        public const string PAGING = "For paging  ->  /GET admin/sort/{itemsPerPage}/{DestinationPage}/{OrderBy}";
        public const string PagingExample = "Example : /admin/sort/20/1/fullname";
        public const string PagingDefault = "Default : itemPerPage(40)  -  DestinationPage(1)  -  OrderBy(DateCreate)";
        public const string PagingRequired = "Required : non";


        public const string SORT = "For position filtering  ->  /GET admin/sort//{itemsPerPage}/{DestinationPage}/{OrderBy}/{Position}";
        public const string SortExample = "Example : /admin/role/20/1/datecreate/junior";
        public const string SortDefault = "Default : itemPerPage(40)  -  DestinationPage(1)  -  OrderBy(DateCreate)";
        public const string SortRequired = "Required : Position";

        public const string CV = "To get Candidate's Cv  ->  /GET admin/{userId}";
        public const string CvExample = "Example : /admin/0000-0000-0000-0000";
        public const string CvDefault = "Default : non";
        public const string CvRequired = "Required : userId/cv/{userId}";
        public const string CvNote = "Note : Use Previous Functions to Acquire UserId";
        public List<string> Instructions { get; set; }
        public List<PageModel> UserList { get; set; }

        public AdminPageModel()
        {
            Instructions = new List<string>
            { 
                ADMIN,
                AdminDefault,

                SPACE,

                CRITERIAS,
                REMINDER,

                SPACE,

                PAGING,
                PagingExample,
                PagingDefault,
                PagingRequired,

                SPACE,

                SORT,
                SortExample,
                SortDefault,
                SortRequired,

                SPACE,

                CV,
                CvExample,
                CvDefault,
                CvRequired,
                CvNote
            };
        }
    }
}
