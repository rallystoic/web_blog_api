using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


// additional

using finalBlog.Models;
using finalBlog.Services;

namespace finalBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class loginController : ControllerBase
    {
	    private readonly BlogContext _Context;
	    private readonly IAuthenticationService _authservice;
	    public loginController(
			    BlogContext Context,
			    IAuthenticationService authservice
			    )
	    {
		    _Context = Context;
		    _authservice = authservice;
	    }
//	    [HttpGet]
//		    public IActionResult test()
//		    {
//			    string token = _authservice.generatetoken("sdfsdf","sdfsdfdsf","sdfsdfds");
//			    return Ok(token);
//		    }
//


		[HttpPost]
	    public IActionResult Login([FromBody]userrequest rq)
	    {

ValidateInfomation validateinfomation = new ValidateInfomation();
		user uservalue = _Context.users.FirstOrDefault(x => x.username == rq.rname);
		if(uservalue == null)
		{
			validateinfomation.IsSucceed = false;
			return Ok(validateinfomation);
		}
		// if not null then time to verify password	
		bool result = _authservice.verifyHashed(rq.rpassword,uservalue.usersalt,uservalue.userhashed);
		if(result == false)
		{
			validateinfomation.IsSucceed = false;
			return Ok(validateinfomation);
		}

		role rolevalue = _Context.roles.FirstOrDefault(x=> x.roleid == uservalue.roleid);
		// if true then generate a token
		validateinfomation.IsSucceed = true;
		validateinfomation.Token = _authservice.generatetoken(uservalue.username,rolevalue.rolename,uservalue.roleid.ToString());
		validateinfomation.Firstname = uservalue.userfirstname;
		validateinfomation.Lastname = uservalue.userlastname;
		validateinfomation.UserID = uservalue.userid;
		validateinfomation.role = rolevalue.rolename;
	return Ok(validateinfomation);	
	    }

    }
}
