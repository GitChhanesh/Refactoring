using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LegacyApp
{
    public class ValidationService
    {
		public bool ValidateUser(User user)
		{
			try
			{

				if (string.IsNullOrEmpty(user.Firstname) || string.IsNullOrEmpty(user.Surname))
				{
					return false;
				}

				if (!IsValidEmail(user.EmailAddress))
				{
					return false;
				}

				var now = DateTime.Now;
				int age = now.Year - user.DateOfBirth.Year;
				if (now.Month < user.DateOfBirth.Month || (now.Month == user.DateOfBirth.Month && now.Day < user.DateOfBirth.Day)) age--;

				if (age < 21)
				{
					return false;
				}
				return true;
			}
			catch 
			{
				return false;
			}

		}

		public static bool IsValidEmail(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
				return false;

			try
			{
				// Normalize the domain
				email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
									  RegexOptions.None, TimeSpan.FromMilliseconds(200));

				// Examines the domain part of the email and normalizes it.
				string DomainMapper(Match match)
				{
					// Use IdnMapping class to convert Unicode domain names.
					var idn = new IdnMapping();

					// Pull out and process domain name (throws ArgumentException on invalid)
					string domainName = idn.GetAscii(match.Groups[2].Value);

					return match.Groups[1].Value + domainName;
				}
			}
			catch 
			{
				return false;
			}
			try
			{
				return Regex.IsMatch(email,
					@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
					RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
			}
			catch 
			{
				return false;
			}
		}
	}
}
