using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using NotlaGel.Entities;

namespace NotlaGel.DataAccessLayer.EntityFramework
{
    public class MyInitializer :CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            NotlaGelUser admin = new NotlaGelUser()
            {
                Name = "Metehan",
                SurName = "Metinoğlu",
                Email = "mete3777@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive=true,
                IsAdmin=true,
                UserName="metehan",
                ProfileImageFilename="img_avatar.png",
                Password="123456",
                CreatedOn=DateTime.Now,
                ModifiedOn=DateTime.Now.AddMinutes(5),
                ModifiedUsername="metehan"
            };
            NotlaGelUser standartuser = new NotlaGelUser()
            {
                Name = "Mete",
                SurName = "Metinoğlu",
                Email = "mete3778@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                UserName = "metehann",
                ProfileImageFilename = "img_avatar.png",
                Password = "654321",
                CreatedOn = DateTime.Now.AddHours(1),
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "metehan"
            };
            context.NotlaGelUsers.Add(admin);
            context.NotlaGelUsers.Add(standartuser);

            for(int i = 0; i < 8; i++)
            {
                NotlaGelUser user = new NotlaGelUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    SurName = FakeData.NameData.GetSurname(),
                    Email =  FakeData.NetworkData.GetEmail(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    UserName = $"user{i}",
                    ProfileImageFilename = "img_avatar.png",
                    Password = "123",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{i}"
                };
                context.NotlaGelUsers.Add(user);
            }
            context.SaveChanges();

            List<NotlaGelUser> userlist = context.NotlaGelUsers.ToList();

            //fake kategori ekleme
            for (int i = 0;i< 10;i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUsername = "metehan"
                };

                context.Categories.Add(cat);

                //Not ekleme
                for(int k=0; k < FakeData.NumberData.GetNumber(5, 9); k++)
                {
                    NotlaGelUser owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];
                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text=FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1,3)),
                        IsDraft=false,
                        LikeCount=FakeData.NumberData.GetNumber(1,9),
                        Owner=owner,
                        CreatedOn=FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1),DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername= owner.UserName
                    };
                    cat.Notes.Add(note);

                    //yorum ekleme
                    for(int j = 0; j < FakeData.NumberData.GetNumber(3, 5); j++)
                    {
                        NotlaGelUser comment_owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];


                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Owner = comment_owner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = comment_owner.UserName
                        };
                        note.Comments.Add(comment);
                    }

                    //like ekleme
                    

                    for(int m = 0; m < note.LikeCount; m++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userlist[m]

                        };
                        note.Likes.Add(liked);
                    }
                }
            }
            context.SaveChanges();
        }
    }
}
