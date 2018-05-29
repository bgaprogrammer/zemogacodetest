using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JR.FinancialManager.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JR.FinancialManager.Core.Data
{
    public class FinancialManagerDbContext : IdentityDbContext<ApplicationUser>
    {
        public FinancialManagerDbContext(DbContextOptions<FinancialManagerDbContext> options) : base(options)
        { }

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<TransactionDetail> TransactionDetail { get; set; }
        public virtual DbSet<TransactionType> TransactionType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .HasName("UX_Customer_name")
                    .IsUnique();

                entity.HasIndex(e => e.Name);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(11)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasIndex(e => e.Step);

                entity.HasIndex(e => new { e.OriginCustomerId, e.DestCustomerId, e.ExecutionDate })
                    .HasName("IX_Transaction_origin_dest_date");

                entity.HasIndex(e => new { e.OriginCustomerId, e.DestCustomerId, e.TransactionTypeId })
                    .HasName("IX_Transaction_origin_dest_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.DestCustomerId).HasColumnName("dest_customer_id");

                entity.Property(e => e.ExecutionDate)
                    .HasColumnName("execution_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.OriginCustomerId).HasColumnName("origin_customer_id");

                entity.Property(e => e.Step).HasColumnName("step");

                entity.Property(e => e.TransactionTypeId).HasColumnName("transaction_type_id");

                entity.HasOne(d => d.DestCustomer)
                    .WithMany(p => p.TransactionDestCustomer)
                    .HasForeignKey(d => d.DestCustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_dest_customer");

                entity.HasOne(d => d.OriginCustomer)
                    .WithMany(p => p.TransactionOriginCustomer)
                    .HasForeignKey(d => d.OriginCustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_origin_customer");

                entity.HasOne(d => d.TransactionType)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.TransactionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Transaction_Type");
            });

            modelBuilder.Entity<TransactionDetail>(entity =>
            {
                entity.ToTable("Transaction_Detail");

                entity.HasIndex(e => new { e.IsFraud, e.IsFlaggedFraud })
                    .HasName("IX_Transaction_Detail_fraud_flaggedfraud");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.IsFlaggedFraud).HasColumnName("is_flagged_fraud");

                entity.Property(e => e.IsFraud).HasColumnName("is_fraud");

                entity.Property(e => e.NewBalanceDest).HasColumnName("new_balance_dest");

                entity.Property(e => e.NewBalanceOrig).HasColumnName("new_balance_orig");

                entity.Property(e => e.OldBalanceDest).HasColumnName("old_balance_dest");

                entity.Property(e => e.OldBalanceOrig).HasColumnName("old_balance_orig");

                entity.HasOne(d => d.Transaction)
                    .WithOne(p => p.TransactionDetail)
                    .HasForeignKey<TransactionDetail>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Detail_Transaction");
            });

            modelBuilder.Entity<TransactionType>(entity =>
            {
                entity.ToTable("Transaction_Type");

                entity.HasIndex(e => e.Name)
                    .HasName("UX_Transaction_Type_name");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(8)
                    .IsUnicode(false);
            });

            //Added to prevent delete on cascade
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);
        }
    }
}