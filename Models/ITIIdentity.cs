using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using userIdentity.Models;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public class ITIIdentity : IdentityDbContext<ApplicationUser>
    {
        public ITIIdentity()
        {
        }
        public ITIIdentity(DbContextOptions options) : base(options) { }
        
            public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
         
    }


}


    
