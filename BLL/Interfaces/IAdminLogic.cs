using BLL.Models;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IAdminLogic
    {
        List<PageModel> GetPageModelList();
        List<PageModel> GetPageModelSortList(int pageItems, int page,string INDEX);
        List<PageModel> GetSearchPageModelList(int pageItems, int page, string name);
    }
}
