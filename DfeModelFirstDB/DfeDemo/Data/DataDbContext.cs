using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Models;

namespace DfeDemo.Data
{
	public partial class DataDbContext : DbContext
	{
		public virtual DbSet<Ladata> Ladata { get; set; }
		public virtual DbSet<User> User { get; set; }
		public virtual DbSet<UserCase> UserCase { get; set; }

		public DataDbContext(DbContextOptions<DataDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Ladata>(entity =>
			{
				entity.ToTable("LAData");

				entity.Property(e => e.Name).HasMaxLength(50);
			});

			modelBuilder.Entity<User>(entity =>
			{
				entity.Property(e => e.Id).ValueGeneratedNever();

				entity.Property(e => e.Email).HasMaxLength(80);

				entity.Property(e => e.Firstname).HasMaxLength(50);

				entity.Property(e => e.Surname).HasMaxLength(50);
			});

			modelBuilder.Entity<UserCase>(entity =>
			{
				entity.HasKey(e => e.CaseId);

				entity.Property(e => e.CaseId).HasColumnName("CaseID");

				entity.Property(e => e.DateOfBirth).HasColumnType("date");

				entity.Property(e => e.Firstname).HasMaxLength(50);

				entity.Property(e => e.Laid).HasColumnName("LAId");

				entity.Property(e => e.Surname).HasMaxLength(50);

				entity.Property(e => e.UserId)
					.HasColumnName("UserID")
					.HasMaxLength(450);
			});
		}
	}
}
