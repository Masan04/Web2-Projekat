using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekat.Models;

namespace Projekat.Repository.Configurations
{
    public class ItemOrderConfiguration : IEntityTypeConfiguration<ItemOrder>
    {
        public void Configure(EntityTypeBuilder<ItemOrder> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

        }
    }
}
