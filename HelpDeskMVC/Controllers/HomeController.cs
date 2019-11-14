using HelpDeskMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelpDeskMVC.Controllers
{
    public class HomeController : Controller
    {

        private TicketsEntities db = new TicketsEntities();

        // GET: Tickets
        public ActionResult Index()
        {
            return View(db.Tickets.ToList());
        }

        public ActionResult Details()
        {   
            return View();
        }


        // GET: Tickets/Create
        public ActionResult Create()
        {            
            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketGuid,TicketNumber,Title,Description,CreationDate," +
            "Creator,Closed,TicketPriority")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.TicketGuid = Guid.NewGuid();
                ticket.TicketNumber = db.Tickets.Max(r => r.TicketNumber) + 1;
                ticket.CreationDate = DateTime.Now;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ticket);
        }
    }
}