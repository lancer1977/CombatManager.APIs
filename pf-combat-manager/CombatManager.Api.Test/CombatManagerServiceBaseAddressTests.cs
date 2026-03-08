using CombatManager.Api;
using NUnit.Framework;

namespace CombatManager.Api.Test
{
    public class CombatManagerServiceBaseAddressTests
    {
        [Test]
        public void BaseAddress_Appends_Api_With_Single_Slash_When_No_Trailing_Slash()
        {
            var svc = new CombatManagerService("http://localhost:12457");
            Assert.That(svc.BaseAddress, Is.EqualTo("http://localhost:12457/api/"));
        }

        [Test]
        public void BaseAddress_Appends_Api_With_Single_Slash_When_Trailing_Slash()
        {
            var svc = new CombatManagerService("http://localhost:12457/");
            Assert.That(svc.BaseAddress, Is.EqualTo("http://localhost:12457/api/"));
        }
    }
}
