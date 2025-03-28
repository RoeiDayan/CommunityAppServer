using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models;

public partial class CommunityDBContext : DbContext
{
    public CommunityDBContext()
    {
    }

    public CommunityDBContext(DbContextOptions<CommunityDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Community> Communities { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Notice> Notices { get; set; }

    public virtual DbSet<NoticeFile> NoticeFiles { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Priority> Priorities { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<ReportFile> ReportFiles { get; set; }

    public virtual DbSet<RoomRequest> RoomRequests { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<TenantRoom> TenantRooms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=CommunityDB;User ID=AdminLogin;Password=ComPass;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC27B30A5181");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Community>(entity =>
        {
            entity.HasKey(e => e.ComId).HasName("PK__Communit__E15F41126E6B41B9");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => new { e.ComId, e.UserId }).HasName("PK__Members__3027CDD6CE67D7B7");

            entity.Property(e => e.Balance).HasDefaultValue(0);
            entity.Property(e => e.IsApproved).HasDefaultValue(false);
            entity.Property(e => e.IsLiable).HasDefaultValue(false);
            entity.Property(e => e.IsManager).HasDefaultValue(false);
            entity.Property(e => e.IsProvider).HasDefaultValue(false);
            entity.Property(e => e.IsResident).HasDefaultValue(false);
            entity.Property(e => e.UnitNum).HasDefaultValue(0);

            entity.HasOne(d => d.Com).WithMany(p => p.Members).HasConstraintName("FK__Members__ComId__33D4B598");

            entity.HasOne(d => d.User).WithMany(p => p.Members).HasConstraintName("FK__Members__UserId__32E0915F");
        });

        modelBuilder.Entity<Notice>(entity =>
        {
            entity.HasKey(e => e.NoticeId).HasName("PK__Notices__CE83CBE520812205");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.Notices).HasConstraintName("FK__Notices__UserId__440B1D61");

            entity.HasMany(d => d.Coms).WithMany(p => p.Notices)
                .UsingEntity<Dictionary<string, object>>(
                    "CommunityNotice",
                    r => r.HasOne<Community>().WithMany()
                        .HasForeignKey("ComId")
                        .HasConstraintName("FK__Community__ComId__47DBAE45"),
                    l => l.HasOne<Notice>().WithMany()
                        .HasForeignKey("NoticeId")
                        .HasConstraintName("FK__Community__Notic__46E78A0C"),
                    j =>
                    {
                        j.HasKey("NoticeId", "ComId").HasName("PK__Communit__F0963FF4C02C5ACA");
                        j.ToTable("CommunityNotices");
                    });
        });

        modelBuilder.Entity<NoticeFile>(entity =>
        {
            entity.HasKey(e => new { e.NoticeId, e.FileName }).HasName("PK__NoticeFi__0B0A2D0BB0B45681");

            entity.HasOne(d => d.Notice).WithMany(p => p.NoticeFiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NoticeFil__Notic__5CD6CB2B");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A38A739D4F7");

            entity.Property(e => e.MarkedPayed).HasDefaultValue(false);
            entity.Property(e => e.WasPayed).HasDefaultValue(false);

            entity.HasOne(d => d.Com).WithMany(p => p.Payments).HasConstraintName("FK__Payments__ComId__4CA06362");

            entity.HasOne(d => d.User).WithMany(p => p.Payments).HasConstraintName("FK__Payments__UserId__4D94879B");
        });

        modelBuilder.Entity<Priority>(entity =>
        {
            entity.HasKey(e => e.PriorityNum).HasName("PK__Priority__B8C9D75AAC721F86");

            entity.Property(e => e.PriorityNum).ValueGeneratedNever();
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Report__D5BD4805A724F214");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsAnon).HasDefaultValue(false);
            entity.Property(e => e.Title).HasDefaultValue("");

            entity.HasOne(d => d.Com).WithMany(p => p.Reports).HasConstraintName("FK__Report__ComId__3E52440B");

            entity.HasOne(d => d.PriorityNavigation).WithMany(p => p.Reports).HasConstraintName("FK__Report__Priority__3F466844");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Reports).HasConstraintName("FK__Report__Status__403A8C7D");

            entity.HasOne(d => d.User).WithMany(p => p.Reports).HasConstraintName("FK__Report__UserId__3D5E1FD2");
        });

        modelBuilder.Entity<ReportFile>(entity =>
        {
            entity.HasKey(e => new { e.ReportId, e.FileName }).HasName("PK__ReportFi__1034AEEB9113BD14");

            entity.HasOne(d => d.Report).WithMany(p => p.ReportFiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReportFil__Repor__59FA5E80");
        });

        modelBuilder.Entity<RoomRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__RoomRequ__33A8517A0099CE50");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsApproved).HasDefaultValue(false);

            entity.HasOne(d => d.Com).WithMany(p => p.RoomRequests).HasConstraintName("FK__RoomReque__ComId__534D60F1");

            entity.HasOne(d => d.User).WithMany(p => p.RoomRequests).HasConstraintName("FK__RoomReque__UserI__52593CB8");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatNum).HasName("PK__Status__C5F8EFB633EF57C6");

            entity.Property(e => e.StatNum).ValueGeneratedNever();
        });

        modelBuilder.Entity<TenantRoom>(entity =>
        {
            entity.HasKey(e => new { e.ComId, e.KeyHolderId }).HasName("PK__TenantRo__18F049697A85E327");

            entity.HasOne(d => d.Com).WithMany(p => p.TenantRooms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TenantRoo__ComId__5629CD9C");

            entity.HasOne(d => d.KeyHolder).WithMany(p => p.TenantRooms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TenantRoo__KeyHo__571DF1D5");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
