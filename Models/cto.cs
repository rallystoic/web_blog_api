using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;

namespace finalBlog.Models
{
	public class userrequest
	{
		public string rname {get;set;}
		public string rpassword {get;set;}
	}

	// return object after Authenticated
	public class Infostorage
	{

	}

	// 
	//  get UserID from localstorage
	//  get category from a fetch http category
	//  
	public class PostPayload
	{
		public string posttitle {get;set;}
		public string contentdetail {get;set;}
		public IFormFile imgfile {get;set;}
		public int categoryid {get;set;}
		public int userid {get;set;}
	}

	// ../Controllers/userController.cs 
	public class Updatepayload
	{
		public int postid {get;set;}
		public string posttitle {get;set;}
		public string contentdetail {get;set;}
		public IFormFile imgfile {get;set;}
		public int categoryid {get;set;}
	}
	public class CategoryPayload
	{
		public string CategoryName {get;set;}
	}
	// validate updateinformation from update Services
	public class SucceedInfomation
	{
		// SucceedInfomation
		// subject to change to Issucceed
		// and SucceedDetail
		public bool Issucceed {get;set;}
		public string SucceedDetail {get;set;}
		public SucceedInfomation(bool Issucceed,string SucceedDetail)
		{
			this.Issucceed = Issucceed;
			this.SucceedDetail = SucceedDetail;
		}
		//second Constructor
		public SucceedInfomation()
		{
			Issucceed = true;
		}
		//Third Constructor
		public SucceedInfomation(string SucceedDetail)
		{
			Issucceed = true;
			this.SucceedDetail = SucceedDetail;
		}
	}

	// Data object to return for getPostContent();
	public class ObjPostContent 
	{
		public int PostContentID {get;set;}
		public string PostTitle {get;set;}
		public string PostContent {get;set;}
		public string PostedDate {get;set;}
		public string AuthorFirstName {get;set;}
		public string AuthorLastName {get;set;}
	}
	//  return infomation after authenticated
	public class ValidateInfomation
	{
		public bool IsSucceed {get;set;}
		public int UserID {get;set;}
		public string Token {get;set;}
		public string Firstname {get;set;}
		public string Lastname {get;set;}
		public string role {get;set;}
	}
	public class RootObject
	{
		public RootObject()
		{
			this.ValidateInfomation = new List<ValidateInfomation>();

		}
		public int id {get;set;}
		public List<ValidateInfomation> ValidateInfomation {get;set;}
	}
	public class totalpost
	{
		//p.PostID,p.PostTitle, postedDateTime = pc.PostContentCreated.AddHours(7).ToString("dddd dd MMMM yyyy")
		//	,usr.UserFirstName,usr.UserLastName,ct.CategoryID,ct.CategoryName,
		public totalpost()
		{
			this.post = new List<post>();
		}
		public int total {get;set;}
		public List<post> post {get;set;}


	}

	// return title and timeposted for BlogTitle();
	public class TitleTime
	{
		public int PostID {get;set;}
		public string PostTitle {get;set;}
		public string PostedDateTime {get;set;}
	}


	// jsonformat
	public class imagepayload
	{
		public string base64 {get;set;}
	}



	// sending file with Ifromfile

	public class FormImage
	{
		public string  username {get;set;}
		public string  password {get;set;}
		public string  userfirstname {get;set;}
		public string  userlastname {get;set;}
		public IFormFile filename {get;set;}
	}

	public class Multiimage {
		public string name { get;set;}	
		public IFormFile file01{get;set;}
		public IFormFile file02{get;set;}
		public IFormFile file03{get;set;}
	}


	// this class payload is use to create user
	public class UserPayload
	{
		public string username {get;set;}
		public string password {get;set;}
		public string userfirstname {get;set;}
		public string userlastname {get;set;}
	}

	// this class payload is use to Update user
	public class UserUpdatepayload
	{
		public int  userid {get;set;}
		public string userfirstname {get;set;}
		public string userlastname {get;set;}
		public string password {get;set;}
	}
	
	// prototype
	public class tokeniden{ 
		public string UserName {get;set;} 
		public string role {get;set;} 
		public int nbf {get;set;}
		public int exp {get;set;}
		public int iat {get;set;}
	
	}
}

