using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekat.Models;

namespace Projekat.Repository.Configurations
{
    public class ItemsInsideOrderConfiguration : IEntityTypeConfiguration<ItemsInsideOrder>
    {
        public void Configure(EntityTypeBuilder<ItemsInsideOrder> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

        }
    }
}
