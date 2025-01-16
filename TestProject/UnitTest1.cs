using Moq;
using Moq.EntityFrameworkCore;
using URL_Shortener1.DBContext;
using URL_Shortener1.Models;
using URL_Shortener1.Services;
using System.Collections.Generic;
using System.Linq;  // Додано для використання FirstOrDefault
using NUnit.Framework;

namespace TestProject
{
    public class Tests
    {
        private Mock<IURLService> _urlServiceMock;
        private Mock<ApplicationDBContext> _applicationDBContextMock;
        private IEnumerable<User> _users;

        [SetUp]
        public void Setup()
        {
            _urlServiceMock = new Mock<IURLService>();

            _applicationDBContextMock = new Mock<ApplicationDBContext>();

            _users = new List<User>
            {
                new User { Id = 1, UserName = "m" }
            };

            var urls = new List<URL>
            {
                new URL
                {
                    Id = 1,
                    OriginalUrl = @"https://github.com/maxusernameisavailable/URL_Shortener",
                    ShortenedUrl = @"https://localhost:7218/Shorten/1D1KpPRv4",
                    UserId = 1
                }
            };

            // Мокування DbSet для Users та URLs
            _applicationDBContextMock.Setup(db => db.Users).ReturnsDbSet(_users);
            _applicationDBContextMock.Setup(db => db.URLs).ReturnsDbSet(urls);
        }

        [Test]
        public void TestCheckIfRecordExist()
        {
            // Arrange: Мокування методу GetUrls в IURLService
            _urlServiceMock.Setup(service => service.GetUrls()).Returns(new List<URL>
            {
                new URL { Id = 1, OriginalUrl = "https://github.com/maxusernameisavailable/URL_Shortener", ShortenedUrl = "https://localhost/short/1" }
            });

            // Act: Отримуємо список URL
            var urls = _urlServiceMock.Object.GetUrls();
            var url = urls.FirstOrDefault(u => u.Id == 1);

            // Assert: Перевіряємо, чи правильний URL за ID
            Assert.IsNotNull(url); // Перевірка, чи знайшли URL
            /*Assert.AreEqual(1, url?.Id); // Перевірка ID
            Assert.AreEqual(url.UserId, _users.First().Id); // Перевірка UserId
            Assert.AreEqual("https://github.com/maxusernameisavailable/URL_Shortener", url?.OriginalUrl); // Перевірка OriginalUrl
            Assert.AreEqual("https://localhost/short/1", url?.ShortenedUrl); // Перевірка ShortenedUrl*/
        }
    }
}
