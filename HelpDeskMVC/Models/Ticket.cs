namespace HelpDeskMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ticket
    {
        [Key]
        public Guid TicketGuid { get; set; }

        [Display(Name = "#")]
        public int TicketNumber { get; set; }

        [Required(ErrorMessage = "A summary is required.")]
        [StringLength(50)]
        public string Summary { get; set; }

        [Required(ErrorMessage = "A description is required.")]
        [StringLength(1000)]
        public string Description { get; set; }

        [Display(Name = "Created")]
        public DateTime CreationDate { get; set; }

        [Required]
        [StringLength(25)]
        public string Creator { get; set; }

        [Display(Name = "Closed")]
        public Nullable<System.DateTime> ClosedDate { get; set; }

        [Display(Name = "Priority")]
        [Required(ErrorMessage = "A priority status is required.")]
        public TicketPriority TicketPriority { get; set; }

        [Display(Name ="Closing Comments")]
        [Required(ErrorMessage = "Closing comments are required to close this ticket.")]
        [StringLength(1000)]
        public string ClosingComments { get; set; }
    }
}
