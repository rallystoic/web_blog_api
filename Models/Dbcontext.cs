using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore.Design;

namespace finalBlog.Models
{
	public class BlogContext : DbContext
	{
		public BlogContext(DbContextOptions<BlogContext> options)
			: base(options)
		{
		}

		public DbSet<user> users {get;set;}
		public DbSet<post> posts {get;set;}
		public DbSet<content> contents {get;set;}
		public DbSet<postcontent> postcontents {get;set;}
		public DbSet<category> categories {get;set;}
		public DbSet<role> roles {get;set;}
		public DbSet<highlight> highlights {get;set;}


		// seeding data to database
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<role>().HasData(
					new role{ roleid =1,rolename = "administrator"},
					new role { roleid =2,rolename = "author"}
					);
			modelBuilder.Entity<user>().HasData(
					new user {
					userid =1,
					username = "admin",
					usersalt = "cztlN43JJ1nDn1NWuvAnpA==",
					userhashed = "9DDbStYYrn8ExReQN62o5J3wpVZUztBVMoEFqW3Ko2A=",
					userfirstname = "admin",
					userlastname = "admin",
					roleid = 1
					},
					new user {
					userid = 2,
					username = "koala02",
					usersalt = "vuc7ESG50L8bY9gUjvx9HQ==",
					userhashed = "CTXXZzNjBghOuo1X2353N7VWJNysSVwcXR2+VsiZwuA=",
					userfirstname = "koala",
					userlastname = "koalo",
					roleid = 2
					}
					);
		}

	}


}
