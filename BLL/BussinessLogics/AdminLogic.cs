using BLL.Helpers;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.BussinessLogics
{
    public class AdminLogic : IAdminLogic
    {
        #region classes and contructor
        private const string DATE = "DateCreate";
        private const string EMAIL = "Email";
        private const string FULLNAME = "FullName";
        private const string POSITION = "Position";

        private readonly IUnitOfWork _uow;
        public AdminLogic(IUnitOfWork uow)
        {
            _uow = uow;
        }
        #endregion



        public IEnumerable<PageModel> GetPageModels()
        {
            IEnumerable<PageModel> result = _uow
            .GetRepository<User>()
            .GetAll()
            .Include(u => u.Position)
            .Where(u => u.Position.Name != "admin") //  skip admin user
            .Select(u => new PageModel
            {
                UserId = u.UserId,
                DateCreate = u.DateCreate,
                Email = u.Email,
                Position = u.Position.Name,
                FullName = u.FullName
            });

            if (result == null)
            {
                return null;
            }

            return result;
        }



        public List<PageModel> GetPageModelList()
        {
            List<PageModel> result = GetPageModels()
            .OrderByDescending(u => u.DateCreate)
            .ToList();

            if (result == null)
            {
                return null;
            }

            return result;
        }



        public List<PageModel> GetPageModelSortList(int pageItems, int page, string INDEX)
        {
            if (INDEX == null || INDEX.Length <= 0)
                INDEX = DATE;
            var paging = new Paging();
            List<PageModel> result = new List<PageModel>();


            //  PageModel {UserId, Email, DateCreated, FullName, Position}
            var propertyInfo = typeof(PageModel).GetProperty(INDEX);

            //  Paging
            IEnumerable<PageModel> list = GetPageModels()
                .Skip(paging.SkipItem(page, pageItems))
                .Take(pageItems);

            // Sorting by input index
            result = list
                    .OrderBy(p => propertyInfo.GetValue(p, null))
                    .ToList();

            if (result == null)
            {
                return null;
            }

            return result;
        }



        public List<PageModel> GetPageModelFilterList(int pageItems, int page, string INDEX, string POSITION)
        {
            if (INDEX == null || INDEX.Length <= 0)
                INDEX = DATE;
            var paging = new Paging();
            List<PageModel> result = new List<PageModel>();


            //  Get propertyInfo 
            //  PageModel {UserId, Email, DateCreated, FullName, Position}
            var propertyInfo = typeof(PageModel).GetProperty(INDEX);

            //  Paging
            IEnumerable<PageModel> list = GetPageModels()
                .Skip(paging.SkipItem(page, pageItems))
                .Take(pageItems);

            // Sorting by input index
            list.OrderBy(p => propertyInfo.GetValue(p, null));

            //  Filter by Candidate's Position Input
            result = list.Where(u => u.Position == POSITION).ToList();

            if (result == null)
            {
                return null;
            }

            return result;
        }



        public List<PageModel> GetPageModelSearchList(int pageItems, int page, string NAME)
        {
            var paging = new Paging();
            string searchName = NAME.ToLower();
            List<PageModel> result = new List<PageModel>();

            result = GetPageModels()
                .Where(u => (u.FullName.ToLower().Contains(searchName)))
                .OrderBy(p => p.FullName)                       //  Sort
                .Skip(paging.SkipItem(page, pageItems))         //  Paging
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
