using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;

namespace finalBlog.Models
{


	// return Type of postdetail 
	public class  postdetail {
		public int postid {get;set;} 
		public string posttitle {get;set;} 
		public string contentdetail {get;set;} 
		public string posted {get;set;} 
		public int categoryid {get;set;} 
		public string categoryname {get;set;} 
	}
}
