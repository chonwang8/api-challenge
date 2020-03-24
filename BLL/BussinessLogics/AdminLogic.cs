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
        private const string DATE = "date";
        private const string EMAIL = "email";
        private const string FULLNAME = "fullname";
        private const string POSITION = "position";

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
                })
                .OrderByDescending(u => u.DateCreate);
            if (result == null)
            {
                return null;
            }

            return result;
        }



        public List<PageModel> GetPageModelList()
        {
            List<PageModel> result = GetPageModels()
                .OrderBy(u => u.DateCreate)
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

            IEnumerable<PageModel> list = GetPageModels()
                .Skip(paging.SkipItem(page, pageItems))
                .Take(pageItems)
                .ToList();

            if (INDEX == null || INDEX.Length <= 0)
                INDEX = DATE;

            #region Sorting Condition Returns
            if (INDEX == DATE)
                result = list
                    .OrderByDescending(p => p.DateCreate)
                    .ToList();
            else if (INDEX == EMAIL)
                result = list
                    .OrderByDescending(p => p.Email)
                    .ToList();
            else if (INDEX == POSITION)
                result = list
                    .OrderByDescending(p => p.Position)
                    .ToList();
            else if (INDEX == FULLNAME)
                result = list
                    .OrderByDescending(p => p.FullName)
                    .ToList();
            else
                result = list
                    .OrderByDescending(p => p.DateCreate)
                    .ToList();
            #endregion

            if (result == null)
            {
                return null;
            }

            return result;
        }



        public List<PageModel> GetPageModelFilterList(int pageItems, int page, string INDEX, string Position)
        {
            var paging = new Paging();
            List<PageModel> result = new List<PageModel>();

            IEnumerable<PageModel> list = GetPageModels()
                .Skip(paging.SkipItem(page, pageItems))
                .Take(pageItems);

            #region Sorting Condition Returns
            if (INDEX == null || INDEX.Length <= 0)
                INDEX = DATE;

            if (INDEX == DATE)
                result = list
                    .OrderByDescending(p => p.DateCreate)
                    .ToList();
            else if (INDEX == EMAIL)
                result = list
                    .OrderByDescending(p => p.Email)
                    .ToList();
            else if (INDEX == POSITION)
                result = list
                    .OrderByDescending(p => p.Position)
                    .ToList();
            else if (INDEX == FULLNAME)
                result = list
                    .OrderByDescending(p => p.FullName)
                    .ToList();
            else
                result = list
                    .OrderByDescending(p => p.DateCreate)
                    .ToList();
            #endregion

            #region Check Position Input
            if (Position != "junior" && Position != "mid-level" && Position != "senior")
            {
                throw new ArgumentException("Invalid Candidate's Position");
            }

            if (Position == "junior")
                result = list
                    .Where(u => u.Position == "junior")
                    .ToList();
            else if (Position == "mid-level")
                result = list
                    .Where(u => u.Position == "mid-level")
                    .ToList();
            else if (Position == "senior")
                result = list
                    .Where(u => u.Position == "senior")
                    .ToList();
            else
                result = null;
            #endregion

            if (result == null)
            {
                return null;
            }

            return result;
        }


        public List<PageModel> GetPageModelSearchList(int pageItems, int page, string name)
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
                    UserId = u.UserId,
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
