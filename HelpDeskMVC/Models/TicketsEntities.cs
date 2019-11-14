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
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Ticket>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Ticket>()
                .Property(e => e.Creator)
                .IsFixedLength();
        }
    }
}
