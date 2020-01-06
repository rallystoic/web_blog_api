using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using finalBlog.Models;
// ../Models/cto.cs
// ../Models/Entities.cs
// ../Models/Dbcontext.cs
using finalBlog.Services;
// ../Services/ImageService.cs

using System.IdentityModel.Tokens.Jwt;
// decoding jwt
using System.Text;

// using System.Text.Json;
// AddAuthorization
using Microsoft.AspNetCore.Authorization;
namespace finalBlog.Controllers
{
	[Authorize (Policy = "Require_author")]
	[Route("api/[controller]")]
		[ApiController]
			public class postcontentController : ControllerBase
		{
			private readonly BlogContext _Context;
			private readonly IImageService _imgservice;
			public postcontentController(
					BlogContext Context,
					IImageService imgservice
					) { 
				_Context = Context;	
				_imgservice = imgservice;
			}

			// highlight on front page
			// [AllowAnonymous]
			// [HttpGet("highlight")]
			// 	public IActionResult Gethighlight(){
			// 		var highlightValue = from h in _Context.highlights.OrderByDescending(x => x.highlightid).Take(4)
			// 			join pct in _Context.postcontents on h.postcontentid equals pct.postcontentid
			// 			join p in _Context.posts on pct.postid equals p.postid
			// 			select new {
			// 				p.postid,
			// 				p.posttitle,
			// 				p.titleimgurl,
			// 			posted = pct.postcontentcreated.AddHours(7).ToString("dd/MM/yyyy")
			// 			};
			// 		return Ok(highlightValue);
			// 	}
			[AllowAnonymous]
			[HttpGet("frontpage/highlight")]
			public IActionResult Returnhighlight(){
			var highlightValue = from h in _Context.highlights.OrderByDescending(x => x.highlightid).Take(8)
						join pct in _Context.postcontents on h.postcontentid equals pct.postcontentid
						join p in _Context.posts on pct.postid equals p.postid
						select new {
						p.postid,
						p.posttitle,
						p.titleimgurl,
						posted = pct.postcontentcreated.AddHours(7).ToString("dd/MM/yyyy")
						};
					return Ok(highlightValue);
				}

			// return a collection of post (9 content per page)
			// will be use on front page
			[AllowAnonymous]
			[HttpGet("page/{id}")]
				public IActionResult CollectionOfPosts(int id) {
					int maxcon = 9;
					int page = maxcon * id;
					var posttitles = from pct in  _Context.postcontents.OrderByDescending(x=> x.postcontentid).Skip(page).Take(maxcon).ToList()
						join p in _Context.posts on pct.postid equals p.postid
						select new { p.postid,p.posttitle,p.titleimgurl, posted = pct.postcontentcreated.AddHours(7).ToString("dd/MM/yyyy")
						};
					return Ok(posttitles);
				}


			// detail
			[AllowAnonymous]
			[HttpGet("detail/{id}")]
				public ActionResult<postdetail> PostcontentDetail(int id){
					postdetail pdt = new postdetail();
					var postdetail = from pct in _Context.postcontents
						join p in _Context.posts on pct.postid equals p.postid
						join c in _Context.contents on pct.contentid equals c.contentid
						where pct.postid == id
						select new {
							p.postid,p.posttitle,c.contentdetail, posted = pct.postcontentcreated.AddHours(7).ToString("dd/MM/yyyy")
						};

					foreach(var detail in postdetail)
					{
						pdt.postid = detail.postid;
						pdt.posttitle = detail.posttitle ;
						pdt.contentdetail = detail.contentdetail;
						pdt.posted = detail.posted;
					}
					return Ok(pdt);
				}




			// mark o
			[HttpGet("header")]	
				public ActionResult prototyping([FromHeader] string Authorization)
				{
					if(Authorization == null)
					{
						return BadRequest();
					}
					string token = Authorization.Substring(7);
					string[] splitout = token.Split(".");
					string payload = splitout[1]+"=";
					byte[]  bytepayload = Convert.FromBase64String(payload);
					string result = ASCIIEncoding.ASCII.GetString(bytepayload);
					return Ok(result);
				}



			/****************************************/  
			/*  ADMIN SECTION START BELOW*/	
			/****************************************/  
			[HttpGet("total")]
				public IActionResult Totalpost(){
					int totaln = _Context.postcontents.Count();
					var total =  new { total = totaln  };
					return Ok(total);
				}

			[HttpGet("tableformat/total")]
				public IActionResult Totalposttableformat(){
					int totaln = _Context.postcontents.Count();
					var total =  new { total = totaln  };
					return Ok(total);
				}
			[HttpGet("tableformat/page/{id}")]
				public IActionResult PostTableformat(int id) {
					int maxcon = 12;
					int page = maxcon * id;
					var posttitles = from pct in _Context.postcontents.OrderByDescending(x => x.postcontentid).Skip(page).Take(maxcon).ToList()
						join p in _Context.posts on pct.postid equals p.postid
						join u in _Context.users on pct.userid equals u.userid 
						join c in _Context.categories on pct.categoryid equals c.categoryid
						select new {
							p.postid,
							p.posttitle,
							u.userfirstname,
							u.userlastname,
							c.categoryname,
							posted = pct.postcontentcreated.AddHours(7).ToString("dd/MM/yyyy")
						};
					return Ok(posttitles);
				}
			// deletepostcontent 
			// first find postcontent by id 
			// then split down by finding post id and content id using postcontent metadata
			[HttpDelete("{id}")]
				public async Task<ActionResult<SucceedInfomation>> post(int id) {
					SucceedInfomation succeedinformation = new SucceedInfomation();
					postcontent postcontenvalue = await _Context.postcontents.FirstOrDefaultAsync( x => x.postid == id);
					post postvalue =  new post() { postid = postcontenvalue.postid};
					content contentvalue =  new content() { contentid = postcontenvalue.contentid};
					_Context.posts.Remove(postvalue);
					_Context.contents.Remove(contentvalue);
					var result = await  _Context.SaveChangesAsync();
					succeedinformation.SucceedDetail = result.ToString();	
					return Ok(succeedinformation);
				}
			// edit postcontent 
			//  ../Models/cto.cs line31
			//
			[HttpPut]
				public ActionResult Editpostcontent([FromForm]Updatepayload updatepayload) {
					SucceedInfomation succeedinfomation = new SucceedInfomation();
					//find postcontent metadata and use this metada to obtain post id and content id
					postcontent postcontentvalue =  _Context.postcontents.FirstOrDefault(x=>x.postid == updatepayload.postid);
					// find post id
					post postvalue = _Context.posts.FirstOrDefault(x=> x.postid == postcontentvalue.postid);
					// insert new imgurl if imgfile payload not null
					if (updatepayload.imgfile != null)
					{
						// remove old file but that come later 
						if (updatepayload.imgfile.FileName.EndsWith(".jpg"))
						{
							postvalue.titleimgurl = _imgservice.SaveAsJPG(updatepayload.imgfile);
						} else if (updatepayload.imgfile.FileName.EndsWith(".png")) {
							postvalue.titleimgurl = _imgservice.SaveAsPng(updatepayload.imgfile);
						}
					}
					// insert new value of postitle
					postvalue.posttitle = updatepayload.posttitle;
					// find content id
					content contentvalue = _Context.contents.FirstOrDefault(x=> x.contentid == postcontentvalue.contentid);
					// insert new value of content
					contentvalue.contentdetail = updatepayload.contentdetail;
					// the Update operation start here
					//
					_Context.posts.Update(postvalue);
					_Context.contents.Update(contentvalue);
					_Context.SaveChanges();
					return Ok(succeedinfomation);
				}
			// createpostcontent return Succeedinformation 
			// payload of postitle postcontent and img file(accepted only png or jpeg);
			[HttpPost]
				public async Task<ActionResult<SucceedInfomation>> createpostcontent([FromForm]PostPayload postpayload) {
					SucceedInfomation succeedinformation = new SucceedInfomation();
					post Newpost = new post(){ posttitle = postpayload.posttitle};
					if(postpayload.imgfile == null || postpayload.posttitle.Length < 3)
					{
						succeedinformation.Issucceed = false;
						succeedinformation.SucceedDetail = "Please complete the form";
						return Ok(succeedinformation);
					}
					if (postpayload.imgfile.FileName.EndsWith(".jpg"))
					{
						Newpost.titleimgurl = await _imgservice.SaveFileJpg(postpayload.imgfile);
					} else if (postpayload.imgfile.FileName.EndsWith(".png")) {
						Newpost.titleimgurl = await _imgservice.SaveFilePng(postpayload.imgfile);
					}
					// else if(postpayload.imgfile.Length  == 0)
					// {
					// 	succeedinformation.Issucceed = false;
					// 	succeedinformation.SucceedDetail = "file format invalide , pls use jpg or png";
					// 	return Ok(succeedinformation);
					// }
					content Newcontent = new content() { contentdetail = postpayload.contentdetail};
					_Context.posts.Add(Newpost);
					_Context.contents.Add(Newcontent);
					_Context.SaveChanges();
					var Newposcontent = new postcontent(){
						postid = Newpost.postid,
						       contentid = Newcontent.contentid,
						       userid = postpayload.userid,
						       categoryid = postpayload.categoryid,
						       postcontentcreated = DateTime.UtcNow
					};
					_Context.postcontents.Add(Newposcontent);
					_Context.SaveChanges();
					return succeedinformation;
				}

			// highlight section return posttitle
			// show highlight in admin secttion
			[HttpGet("tableformat/highlight")]
				public IActionResult GetTableformatHighligh()
				{
					var high = from h in _Context.highlights.OrderByDescending(x => x.highlightid).Take(10)
						join pct in _Context.postcontents on h.postcontentid equals pct.postcontentid
						join p in _Context.posts on pct.postid equals p.postid
						select new {
						p.postid,
						p.posttitle,
						};
					return Ok(high);
				}
			// create highlight post
			//
			[HttpPost("highlight/{id}")]
			public IActionResult Createhighlight(int id)
			{
				SucceedInfomation succeedinfomation = new SucceedInfomation();
				// find postocntent first if found then Add it to the highlight table
				postcontent postcontentValue = _Context.postcontents.FirstOrDefault(x=> x.postid == id);
				if (postcontentValue == null)
				{
					succeedinfomation.Issucceed = false;
					return Ok(succeedinfomation);
				}
				var checkhg  = _Context.highlights.FirstOrDefault(x=> x.postcontentid == postcontentValue.postcontentid);
				if( checkhg != null)
				{
					succeedinfomation.Issucceed = false;
					succeedinfomation.SucceedDetail = "duplicated content";
					return Ok(succeedinfomation);
				}
				var Newhighlight = new highlight();
				Newhighlight.postcontentid = postcontentValue.postcontentid;
				_Context.highlights.Add(Newhighlight);
				_Context.SaveChanges();
							
			succeedinfomation.Issucceed = true;
			return Ok(succeedinfomation);
			}
			// remove highlight post
			[HttpDelete("highlight/{id}")]
			public IActionResult deleteHighlight(int id)
			{
				SucceedInfomation succeedinfomation = new SucceedInfomation();
				// find postocntent first if found then Add it to the highlight table
				postcontent postcontentValue = _Context.postcontents.FirstOrDefault(x=> x.postid == id);
				
				
				var highlightValue = _Context.highlights.FirstOrDefault(x=> x.postcontentid == postcontentValue.postcontentid);
				if (highlightValue == null)
				{
					succeedinfomation.Issucceed = false;
					return Ok(succeedinfomation);
				}
				var Newhighlight = new highlight();
				_Context.highlights.Remove(highlightValue);
				_Context.SaveChanges();
			succeedinfomation.Issucceed = true;
			return Ok(succeedinfomation);
			}


			

			

    }
}
