using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNet.Identity;
using URL_Shortener1.Models;

namespace URL_Shortener1.DBContext
{
    public class UserDBContext : 
    {
        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options) { }
    }
}
