using BLL.Helpers;
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
        #region classes and contructor
        private const string DATE = "date";
        private const string EMAIL = "email";
        private const string POSITION = "position";

        private readonly IUnitOfWork _uow;
        public AdminLogic(IUnitOfWork uow)
        {
            _uow = uow;
        }
        #endregion

        public List<PageModel> GetPageModelList()
        {
            List<PageModel> result = new List<PageModel>();

            result = _uow
                .GetRepository<User>()
                .GetAll()
                .Include(u => u.Position)
                //  skip admin user
                .Where(u => u.Position.Name != "admin")
                .Select(u => new PageModel
                {
                    UserId = u.UserId,
                    DateCreate = u.DateCreate,
                    Email = u.Email,
                    Position = u.Position.Name,
                    FullName = u.FullName
                })
                .ToList();

            if (result == null)
            {
                return null;
            }

            return result;
        }

        public List<PageModel> GetPageModelSortList(int pageItems, int page, string INDEX)
        {
            var paging = new Paging();
            List<PageModel> result = new List<PageModel>();

            if (INDEX == null || INDEX.Length <= 0)
            {
                return null;
            }

            IEnumerable<PageModel> list = _uow
                .GetRepository<User>()
                .GetAll()
                .Include(u => u.Position)
                //  skip admin user
                .Where(u => u.Position.Name != "admin")
                .Select(u => new PageModel
                {
                    DateCreate = u.DateCreate,
                    Email = u.Email,
                    Position = u.Position.Name,
                    FullName = u.FullName
                })
                .Skip(paging.SkipItem(page, pageItems))
                .Take(pageItems);

            if (INDEX == DATE)
                result = list
                    .OrderBy(p => p.DateCreate)
                    .ToList();
            else if (INDEX == EMAIL)
                result = list
                    .OrderBy(p => p.Email)
                    .ToList();
            else if (INDEX == POSITION)
                result = list
                    .OrderBy(p => p.Position)
                    .ToList();
            else
                result = list
                    .OrderBy(p => p.DateCreate)
                    .ToList();

            if (result == null)
            {
                return null;
            }

            return result;
        }

        public List<PageModel> GetSearchPageModelList(int pageItems, int page, string name)
        {
            var paging = new Paging();
            List<PageModel> result = new List<PageModel>();
            result = _uow
                .GetRepository<User>()
                .GetAll()
                .Include(u => u.Position)
                //  skip admin user
                .Where(u => (u.FullName.Contains(name)) && (u.Position.Name != "admin"))
                .Select(u => new PageModel
                {
                    DateCreate = u.DateCreate,
                    Email = u.Email,
                    Position = u.Position.Name,
                    FullName = u.FullName
                })
                .OrderBy(p => p.FullName)
                .Skip(paging.SkipItem(page, pageItems))
                .Take(pageItems)
                .ToList();


            if (result == null)
            {
                return null;
            }

            return result;
        }

    }
}
