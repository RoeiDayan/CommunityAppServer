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
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC2746B07617");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Community>(entity =>
        {
            entity.HasKey(e => e.ComId).HasName("PK__Communit__E15F4112E6203F77");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.ExpenseId).HasName("PK__Expenses__1445CFD3B70C75A5");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Com).WithMany(p => p.Expenses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Expenses__ComId__47DBAE45");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => new { e.ComId, e.UserId }).HasName("PK__Members__3027CDD6231D9D93");

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
            entity.HasKey(e => e.NoticeId).HasName("PK__Notices__CE83CBE5B6DB15C9");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Com).WithMany(p => p.Notices).HasConstraintName("FK__Notices__ComId__3E52440B");

            entity.HasOne(d => d.User).WithMany(p => p.Notices).HasConstraintName("FK__Notices__UserId__3D5E1FD2");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A3802BE5A08");

            entity.Property(e => e.MarkedPayed).HasDefaultValue(false);
            entity.Property(e => e.WasPayed).HasDefaultValue(false);

            entity.HasOne(d => d.Com).WithMany(p => p.Payments).HasConstraintName("FK__Payments__ComId__4316F928");

            entity.HasOne(d => d.User).WithMany(p => p.Payments).HasConstraintName("FK__Payments__UserId__440B1D61");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Report__D5BD480586A6482C");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Title).HasDefaultValue("");

            entity.HasOne(d => d.Com).WithMany(p => p.Reports).HasConstraintName("FK__Report__ComId__398D8EEE");

            entity.HasOne(d => d.User).WithMany(p => p.Reports).HasConstraintName("FK__Report__UserId__38996AB5");
        });

        modelBuilder.Entity<RoomRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__RoomRequ__33A8517A830222BA");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsApproved).HasDefaultValue(false);

            entity.HasOne(d => d.Com).WithMany(p => p.RoomRequests).HasConstraintName("FK__RoomReque__ComId__4D94879B");

            entity.HasOne(d => d.User).WithMany(p => p.RoomRequests).HasConstraintName("FK__RoomReque__UserI__4CA06362");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
