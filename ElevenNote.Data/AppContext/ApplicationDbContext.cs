using Microsoft.EntityFrameworkCore;

namespace ElevenNote.Data.AppContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<NoteEntity> Notes { get; set; }
        public DbSet<CategoryEntity> Categories {get; set; }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);
        //     modelBuilder.Entity<NoteEntity>()
        //                 .HasOne(n=>n.Owner)
        //                 .WithMany(p=>p.Notes)
        //                 .HasForeignKey(n=>n.OwnerId);
        // }
    }
}