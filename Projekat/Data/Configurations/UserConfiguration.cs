using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekat.Models;

namespace Projekat.Repository.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id); //Podesavam primarni kljuc tabele

            builder.Property(x => x.Id).ValueGeneratedOnAdd(); //Kazem da ce se primarni kljuc
                                                               //automatski generisati prilikom dodavanja,
                                                               //redom 1 2 3...

            builder.Property(x => x.UserName).HasMaxLength(20);//kazem da je maks duzina 30 karaktera
            

            builder.Property(x => x.Name).HasMaxLength(20);//kazem da je maks duzina 30 karaktera

            builder.Property(x => x.Surname).HasMaxLength(20);//kazem da je maks duzina 30 karaktera

            builder.Property(x => x.UserEmail).HasMaxLength(30);//kazem da je maks duzina 30 karaktera

            builder.Property(x => x.Address).HasMaxLength(30);//kazem da je maks duzina 30 karaktera


        }
    }
}
