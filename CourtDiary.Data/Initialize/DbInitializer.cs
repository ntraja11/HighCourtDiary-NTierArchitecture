using CourtDiary.Data.Context;
using CourtDiary.Data.Models;
using CourtDiary.Data.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CourtDiary.Data.Initialize
{
    public class DbInitializer : IDbInitializer
    {
        private readonly CourtDiaryDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DbInitializer(CourtDiaryDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task InitializeAsync()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();                    
                }

                if (!await _roleManager.RoleExistsAsync(StaticDetails.RoleSuperAdmin))
                {
                    await _roleManager.CreateAsync(new IdentityRole(StaticDetails.RoleSuperAdmin));
                    await _roleManager.CreateAsync(new IdentityRole(StaticDetails.RoleOrganizationAdmin));
                    await _roleManager.CreateAsync(new IdentityRole(StaticDetails.RoleLawyer));
                    await _roleManager.CreateAsync(new IdentityRole(StaticDetails.RoleJunior));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error during database migration: {ex.Message}");
            }
        }
    }

}
