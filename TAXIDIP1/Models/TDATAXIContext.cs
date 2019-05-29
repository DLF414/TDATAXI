using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TAXIDIP1.Models
{
    public partial class TDATAXIContext : DbContext
    {
        public TDATAXIContext()
        {
        }

        public TDATAXIContext(DbContextOptions<TDATAXIContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<Admins> Admins { get; set; }
        public virtual DbSet<Cars> Cars { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<Companies> Companies { get; set; }
        public virtual DbSet<Drivers> Drivers { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }
        public virtual DbSet<Managers> Managers { get; set; }
        public virtual DbSet<Rides> Rides { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=TDATAXI;Username=DLF414;Password=123456Qw");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<Accounts>(entity =>
            {
                entity.ToTable("accounts");

                entity.HasIndex(e => e.Login)
                    .HasName("uni_acc_login")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login")
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(140);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasColumnName("role")
                    .HasMaxLength(20);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<Admins>(entity =>
            {
                entity.ToTable("admins");

                entity.HasIndex(e => e.AccountId)
                    .HasName("admins_account_id_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(30);

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(30);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(30);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Account)
                    .WithOne(p => p.Admins)
                    .HasForeignKey<Admins>(d => d.AccountId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_admin_acc_id");
            });

            modelBuilder.Entity<Cars>(entity =>
            {
                entity.ToTable("cars");

                entity.HasIndex(e => e.CarPlate)
                    .HasName("cars_car_plate_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Car)
                    .IsRequired()
                    .HasColumnName("car")
                    .HasColumnType("character varying");

                entity.Property(e => e.CarColor)
                    .HasColumnName("car_color")
                    .HasColumnType("character varying");

                entity.Property(e => e.CarPlate)
                    .IsRequired()
                    .HasColumnName("car_plate")
                    .HasColumnType("character varying");

                entity.Property(e => e.ClaimedBy).HasColumnName("claimed_by");

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.ClaimedByNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.ClaimedBy)
                    .HasConstraintName("FK_claimed_by");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_company_id");
            });

            modelBuilder.Entity<Clients>(entity =>
            {
                entity.ToTable("clients");

                entity.HasIndex(e => e.AccountId)
                    .HasName("clients_account_id_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(30);

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(30);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(30);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Account)
                    .WithOne(p => p.Clients)
                    .HasForeignKey<Clients>(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_client_acc_id");
            });

            modelBuilder.Entity<Companies>(entity =>
            {
                entity.ToTable("companies");

                entity.HasIndex(e => e.Name)
                    .HasName("companies_name_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.LegalAddress)
                    .HasColumnName("legal_address")
                    .HasMaxLength(50);

                entity.Property(e => e.LegalData).HasColumnName("legal_data");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(30);

                entity.Property(e => e.UpdaedAt)
                    .HasColumnName("updaed_at")
                    .HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<Drivers>(entity =>
            {
                entity.ToTable("drivers");

                entity.HasIndex(e => e.AccountId)
                    .HasName("drivers_account_id_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(50);

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(30);

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(30);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(30);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Account)
                    .WithOne(p => p.Drivers)
                    .HasForeignKey<Drivers>(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_driver_acc_id");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Drivers)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_driver_company_id");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("feedback");

                entity.HasIndex(e => e.RideId)
                    .HasName("feedback_ride_id_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasColumnName("note");

                entity.Property(e => e.RideId).HasColumnName("ride_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Ride)
                    .WithOne(p => p.Feedback)
                    .HasForeignKey<Feedback>(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_feedback_ride_id");
            });

            modelBuilder.Entity<Managers>(entity =>
            {
                entity.ToTable("managers");

                entity.HasIndex(e => e.AccountId)
                    .HasName("managers_account_id_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(30);

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(30);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(30);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Account)
                    .WithOne(p => p.Managers)
                    .HasForeignKey<Managers>(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_manager_acc_id");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Managers)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_manager_company_id");
            });

            modelBuilder.Entity<Rides>(entity =>
            {
                entity.ToTable("rides");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AcceptedAt)
                    .HasColumnName("accepted_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.AddressCurrent).HasColumnName("address_current");

                entity.Property(e => e.AddressEnd).HasColumnName("address_end").HasMaxLength(50); 

                entity.Property(e => e.AddressStart).HasColumnName("address_start").HasMaxLength(50); 

                entity.Property(e => e.ClientId).HasColumnName("client_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.Distance).HasColumnName("distance");

                entity.Property(e => e.DriverId).HasColumnName("driver_id");

                entity.Property(e => e.IsAccepted)
                    .HasColumnName("is_accepted")
                    .HasColumnType("boolean");

                entity.Property(e => e.IsCanceled).HasColumnName("is_canceled");

                entity.Property(e => e.IsComplained).HasColumnName("is_complained");

                entity.Property(e => e.Path).HasColumnName("path");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("money");

                entity.Property(e => e.Rate).HasColumnName("rate");

                entity.Property(e => e.StartedAt)
                    .HasColumnName("started_at")
                    .HasColumnType("timestamp with time zone");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp with time zone");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Rides)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ride_client_id");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.Rides)
                    .HasForeignKey(d => d.DriverId)
                    .HasConstraintName("FK_ride_driver_id");
            });

            modelBuilder.HasSequence("accounts_id_seq");

            modelBuilder.HasSequence("admins_id_seq");

            modelBuilder.HasSequence("cars_id_seq");

            modelBuilder.HasSequence("clients_id_seq");

            modelBuilder.HasSequence("companies_id_seq");

            modelBuilder.HasSequence("drivers_id_seq");

            modelBuilder.HasSequence("feedback_id_seq");

            modelBuilder.HasSequence("managers_id_seq");

            modelBuilder.HasSequence("rides_id_seq");
        }
    }
}
