namespace HelpDeskMVC.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TicketsEntities : DbContext
    {
        public TicketsEntities()
            : base("name=TicketsEntities")
        {
        }

        public virtual DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .Property(e => e.Summary)
                .IsUnicode(false);

            modelBuilder.Entity<Ticket>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Ticket>()
                .Property(e => e.Creator)
                .IsFixedLength();

            modelBuilder.Entity<Ticket>()
                .Property(e => e.ClosingComments)
                .IsUnicode(false);
        }
    }
}
