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

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Notice> Notices { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<RoomRequest> RoomRequests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=CommunityDB;User ID=AdminLogin;Password=ComPass;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC27C548B4D9");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Community>(entity =>
        {
            entity.HasKey(e => e.ComId).HasName("PK__Communit__E15F4112333618F5");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.ExpenseId).HasName("PK__Expenses__1445CFD325D28DB4");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Com).WithMany(p => p.Expenses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Expenses__ComId__46E78A0C");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => new { e.ComId, e.UserId }).HasName("PK__Members__3027CDD673F01B76");

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
            entity.HasKey(e => e.NoticeId).HasName("PK__Notices__CE83CBE55FFD9DC1");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Com).WithMany(p => p.Notices).HasConstraintName("FK__Notices__ComId__3E52440B");

            entity.HasOne(d => d.User).WithMany(p => p.Notices).HasConstraintName("FK__Notices__UserId__3D5E1FD2");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A38A12CA68B");

            entity.Property(e => e.WasPayed).HasDefaultValue(false);

            entity.HasOne(d => d.Com).WithMany(p => p.Payments).HasConstraintName("FK__Payments__ComId__4222D4EF");

            entity.HasOne(d => d.User).WithMany(p => p.Payments).HasConstraintName("FK__Payments__UserId__4316F928");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Report__D5BD4805FF34E1BF");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Title).HasDefaultValue("");

            entity.HasOne(d => d.Com).WithMany(p => p.Reports).HasConstraintName("FK__Report__ComId__398D8EEE");

            entity.HasOne(d => d.User).WithMany(p => p.Reports).HasConstraintName("FK__Report__UserId__38996AB5");
        });

        modelBuilder.Entity<RoomRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__RoomRequ__33A8517AF3444343");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsApproved).HasDefaultValue(false);

            entity.HasOne(d => d.Com).WithMany(p => p.RoomRequests).HasConstraintName("FK__RoomReque__ComId__4CA06362");

            entity.HasOne(d => d.User).WithMany(p => p.RoomRequests).HasConstraintName("FK__RoomReque__UserI__4BAC3F29");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
