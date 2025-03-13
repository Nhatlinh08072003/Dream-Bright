using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dream_Bridge.Models.Main;

public partial class StudyAbroadDbContext : DbContext
{
    public StudyAbroadDbContext()
    {
    }

    public StudyAbroadDbContext(DbContextOptions<StudyAbroadDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChatMessage> ChatMessages { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<ConsultingRegistration> ConsultingRegistrations { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<School> Schools { get; set; }

    public virtual DbSet<StudyAbroadCatalog> StudyAbroadCatalogs { get; set; }
    public DbSet<EmailHistory> EmailHistories { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public object ChatPermissions { get; internal set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

<<<<<<< HEAD
      => optionsBuilder.UseSqlServer("Server=LAPTOP-6MMRS9KB\\TAQUAN;Database=StudyAbroad;MultipleActiveResultSets=true;User ID=sa ;Password=asdasd;Trusted_Connection=True;TrustServerCertificate=Yes");
=======
      => optionsBuilder.UseSqlServer("Server=NHATLINH\\NHATLINH;Database=StudyAbroad;MultipleActiveResultSets=true;User ID=admin;Password=asdasd;Trusted_Connection=True;TrustServerCertificate=Yes");
>>>>>>> ecf659aa4f4abc2092fd071cb6f927aaa143752b
    //  => optionsBuilder.UseSqlServer("Server=tcp:uinlan.database.windows.net,1433;Initial Catalog=StudyAbroad;Persist Security Info=False;User ID=uinlan;Password=AnhthuongAnh@2;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notification>()
            .ToTable("Notifications");

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            
            entity.HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(e => e.IdMessage).HasName("PK__ChatMess__47AAF3043C702C38");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Receiver).WithMany(p => p.ChatMessageReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChatMessa__Recei__6754599E");

            entity.HasOne(d => d.Sender).WithMany(p => p.ChatMessageSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChatMessa__Sende__66603565");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.IdComment).HasName("PK__Comments__57C9AD582E41940E");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdNewsNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.IdNews)
                .HasConstraintName("FK__Comments__IdNews__628FA481");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__Comments__IdUser__619B8048");
        });

        modelBuilder.Entity<ConsultingRegistration>(entity =>
        {
            entity.HasKey(e => e.IdConsulting).HasName("PK__Consulti__67487AA91F357E45");

            entity.ToTable("Consulting_Registration");

            entity.Property(e => e.ConsultingType)
                .HasMaxLength(255)
                .HasDefaultValue("Study abroad consulting");
            entity.Property(e => e.ContentConsulting)
                .HasMaxLength(255)
                .HasDefaultValue("Study in USA");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("chua tu v?n");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.IdNews).HasName("PK__News__4559C72DAD5F8D18");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NewsImage).HasMaxLength(255);
            entity.Property(e => e.TitleNews).HasMaxLength(255);

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.News)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__News__IdUser__5CD6CB2B");
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.HasKey(e => e.IdSchool).HasName("PK__School__B544508521507AE2");

            entity.ToTable("School");

            entity.Property(e => e.AverageTuition)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Average_tuition");
            entity.Property(e => e.ImageSchool).HasMaxLength(255);
            entity.Property(e => e.Level).HasMaxLength(255);
            entity.Property(e => e.Nation).HasMaxLength(255);
            entity.Property(e => e.SchoolDescription).HasColumnName("School_description");
            entity.Property(e => e.StateCity)
                .HasMaxLength(255)
                .HasColumnName("State_City");
            entity.Property(e => e.TitleSchool).HasMaxLength(255);

            entity.HasOne(d => d.IdcategoryStabNavigation).WithMany(p => p.Schools)
                .HasForeignKey(d => d.IdcategoryStab)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__School__Idcatego__59063A47");
        });

        modelBuilder.Entity<StudyAbroadCatalog>(entity =>
        {
            entity.HasKey(e => e.IdcategoryStab).HasName("PK__Study_ab__2CD6C429BF218044");

            entity.ToTable("Study_abroad_catalog");

            entity.Property(e => e.NamecategoryStab).HasMaxLength(255);

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.StudyAbroadCatalogs)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Study_abr__IdUse__5629CD9C");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__User__B7C92638A1EA2A1D");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534A39E0728").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ConsultingStatus)
                .HasMaxLength(50)
                .HasDefaultValue("chua tu v?n");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasDefaultValue("user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
