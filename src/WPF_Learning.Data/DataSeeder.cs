﻿using Microsoft.AspNetCore.Identity;
using WPF_Learning.Core.Domain.Identity;

namespace WPF_Learning.Data
{
    public class DataSeeder
    {
        public async Task SeedAsync(ApplicationDbContext context)
        {
            var passwordHasher = new PasswordHasher<AppUser>();

            var rootAdminRoleId = Guid.NewGuid();
            var userRoleId = Guid.NewGuid();
            if (!context.Roles.Any())
            {
                await context.Roles.AddAsync(new AppRole()
                {
                    Id = rootAdminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    DisplayName = "Quản trị viên"
                });
                await context.Roles.AddAsync(new AppRole()
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER",
                    DisplayName = "Người dùng"
                });
                await context.SaveChangesAsync();
            }

            if (!context.Users.Any())
            {
                //User Admin
                var userAdminId = Guid.NewGuid();
                var userId = Guid.NewGuid();
                var userAdmin = new AppUser()
                {
                    Id = userAdminId,
                    FirstName = "Huy",
                    LastName = "Tran",
                    Email = "huytq@ics-p.vn",
                    NormalizedEmail = "HUYTQ3103@GMAIL.COM",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    IsActive = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    LockoutEnabled = false,
                    DateCreated = DateTime.Now,
                };
                userAdmin.PasswordHash = passwordHasher.HashPassword(userAdmin, "Admin@123$");
                await context.Users.AddAsync(userAdmin);
                await context.UserRoles.AddAsync(new IdentityUserRole<Guid>()
                {
                    RoleId = rootAdminRoleId,
                    UserId = userAdminId
                });

                // User người dùng
                var user = new AppUser()
                {
                    Id = userId,
                    FirstName = "Huy",
                    LastName = "Tran",
                    Email = "huytq3103@gmail.com",
                    NormalizedEmail = "HUYTQ3103@GMAIL.COM",
                    UserName = "huytq",
                    NormalizedUserName = "HUYTQ",
                    IsActive = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    LockoutEnabled = false,
                    DateCreated = DateTime.Now,
                };
                user.PasswordHash = passwordHasher.HashPassword(user, "Admin@123$");
                await context.Users.AddAsync(user);
                await context.UserRoles.AddAsync(new IdentityUserRole<Guid>()
                {
                    RoleId = userRoleId,
                    UserId = userId
                });

                await context.SaveChangesAsync();
            }
        }
    }
}