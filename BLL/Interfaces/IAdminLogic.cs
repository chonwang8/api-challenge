using BLL.Models;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IAdminLogic
    {
        List<PageModel> GetPageModelList();
        List<PageModel> GetSearchPageModelList(string name);
    }
}
