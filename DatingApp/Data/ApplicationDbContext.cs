using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data
{
    // ApplicationDbContext with Identity, all PKs as int
    public class ApplicationUser : IdentityUser<int> { }
    public class ApplicationRole : IdentityRole<int> { }
    public class ApplicationUserClaim : IdentityUserClaim<int> { }
    public class ApplicationUserRole : IdentityUserRole<int> { }
    public class ApplicationUserLogin : IdentityUserLogin<int> { }
    public class ApplicationRoleClaim : IdentityRoleClaim<int> { }
    public class ApplicationUserToken : IdentityUserToken<int> { }

    public class ApplicationDbContext : IdentityDbContext<
        ApplicationUser,
        ApplicationRole,
        int,
        ApplicationUserClaim,
        ApplicationUserRole,
        ApplicationUserLogin,
        ApplicationRoleClaim,
        ApplicationUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
    }
}
