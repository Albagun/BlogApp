using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore{

   public static class SeedData{

     public static void TestVerileriniDoldur(IApplicationBuilder app){

    var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();

    

         if (context != null)
            {
                if(context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if(!context.Tags.Any())
                {
                  
                      context.Tags.AddRange(
                      
                      new Entity.Tag{ Text ="web programlama", Url="web-programlama", Color= Entity.Tag.TagColors.warning},
                      new Entity.Tag{ Text ="backend", Url="backend", Color= Entity.Tag.TagColors.info},
                      new Entity.Tag{ Text ="frontend", Url="frontend", Color= Entity.Tag.TagColors.success},
                      new Entity.Tag{ Text ="fullstack", Url="fullstack", Color= Entity.Tag.TagColors.secondary},
                      new Entity.Tag{ Text ="php", Url="php", Color= Entity.Tag.TagColors.primary}
                       
                      );

                      context.SaveChanges();

                }

                 if (!context.Users.Any())
                    {

                        context.Users.AddRange(
                        new Entity.User {UserName = "YusufGüneş",Name= "Yusuf Güneş",Email= "ygunes@hotmail.com",Password="ygunes" ,Image ="p1.jpg"},
                        new Entity.User {UserName = "Altai Güneş",Name= "Altai Güneş",Email= "agunes@hotmail.com",Password="agunes", Image ="p2.jpg"}
                        );

                        context.SaveChanges();

                    }

                      if (!context.Posts.Any())
                    {

                        context.Posts.AddRange(
                        new Entity.Post {
                            Title= "Asp.net core",
                            Description="Asp.net core dersleri ",
                            Content = "Asp.net core dersleri",
                            Url="aspnet-core",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-10),
                            Tags =context.Tags.Take(3).ToList(),
                            Image = "1.jpg",
                            UserId = 1,
                             Comments = new List<Comment> {
                                new Comment {Text ="İyi bir kurs",  PublishedOn= DateTime.Now.AddDays(-10), UserId = 1},
                                 new Comment {Text ="Harika", PublishedOn= DateTime.Now.AddDays(-20), UserId = 2},

                             }
                            
                        },
                    

                        new Entity.Post {
                            Title= "Php ",
                            Description="php dersleri ",
                            Content = "php dersleri",
                            Url="php-dersleri",
                            IsActive = true,
                            Image = "2.jpg",
                            PublishedOn = DateTime.Now.AddDays(-20),
                            Tags =context.Tags.Take(2).ToList(),
                            UserId = 1 
                        },

                         new Entity.Post {
                            Title= "Django",
                            Description="Django dersleri ",
                            Content = "django dersleri",
                            Url="django-dersleri",
                            IsActive = true,
                              Image = "3.jpg",
                            PublishedOn = DateTime.Now.AddDays(-5),
                            Tags =context.Tags.Take(4).ToList(),
                            UserId = 2 
                        }
                        );

                        context.SaveChanges();


                    }



            }
           
           
   }

   }
}