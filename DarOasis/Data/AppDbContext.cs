using DarOasis.Models;
using Microsoft.EntityFrameworkCore;

namespace DarOasis.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employe> Employes { get; set; }
        public DbSet<Chambre> Chambres { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Affectation> Affectations { get; set; }
        public DbSet<ServiceSupplementaire> ServicesSupplementaires { get; set; }
        public DbSet<ReservationService> ReservationServices { get; set; }
        public DbSet<Facture> Factures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
          
            modelBuilder.Entity<Chambre>()
                .HasKey(c => c.IdChambre);
            modelBuilder.Entity<Reservation>()
                .HasKey(r => r.IdReservation);
            modelBuilder.Entity<Affectation>()
                .HasKey(a => a.IdAffectation);
            modelBuilder.Entity<Facture>()
                .HasKey(f => f.IdFacture);
            modelBuilder.Entity<ServiceSupplementaire>()
                .HasKey(s => s.IdService);

            // Les relations bin les models 
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Client)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Chambre)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.ChambreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Facture>()
                .HasOne(f => f.Reservation)
                .WithOne(r => r.Facture)
                .HasForeignKey<Facture>(f => f.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Affectation>()
                .HasOne(a => a.Employe)
                .WithMany(e => e.Affectations)
                .HasForeignKey(a => a.EmployeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Affectation>()
                .HasOne(a => a.Chambre)
                .WithMany(c => c.Affectations)
                .HasForeignKey(a => a.ChambreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReservationService>()
                .HasOne(rs => rs.Reservation)
                .WithMany(r => r.ReservationServices)
                .HasForeignKey(rs => rs.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReservationService>()
                .HasOne(rs => rs.ServiceSupplementaire)
                .WithMany(s => s.ReservationServices)
                .HasForeignKey(rs => rs.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // fil les factures w ay partie fiha flous nhb 2 chiffres baad el virgules 
            modelBuilder.Entity<Chambre>()
                .Property(c => c.PrixParNuit).HasPrecision(10, 2);
            modelBuilder.Entity<Reservation>()
                .Property(r => r.PrixTotal).HasPrecision(10, 2);
            modelBuilder.Entity<Facture>()
                .Property(f => f.MontantSejour).HasPrecision(10, 2);
            modelBuilder.Entity<Facture>()
                .Property(f => f.MontantServices).HasPrecision(10, 2);
            modelBuilder.Entity<Facture>()
                .Property(f => f.MontantTotal).HasPrecision(10, 2);
            modelBuilder.Entity<ServiceSupplementaire>()
                .Property(s => s.PrixService).HasPrecision(10, 2);
        }
    }
}