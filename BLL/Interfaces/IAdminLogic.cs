using BLL.Models;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IAdminLogic
    {
        IEnumerable<PageModel> GetPageModels();
        List<PageModel> GetPageModelList();
        List<PageModel> GetPageModelSortList(int pageItems, int page, string INDEX);
        List<PageModel> GetPageModelFilterList(int pageItems, int page, string INDEX, string Position);
        List<PageModel> GetPageModelSearchList(int pageItems, int page, string name);
    }
}
