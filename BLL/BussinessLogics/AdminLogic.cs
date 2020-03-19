using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BLL.BussinessLogics
{
    public class AdminLogic : IAdminLogic
    {
        private readonly IUnitOfWork _uow;
        public AdminLogic(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public List<PageModel> GetPageModelList()
        {
            var user = _uow.GetRepository<User>().GetAll().Include(u => u.Position).ToList();
            if (user == null)
            {
                return null;
            }
            List<PageModel> result = new List<PageModel>();
            foreach (User u in user)
            {
                if (u.Position.Name != "admin")
                {

                    result.Add(new PageModel
                    {
                        DateCreate = u.DateCreate,
                        Email = u.Email,
                        Position = u.Position.Name
                    });
                }
            }
            return result;
        }
        public List<PageModel> GetSearchPageModelList(string name)
        {
            var user = _uow.GetRepository<User>().GetAll().Include(u => u.Position).Where(u => u.FullName.Contains(name)).ToList();
            if (user == null)
            {
                return null;
            }
            List<PageModel> result = new List<PageModel>();
            foreach (User u in user)
            {
                if (u.Position.Name != "admin")
                {

                    result.Add(new PageModel
                    {
                        DateCreate = u.DateCreate,
                        Email = u.Email,
                        Position = u.Position.Name,
                        FullName = u.FullName
                    });
                }
            }
            return result;
        }

    }
}
