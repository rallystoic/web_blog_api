using System.Collections.Generic;
using System;

namespace finalBlog.Models
{
    public class user
    {
        public int  userid {get;set;}
        public string username {get;set;}
        public string usersalt {get;set;}
        public string userhashed {get;set;}
        public string userfirstname {get;set;}
        public string userlastname {get;set;}
        public int  roleid {get;set;}
	public role role {get;set;}
	//public DateTime UserCreated {get;set;}
       public ICollection<postcontent> postcontents {get;set;}
    }
    //role_table
    public class role
    {
        public int  roleid {get;set;}
        public string rolename {get;set;}
	public ICollection<user> users {get;set;}
    }
    

    //category_table
    public class category
    {
        public int categoryid {get;set;}
        public string  categoryname {get;set;}
        public ICollection<postcontent> postcontents {get;set;}
    }
// post_talble
    public class post
    {
        public int postid {get;set;}
        public string posttitle{get;set;}
	public string titleimgurl {get;set;}
        public ICollection<postcontent> postcontents {get;set;}
    }
    public class content
    {
        public int contentid {get;set;}
        public string contentdetail{get;set;}
        public ICollection<postcontent> postcontents {get;set;}
    }
   //PostContent_table 
    public class postcontent
    {
        public int postcontentid {get;set;}
        public int postid  {get;set;}
	public post post {get;set;}
	public int contentid {get;set;}
	public content content {get;set;}
        public int userid {get;set;}
        public user user {get;set;}
	public int categoryid {get;set;}
	public category category {get;set;}
	public DateTime postcontentcreated {get;set;}
	public ICollection<highlight> highlights {get;set;}
	
    }

    public class highlight 
	 {
		 public int highlightid {get;set;}
		 public int postcontentid {get;set;}
		 public postcontent postcontent {get;set;}
           
	}


}

