using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using finalBlog.Models;
using finalBlog.Services;
namespace finalBlog.Controllers
{
	[Route("api/[controller]")]
		[ApiController]
			public class categoryController : ControllerBase
		{
			private readonly BlogContext _Context;

			public categoryController(BlogContext Context){

				_Context = Context;
			}


			// get a list of category

			[HttpGet]
				public async Task<IActionResult> GetCategories() {

					var categories = await _Context.categories.OrderByDescending(x=> x.categoryid).ToListAsync();


					return Ok(categories);
				}

			// Creat a new category
			[HttpPost]
				public IActionResult CreateCategory([FromBody]CategoryPayload categorypayload) {
					SucceedInfomation succeedinfomation = new SucceedInfomation();
					category Newcategory = new category(){
						categoryname = categorypayload.CategoryName
					};
					_Context.categories.Add(Newcategory);
					var result = _Context.SaveChanges();
					if (result == 0)
					{
						succeedinfomation.Issucceed = false;
						return Ok(succeedinfomation);
					}

					return Ok(succeedinfomation);


				}


			// Update a new Category

			[HttpPut]
				public IActionResult updatecategory(category catepayload) {
					SucceedInfomation succeedinfomation = new SucceedInfomation();
					category Newcategory = new category(){
						categoryid = catepayload.categoryid,
							   categoryname = catepayload.categoryname
					};
					_Context.categories.Update(Newcategory);
					var result = _Context.SaveChanges(); 
					if (result == 0)
					{
						succeedinfomation.Issucceed = false;
						return Ok(succeedinfomation);
					}
					return Ok(succeedinfomation);


				}


			// remove category
			[HttpDelete("{id}")]
				public IActionResult DeleteCategory(int id) {
					SucceedInfomation succeedinfomation = new SucceedInfomation();
					category Newcategory = new category(){
						categoryid = id
					};
					_Context.categories.Remove(Newcategory);
					var result = _Context.SaveChanges(); 
					if (result == 0)
					{
						succeedinfomation.Issucceed = false;
						return Ok(succeedinfomation);
					}
					return Ok(succeedinfomation);
				}


		}
}
