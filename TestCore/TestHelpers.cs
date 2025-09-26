using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using Moq;

namespace TestCore
{
    public class TestHelpers
    {
        [SetUp]
        public void Setup()
        {
        }

        //Happy flow
        [Test]
        public void TestPasswordHelperReturnsTrue()
        {
            string password = "user3";
            string passwordHash = "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=";
            Assert.IsTrue(PasswordHelper.VerifyPassword(password, passwordHash));
        }

        [TestCase("user1", "IunRhDKa+fWo8+4/Qfj7Pg==.kDxZnUQHCZun6gLIE6d9oeULLRIuRmxmH2QKJv2IM08=")]
        [TestCase("user3", "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA=")]
        public void TestPasswordHelperReturnsTrue(string password, string passwordHash)
        {
            Assert.IsTrue(PasswordHelper.VerifyPassword(password, passwordHash));
        }

        //Unhappy flow
        [Test]
        public void TestPasswordHelperReturnsFalse()
        {
            string password = "user3";
            string invalidPasswordHash = "invalidHashFormat";
            Assert.IsFalse(PasswordHelper.VerifyPassword(password, invalidPasswordHash));
        }

        [TestCase("user1", "IunRhDKa+fWo8+4/Qfj7Pg==.kDxZnUQHCZun6gLIE6d9oeULLRIuRmxmH2QKJv2IM08")] 
        [TestCase("user3", "sxnIcZdYt8wC8MYWcQVQjQ==.FKd5Z/jwxPv3a63lX+uvQ0+P7EuNYZybvkmdhbnkIHA")] 
        public void TestPasswordHelperReturnsFalse(string password, string passwordHash)
        {
            Assert.IsFalse(PasswordHelper.VerifyPassword(password, passwordHash));
        }


        [TestCase("xyz")]
        [TestCase("zzz")]
        [TestCase("peerx")]
        public void TestProductSearchUnhappyPath(string searchTerm)
        {
            var products = new List<Product>
            {
                new Product(1, "Appel", 5),
                new Product(2, "Banaan", 3),
                new Product(3, "Peer", 2),
                new Product(4, "Ananas", 1)
            };

            var filtered = products
                .Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Assert.That(filtered.Count, Is.EqualTo(0));
        }

        [TestCase("an", new[] { "Banaan", "Ananas" })]
        [TestCase("App", new[] { "Appel" })]
        [TestCase("PE", new[] { "Appel", "Peer" })]
        public void TestProductSearchHappyPath(string searchTerm, string[] expectedNames)
        {
            var products = new List<Product>
            {
                new Product(1, "Appel", 5),
                new Product(2, "Banaan", 3),
                new Product(3, "Peer", 2),
                new Product(4, "Ananas", 1)
            };

            var filtered = products
                .Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Assert.That(filtered.Count, Is.EqualTo(expectedNames.Length));
            Assert.That(filtered.Select(p => p.Name), Is.EqualTo(expectedNames));
        }
    }
}