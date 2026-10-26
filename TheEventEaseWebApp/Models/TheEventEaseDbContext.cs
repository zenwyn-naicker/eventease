using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TheEventEaseWebApp.Models
{
    public partial class TheEventEaseDbContext : DbContext
    {
        public TheEventEaseDbContext()
        {
        }

        public TheEventEaseDbContext(DbContextOptions<TheEventEaseDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Venue> Venues { get; set; }
        public virtual DbSet<EventType> EventTypes { get; set; }

        // Add DbSet for the view
        public virtual DbSet<BookingViewModel> BookingDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new InvalidOperationException("Set ConnectionStrings__DefaultConnection or supply configured DbContextOptions.");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.BookingId).HasName("PK__Booking__73951AED3BAF877B");

                entity.ToTable("Booking");

                entity.HasIndex(e => new { e.VenueId, e.BookingDate }, "UQ_Venue_Booking").IsUnique();

                entity.Property(e => e.BookingDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Event).WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Booking_Event");

                entity.HasOne(d => d.Venue).WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.VenueId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Booking_Venue");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.EventId).HasName("PK__Event__7944C81040C683E1");

                entity.ToTable("Event");

                entity.Property(e => e.Description).HasColumnType("text");
                entity.Property(e => e.EventDate).HasColumnType("datetime");
                entity.Property(e => e.EventName)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                // Add EventType relationship
                entity.HasOne(d => d.EventType).WithMany(p => p.Events)
                    .HasForeignKey(d => d.EventTypeId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Event_EventType");
            });

            modelBuilder.Entity<Venue>(entity =>
            {
                entity.HasKey(e => e.VenueId).HasName("PK__Venue__3C57E5F2CB66528C");

                entity.ToTable("Venue");

                entity.HasIndex(e => e.VenueName, "UQ__Venue__A40F8D12C902A455").IsUnique();

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.Location)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.VenueName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Availability)
                    .HasDefaultValue(true);
            });

            // Add EventType configuration
            modelBuilder.Entity<EventType>(entity =>
            {
                entity.HasKey(e => e.EventTypeId).HasName("PK__EventType__EventTypeId");

                entity.ToTable("EventType");

                entity.HasIndex(e => e.Name, "UQ__EventType__Name").IsUnique();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TypeName");  // Maps to TypeName column in database
            });

            // Map the view with updated properties
            modelBuilder.Entity<BookingViewModel>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vw_BookingDetails");

                entity.Property(e => e.BookingId);
                entity.Property(e => e.BookingDate);
                entity.Property(e => e.EventId);
                entity.Property(e => e.EventName);
                entity.Property(e => e.EventDate);
                entity.Property(e => e.EventDescription);
                entity.Property(e => e.EventImageUrl);
                entity.Property(e => e.EventTypeId);
                entity.Property(e => e.EventTypeName);
                entity.Property(e => e.VenueId);
                entity.Property(e => e.VenueName);
                entity.Property(e => e.Location);
                entity.Property(e => e.Capacity);
                entity.Property(e => e.VenueImageUrl);
                entity.Property(e => e.VenueAvailability);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}