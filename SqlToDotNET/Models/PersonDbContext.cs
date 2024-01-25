using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SqlToDotNET.Models;

public partial class PersonDbContext : DbContext
{
    public PersonDbContext()
    {
    }

    public PersonDbContext(DbContextOptions<PersonDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<PersonAudit> PersonAudits { get; set; }

    public virtual DbSet<TblBackup> TblBackups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=Person_Db;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PK__Person__AA2FFBE52F3422CE");

            entity.ToTable("Person", tb =>
                {
                    tb.HasTrigger("tr_Person_ForDelete");
                    tb.HasTrigger("tr_Person_ForInsert");
                    tb.HasTrigger("tr_player_ForUpdate");
                    tb.HasTrigger("trg_AfterDelete");
                });

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PersonAudit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PersonAu__3214EC07FCA98910");

            entity.ToTable("PersonAudit");

            entity.Property(e => e.AuditData).HasMaxLength(1000);
        });

        modelBuilder.Entity<TblBackup>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PK__tblBacku__AA2FFBE58BCCBA32");

            entity.ToTable("tblBackup");

            entity.Property(e => e.PersonId).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
