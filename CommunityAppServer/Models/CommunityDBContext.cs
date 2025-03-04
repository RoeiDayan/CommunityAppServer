﻿using System;
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

    public virtual DbSet<Type> Types { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=CommunityDB;User ID=AdminLogin;Password=ComPass;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC2703EF508D");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Community>(entity =>
        {
            entity.HasKey(e => e.ComId).HasName("PK__Communit__E15F4112CC336EA8");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => new { e.ComId, e.UserId }).HasName("PK__Members__3027CDD67B249FC5");

            entity.HasOne(d => d.Com).WithMany(p => p.Members)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Members__ComId__2C3393D0");

            entity.HasOne(d => d.User).WithMany(p => p.Members)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Members__UserId__2B3F6F97");
        });

        modelBuilder.Entity<Notice>(entity =>
        {
            entity.HasKey(e => e.NoticeId).HasName("PK__Notices__CE83CBE57E3E06D4");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.Notices).HasConstraintName("FK__Notices__UserId__403A8C7D");

            entity.HasMany(d => d.Coms).WithMany(p => p.Notices)
                .UsingEntity<Dictionary<string, object>>(
                    "CommunityNotice",
                    r => r.HasOne<Community>().WithMany()
                        .HasForeignKey("ComId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Community__ComId__440B1D61"),
                    l => l.HasOne<Notice>().WithMany()
                        .HasForeignKey("NoticeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Community__Notic__4316F928"),
                    j =>
                    {
                        j.HasKey("NoticeId", "ComId").HasName("PK__Communit__F0963FF473AE22DD");
                        j.ToTable("CommunityNotices");
                    });
        });

        modelBuilder.Entity<NoticeFile>(entity =>
        {
            entity.HasKey(e => new { e.NoticeId, e.FileName }).HasName("PK__NoticeFi__0B0A2D0B84BFCB22");

            entity.HasOne(d => d.Notice).WithMany(p => p.NoticeFiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NoticeFil__Notic__4AB81AF0");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A38D1647448");

            entity.HasOne(d => d.Com).WithMany(p => p.Payments).HasConstraintName("FK__Payments__ComId__4D94879B");

            entity.HasOne(d => d.User).WithMany(p => p.Payments).HasConstraintName("FK__Payments__UserId__4E88ABD4");
        });

        modelBuilder.Entity<Priority>(entity =>
        {
            entity.HasKey(e => e.PriorityNum).HasName("PK__Priority__B8C9D75ABBF1BEB1");

            entity.Property(e => e.PriorityNum).ValueGeneratedNever();
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Report__D5BD480575D29E00");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Com).WithMany(p => p.ReportsNavigation).HasConstraintName("FK__Report__ComId__36B12243");

            entity.HasOne(d => d.PriorityNavigation).WithMany(p => p.Reports).HasConstraintName("FK__Report__Priority__37A5467C");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Reports).HasConstraintName("FK__Report__Status__398D8EEE");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Reports).HasConstraintName("FK__Report__Type__38996AB5");

            entity.HasOne(d => d.User).WithMany(p => p.Reports).HasConstraintName("FK__Report__UserId__35BCFE0A");

            entity.HasMany(d => d.Coms).WithMany(p => p.Reports)
                .UsingEntity<Dictionary<string, object>>(
                    "CommunityReport",
                    r => r.HasOne<Community>().WithMany()
                        .HasForeignKey("ComId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Community__ComId__47DBAE45"),
                    l => l.HasOne<Report>().WithMany()
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Community__Repor__46E78A0C"),
                    j =>
                    {
                        j.HasKey("ReportId", "ComId").HasName("PK__Communit__EBA8BC14309A2B24");
                        j.ToTable("CommunityReports");
                    });
        });

        modelBuilder.Entity<ReportFile>(entity =>
        {
            entity.HasKey(e => new { e.ReportId, e.FileName }).HasName("PK__ReportFi__1034AEEBAB434EBB");

            entity.HasOne(d => d.Report).WithMany(p => p.ReportFiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReportFil__Repor__3C69FB99");
        });

        modelBuilder.Entity<RoomRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__RoomRequ__33A8517A1D406933");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Com).WithMany(p => p.RoomRequests).HasConstraintName("FK__RoomReque__ComId__571DF1D5");

            entity.HasOne(d => d.User).WithMany(p => p.RoomRequests).HasConstraintName("FK__RoomReque__UserI__5629CD9C");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatNum).HasName("PK__Status__C5F8EFB611007B21");

            entity.Property(e => e.StatNum).ValueGeneratedNever();
        });

        modelBuilder.Entity<TenantRoom>(entity =>
        {
            entity.HasKey(e => new { e.ComId, e.KeyHolderId }).HasName("PK__TenantRo__18F049695880825B");

            entity.HasOne(d => d.Com).WithMany(p => p.TenantRooms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TenantRoo__ComId__5165187F");

            entity.HasOne(d => d.KeyHolder).WithMany(p => p.TenantRooms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TenantRoo__KeyHo__52593CB8");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.TypeNum).HasName("PK__Types__E237D70846BA852A");

            entity.Property(e => e.TypeNum).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
