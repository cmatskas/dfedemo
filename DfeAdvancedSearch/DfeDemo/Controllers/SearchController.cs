using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DfeDemo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DfeDemo.Controllers
{
	public class SearchController : Controller
    {
		private DataDbContext dbContext;

		public SearchController(DataDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var searchCriteria = new SearchCriteria()
			{
				LaData = await dbContext.Ladata.ToListAsync()
			};

			return View(searchCriteria);
        }

		[HttpPost]
		public async Task<IActionResult> Index(SearchCriteria searchCriteria)
		{
			if (ModelState.IsValid)
			{
				var results = await GetUserCaseData(searchCriteria);
				return View("Results", results);
			}

			return View(searchCriteria);
		}

		private async Task<List<UserCase>> GetUserCaseData(SearchCriteria searchCriteria)
		{
			var query = dbContext.UserCase.AsQueryable();

			if (!string.IsNullOrEmpty(searchCriteria.Name))
			{
				query = query.Where(s => s.Firstname == searchCriteria.Name);
				query = query.Where(s => s.Surname == searchCriteria.Name);
			}

			query = query.Where(s => s.Laid == searchCriteria.LaValue);
			query = query.Where(s => s.Absence == searchCriteria.Absence);
			query = query.Where(s => s.Bullying == searchCriteria.Bullying);
			query = query.Where(s => s.Other == searchCriteria.Other);


			return await query.ToListAsync();
		}
	}
}
