using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using finalBlog.Models;
// ../Models/cto.cs
using finalBlog.Services;
// for authoritarian 
using Microsoft.AspNetCore.Authorization;
namespace finalBlog.Controllers
{
	// [Authorize]
	[Authorize (Policy = "RequireAdministrator")]
	[Route("api/[controller]")]
		[ApiController]
			public class userController : ControllerBase
		{
			// call AuthenticationService
			private readonly BlogContext _Context;
			private readonly IAuthenticationService _authservice;
			public userController(
					BlogContext Context,
					IAuthenticationService authservice
					)
			{
				_Context = Context ;
				_authservice = authservice ;
			}
			// Total users
			[HttpGet("total")]
				public IActionResult totaluser(){
					int totaln = _Context.users.Count();
					var total =   new { total = totaln};
					return Ok(total);
				}

			// get a collection of users perpage 
			// where 2 equals author rolename

			[HttpGet("page/{id}")]
				public IActionResult GetUserlist(int id) {
					
					int maxcon = 9 ;	
					int page = maxcon * id; 
					var users = from u in  _Context.users.OrderByDescending(x => x.userid).OrderByDescending(x => x.userid).Skip(page).Take(maxcon).ToList()
						join r in _Context.roles on u.roleid equals r.roleid
						where r.roleid == 2
						select new { 
							u.userid,u.username,u.userfirstname,u.userlastname,r.rolename
						};
					return Ok(users);
				}
			// get userdetail
			[HttpGet("detail/{id}")]
			public async Task<ActionResult> getuserdetail(int id){
						var userdetail = new Userdetail();
				user uservalue = await _Context.users.FirstOrDefaultAsync(x=> x.userid ==id);
				if (uservalue == null)
				{
					return Ok(userdetail);
				}
				role rolevalue = await _Context.roles.FirstOrDefaultAsync(x=> x.roleid == uservalue.roleid);

								userdetail.userid = uservalue.userid;
								userdetail.username = uservalue.username;
								userdetail.userfirstname = uservalue.userfirstname;
								userdetail.userlastname = uservalue.userlastname;
								userdetail.rolename  = rolevalue.rolename;
			return Ok(userdetail);
			}

			// create a user (author)
			// using service
			// role is default to author = 2
			[HttpPost]
				public IActionResult createuser(UserPayload userpayload) {
					SucceedInfomation succeedinfomation = new SucceedInfomation();
					if (userpayload.password.Length < 6)
					{
						succeedinfomation.Issucceed = false;
						succeedinfomation.SucceedDetail = "password must be  atleast 6 character";
							return Ok(succeedinfomation);
					}
					// find user if exist return false
					user finduser = _Context.users.FirstOrDefault(x=> x.username == userpayload.username);
					if ( finduser != null)
					{
						succeedinfomation.Issucceed = false;
						succeedinfomation.SucceedDetail = "username exited";
					}
					//generate salt and hash
					SaltHashed salthashed = _authservice.provideSaltHashed(userpayload.password);
					user Newuser = new user(){ 
						username = userpayload.username,
						userfirstname = userpayload.userfirstname,
						userlastname = userpayload.userlastname,
						usersalt = salthashed.StrSalt,
						userhashed = salthashed.StrHashed,
						roleid = 2
					};
					_Context.users.Add(Newuser);
					_Context.SaveChanges();
					return Ok(succeedinfomation);
				}
			// Update user operation
			// first find user if exist
			// if user didnt change password, return default value
			// if user change password , generate a new one 

			[HttpPut]
				public async  Task<ActionResult<SucceedInfomation>> updateuser(UserUpdatepayload userpl) {
					SucceedInfomation succeedinformation = new SucceedInfomation();
					//new object of hashsalted 
					SaltHashed salthashed = new SaltHashed();
					user finduser = await _Context.users.FirstOrDefaultAsync(x=> x.userid == userpl.userid);
					// if userpassword is not null then return a new value using service
					if (userpl.password != null) {
						salthashed = this._authservice.provideSaltHashed(userpl.password);
						finduser.usersalt = salthashed.StrSalt;
						finduser.userhashed = salthashed.StrHashed;
					};
					finduser.userfirstname = userpl.userfirstname;
					finduser.userlastname = userpl.userlastname;
					
					// if userpassword is null then retrun default value of hashed and salted
					_Context.users.Update(finduser);
					var result = await _Context.SaveChangesAsync();
					succeedinformation.SucceedDetail = result.ToString();
					return Ok(succeedinformation);
				}

			// delete user but if role is not author cancel operation
			// and return false
			[HttpDelete("{id}")]
				public async  Task<ActionResult<SucceedInfomation>> DeleteUser(int id) {
					SucceedInfomation succeedinformation = new SucceedInfomation();
					user uservalue = await _Context.users.FirstOrDefaultAsync( x => x.userid == id);
					if (uservalue.roleid != 2)
					{
						succeedinformation.Issucceed = false;
						succeedinformation.SucceedDetail = "failed";
						return Ok(succeedinformation);
					}
					_Context.users.Remove(uservalue);
					var result = await  _Context.SaveChangesAsync();
					succeedinformation.SucceedDetail = result.ToString();
					succeedinformation.SucceedDetail = result.ToString();
					return Ok(succeedinformation);
				}
		}
}
