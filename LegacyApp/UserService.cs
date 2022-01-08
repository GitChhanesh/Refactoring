using System;


namespace LegacyApp
{
	public class UserService
	{
		public bool AddUser(string firname, string surname, string email, DateTime dateOfBirth, int clientId)
		{

			try
			{
				var user = new User
				{
					DateOfBirth = dateOfBirth,
					EmailAddress = email,
					Firstname = firname,
					Surname = surname
				};
				//Validation service
				var validationService = new ValidationService();
				if (!validationService.ValidateUser(user))
					return false;

				// Client Repository
				var clientRepository = new ClientRepository();
				var client = clientRepository.GetById(clientId);
				user.Client = client;

				// Credit check 

				user.HasCreditLimit = false;
				if (client.Name != "VeryImportantClient")
				{
					// Do credit check
					user.HasCreditLimit = true;
					int creditLimit;
					using (var userCreditService = new UserCreditServiceClient())
					{
						creditLimit = userCreditService.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth);
					}
					if (client.Name != "ImportantClient")
						user.CreditLimit = creditLimit;
					else
						// Do credit check and double credit limit
						user.CreditLimit = creditLimit * 2;
				}
				if (user.HasCreditLimit && user.CreditLimit < 500)
				{
					return false;
				}
				//Add user
				UserDataAccess.AddUser(user);
				return true;
			}
			catch (Exception ex)
			{
				Console.Write(ex.Message);
				return false;
			}


		}
	}
}


