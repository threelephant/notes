using Microsoft.EntityFrameworkCore;

namespace NotesApi
{
    public class NoteContext : DbContext
    {
        public NoteContext()
        {
        }
        
        public NoteContext(DbContextOptions<NoteContext> options)
            : base(options)
        {
        }
        
        public virtual DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Note>(entity =>
            {
                entity.HasIndex(e => e.id, "IX_Notes_Id");
                entity.Property(e => e.content)
                    .IsRequired();
            });
        }
    }
}