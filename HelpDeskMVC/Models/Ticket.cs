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

        [Required(ErrorMessage = "A title is required.")]
        [StringLength(50)]
        public string Title { get; set; }

        [Required(ErrorMessage = "A description is required.")]
        [StringLength(1000)]
        public string Description { get; set; }

        [Display(Name = "Created")]
        public DateTime CreationDate { get; set; }

        [Required]
        [StringLength(10)]
        public string Creator { get; set; }

        public bool Closed { get; set; }


        [Display(Name = "Priority")]
        [Required (ErrorMessage = "A priority status is required.")]
        public TicketPriority TicketPriority { get; set; }
    }
}