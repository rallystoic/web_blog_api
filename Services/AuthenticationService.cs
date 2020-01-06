using System; 
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;


// package to be install
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
//folder
using finalBlog.Models;
using finalBlog.Helper;
//jwt
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
// using Microsoft.IdentityModel.Tokens;
using System.Security.Claims; 
// poco class file = Models.Authpoco.cs
using Microsoft.EntityFrameworkCore; 

// convert to byte[]


namespace finalBlog.Services
{
	public class SaltHashed {
		public string StrSalt {get;set;}
		public string StrHashed {get;set;}
	}
	public  interface IAuthenticationService
	{
		// hashing user password with random salt and
		// return value with string salt and hashed .
		SaltHashed provideSaltHashed(string password);
			
		// get a salt string as an input and generate hash.
		bool verifyHashed(string password,string dbsalt,string dbhashed);

		// generate a token with System.Security.Claims
		string generatetoken(string username,string rolename,string userid);
		// register method
		//bool register(string uname,string upassword); 
		// authentication method
		//Task<ValidateInfomation> authenticate(string uname, string upassword);   
	}
	public class AuthenticationService : IAuthenticationService {
		private readonly ConnectionStrings _config;

		public AuthenticationService(
				ConnectionStrings config
				) {
			_config = config;
				
		}
		public SaltHashed provideSaltHashed(string password) 
		{
			byte[] salt = new byte[128 / 8];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(salt);
			}
			// derive a 256-bit subkey (use HMACSHA256 with 10,000 iterations)
			string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
						password: password,
						salt: salt,
						// prf: KeyDerivationPrf.HMACSHA1,
						prf : KeyDerivationPrf.HMACSHA256,
						iterationCount: 10000,
						numBytesRequested: 256 / 8));

			SaltHashed SaltHashed = new SaltHashed(){
			StrSalt = Convert.ToBase64String(salt),
			StrHashed = hashed
			};
			return SaltHashed;
		}

		// mark 3
		public bool verifyHashed(string password,string dbsalt,string dbhashed) {
			byte[] salt = Convert.FromBase64String(dbsalt);
			string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
						password: password,
						salt: salt,
						// prf: KeyDerivationPrf.HMACSHA1,
						prf : KeyDerivationPrf.HMACSHA256,
						iterationCount: 10000,
						numBytesRequested: 256 / 8));
			return dbhashed.Equals(hashed);
		}


		public string generatetoken(string username,string rolename,string userid)
		{
					var tokenmaker = new JwtSecurityTokenHandler();

					// retrieve key from appsetting
					string secretkey = _config.Secret;
					// convert string to byte array
					byte[] key =  Encoding.ASCII.GetBytes(secretkey);

				// Collection of claims that will be put inside token 
					IList<Claim> claimcollection = new List<Claim> 
					{ 
					new Claim("username",username),
					new Claim("role",rolename),
					new Claim("userid",userid),
					};
		ClaimsIdentity myclaim = new ClaimsIdentity(claimcollection);
		var descriptor  = new SecurityTokenDescriptor(){
					Subject = myclaim, 
					Expires = DateTime.Now.AddHours(6),
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) 
		};
		// create a token with SecurityTokenDescriptor
		SecurityToken token = tokenmaker.CreateToken(descriptor);
		// write a token to string
			return tokenmaker.WriteToken(token); 
		}





	}
}

