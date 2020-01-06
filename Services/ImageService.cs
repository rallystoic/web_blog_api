using System; 
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;




namespace finalBlog.Services
{

	public interface IImageService	
	{
		// without async
		string SaveAsJPG(IFormFile image);
		string SaveAsPng(IFormFile image);
		// use of filestream async
		Task<string> SaveAsJPGasync(IFormFile image);
		Task<string> SaveAsPNGasync(IFormFile image);
		// use static system.io.file
		Task<string> SaveFilePng(IFormFile image);
		Task<string> SaveFileJpg(IFormFile image);
	}
	public class ImageService : IImageService
	{
		// *************************************************
		//
		// user upload imagefile with file type .jpg or png
		// this class handle .jpg and png file separately.
		//
		// *************************************************
		public string SaveAsJPG(IFormFile image)
		{
			string guidname = Guid.NewGuid().ToString();
			string filepath = "wwwroot/images/articles/title_image/" + guidname + ".jpg";
			string urlpath = "/images/articles/title_image/" + guidname + ".jpg";

			using(var fs = new FileStream(filepath,FileMode.Create))
			{
				image.CopyTo(fs);
			}



			return urlpath;
		}


		public string SaveAsPng(IFormFile image)
		{
			string guidname = Guid.NewGuid().ToString();
			string filepath = "wwwroot/images/articles/title_image/" + guidname + ".png";
			string urlpath = "/images/articles/title_image/" + guidname + ".png";
			using(var fs = new FileStream(filepath,FileMode.Create))
			{
				image.CopyTo(fs);
			}
			return urlpath;
		}
		//async method
		public async Task<string> SaveAsJPGasync(IFormFile image)
		{
			string guidname = Guid.NewGuid().ToString();
			string filepath = "wwwroot/images/articles/title_image/" + guidname + ".jpg";
			string urlpath = "/images/articles/title_image/" + guidname + ".jpg";

			using(var fs = new FileStream(filepath,FileMode.Create))
			{
				await image.CopyToAsync(fs);
			}



			return urlpath;
		}


		//async method
		public async Task<string> SaveAsPNGasync(IFormFile image)
		{
			string guidname = Guid.NewGuid().ToString();
			string filepath = "wwwroot/images/articles/title_image/" + guidname + ".png";
			string urlpath = "/images/articles/title_image/" + guidname + ".png";
			using(var fs = new FileStream(filepath,FileMode.Create))
			{
				await image.CopyToAsync(fs);
			}
			return   urlpath;
		}
		public async Task<string> SaveFilePng(IFormFile image)
		{
			string guidname = Guid.NewGuid().ToString();
			string filepath = "wwwroot/images/articles/title_image/" + guidname + ".png";
			string urlpath = "/images/articles/title_image/" + guidname + ".png";
			using(var stream = System.IO.File.Create(filepath))
			{
				await image.CopyToAsync(stream);
			}
			return   urlpath;
		}
		public async Task<string> SaveFileJpg(IFormFile image)
		{
			string guidname = Guid.NewGuid().ToString();
			string filepath = "wwwroot/images/articles/title_image/" + guidname + ".jpg";
			string urlpath = "/images/articles/title_image/" + guidname + ".jpg";
			using(var stream = System.IO.File.Create(filepath))
			{
				await image.CopyToAsync(stream);
			}
			return   urlpath;
		}
		//	public async Task  SaveImage(IFormFile image)
		//	{
		//		



		//	}



	}

}


