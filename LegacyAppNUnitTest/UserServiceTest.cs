using NUnit.Framework;
using LegacyApp;

namespace LegacyAppNUnitTest
{
    public class UserServiceTest
    {
        private UserService _userService;
        [SetUp]
        public void Setup()
        {
            _userService = new UserService();
         }

        [Test]
        public void AddUserTestPass()
        {
            bool result = _userService.AddUser("Alvin", "Patrimonio", "alvin.patrimonio@example.com", new System.DateTime(1966, 11, 17), 4);
            Assert.IsTrue(result);
        }
        [Test]
        public void AddUserTestFail()
        {
            bool result = _userService.AddUser("Alvin", "Patrimonio", "alvin.patrimonio example.com", new System.DateTime(1966, 11, 17), 4);
            Assert.IsFalse(result);
        }
    }
}