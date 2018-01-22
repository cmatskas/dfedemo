using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DfeDemo.Data;
using DfeDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DfeDemo.ViewComponents
{
    public class AdvancedSearchViewComponent : ViewComponent
	{
		private DataDbContext dbContext;
		public AdvancedSearchViewComponent(DataDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<IViewComponentResult> InvokeAsync(SearchCriteria searchCriteria)
		{
			var items = await GetUserCaseData(searchCriteria);
			return View(items);
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
