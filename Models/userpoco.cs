using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;

namespace finalBlog.Models
{
	// return type of userdetail
	//
	public class Userdetail{
		public int userid	{get;set;}  
		public string	username {get;set;} 
		public string	userfirstname {get;set;} 
		public string	userlastname {get;set;} 
		public string	rolename {get;set;} 
	}
}
