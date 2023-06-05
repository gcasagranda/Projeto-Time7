using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Data.Mappings;

public class StudentMap : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Student");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .ValueGeneratedOnAdd();
        
        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();
        
        builder.Property(x => x.UpdatedAt)
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAddOrUpdate();

        builder.Property(x => x.Deleted)
            .HasDefaultValueSql("false");

        builder
            .HasOne(p => p.User)
            .WithOne()
            .HasForeignKey<Professor>(p => p.UserId);
    }
}