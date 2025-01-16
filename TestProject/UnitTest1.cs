using Moq;
using Moq.EntityFrameworkCore;
using URL_Shortener1.DBContext;
using URL_Shortener1.Models;
using URL_Shortener1.Services;
using System.Collections.Generic;
using System.Linq;  // ������ ��� ������������ FirstOrDefault
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

            // ��������� DbSet ��� Users �� URLs
            _applicationDBContextMock.Setup(db => db.Users).ReturnsDbSet(_users);
            _applicationDBContextMock.Setup(db => db.URLs).ReturnsDbSet(urls);
        }

        [Test]
        public void TestCheckIfRecordExist()
        {
            // Arrange: ��������� ������ GetUrls � IURLService
            _urlServiceMock.Setup(service => service.GetUrls()).Returns(new List<URL>
            {
                new URL { Id = 1, OriginalUrl = "https://github.com/maxusernameisavailable/URL_Shortener", ShortenedUrl = "https://localhost/short/1" }
            });

            // Act: �������� ������ URL
            var urls = _urlServiceMock.Object.GetUrls();
            var url = urls.FirstOrDefault(u => u.Id == 1);

            // Assert: ����������, �� ���������� URL �� ID
            Assert.IsNotNull(url); // ��������, �� ������� URL
            /*Assert.AreEqual(1, url?.Id); // �������� ID
            Assert.AreEqual(url.UserId, _users.First().Id); // �������� UserId
            Assert.AreEqual("https://github.com/maxusernameisavailable/URL_Shortener", url?.OriginalUrl); // �������� OriginalUrl
            Assert.AreEqual("https://localhost/short/1", url?.ShortenedUrl); // �������� ShortenedUrl*/
        }
    }
}
