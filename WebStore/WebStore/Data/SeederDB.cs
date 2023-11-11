using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using WebStore.Constants;
using WebStore.Data.Entitties;
using WebStore.Data.Entitties.Identity;

namespace WebStore.Data
{
    public static class SeederDB
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using(var scope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppEFContext>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
                context.Database.Migrate();

                if (!context.Roles.Any())
                {
                    RoleEntity admin = new RoleEntity
                    {
                        Name = Roles.Admin
                    };
                    RoleEntity user = new RoleEntity
                    {
                        Name = Roles.User
                    };
                    var result = roleManager.CreateAsync(admin).Result;
                    if (result.Succeeded)
                    {
                        Console.WriteLine("----Роль успішно створено---- {0}", admin.Name);
                    }
                    else
                    {
                        Console.WriteLine("+++Помилка створення ролі+++ {0}", admin.Name);
                    }
                    result = roleManager.CreateAsync(user).Result;
                    if (result.Succeeded)
                    {
                        Console.WriteLine("----Роль успішно створено---- {0}", user.Name);
                    }
                    else
                    {
                        Console.WriteLine("+++Помилка створення ролі+++ {0}", user.Name);
                    }

                    
                }
            
                if(!context.Users.Any())
                {
                    var ivan = new UserEntity
                    {
                        FirstName = "Марко",
                        LastName = "Лисий",
                        Email = "lysyi@gmail.com",
                        UserName = "lysyi@gmail.com"
                    };
                    var result = userManager.CreateAsync(ivan, "123456").Result;
                    if (result.Succeeded)
                    {
                        Console.WriteLine("-------Користувача успішно створено-------- {0}", ivan.Email);
                        result = userManager.AddToRoleAsync(ivan, Roles.Admin).Result;
                        if(result.Succeeded) { Console.WriteLine("---Користувач {0} отримав роль {1}----", ivan.Email, Roles.Admin); }
                        else { Console.WriteLine("++++Помилка видчаі ролі {1} Користувачу {0}++++", ivan.Email, Roles.Admin); }
                    }
                    else
                    {
                        Console.WriteLine("+++++Помилка створення користувача++++++ {0}", ivan.Email);
                    }
                }

                if (!context.Categories.Any())
                {
                    
                    var laptop = new CategoryEntity
                    {
                        Name = "Ноутбуки",
                        Image = SaveUrlImage("https://content.rozetka.com.ua/goods/images/big_tile/334493612.jpg"),
                        Description = "Для роботи і навчання",
                        DateCreated = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        UserId=1
                    };

                    var clothes = new CategoryEntity
                    {
                        Name = "Одяг",
                        Image = SaveUrlImage("https://content2.rozetka.com.ua/goods/images/big_tile/377007608.jpg"),
                        Description = "Для дорослих і малих",
                        DateCreated = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        UserId = 1
                    };

                    context.Categories.Add(laptop);
                    context.Categories.Add(clothes);
                    context.SaveChanges();
                }

            }
        }

        private static string SaveUrlImage(string url)
        {
            string imageName = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    byte[] imageBytes = client.GetByteArrayAsync(url).Result;
                    imageName = Path.GetRandomFileName() + ".jpg";
                    string dirSaveImage = Path.Combine(Directory.GetCurrentDirectory(), "images", imageName);
                    // Save the downloaded image bytes to a file
                    File.WriteAllBytes(dirSaveImage, imageBytes);
                    return imageName;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error downloading or saving image: {ex.Message}");
                    return null;
                }
            }
        }
    }
}
