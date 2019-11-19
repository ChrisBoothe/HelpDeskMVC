using HelpDeskMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            return View(db.Tickets.OrderByDescending(x => x.TicketNumber).ToList());
        }

        // GET: Tickets/Details
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }


        // GET: Tickets/Create
        public ActionResult Create()
        {            
            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketGuid,TicketNumber,Summary,Description,CreationDate," +
            "Creator,ClosedDate,TicketPriority")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.TicketGuid = Guid.NewGuid();
                ticket.TicketNumber = db.Tickets.Max(t => t.TicketNumber) + 1;
                ticket.CreationDate = DateTime.Now;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ticket);
        }

        //POST: Tickets/Close
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Close(Guid? id)
        {
            Ticket ticket = db.Tickets.Find(id);

            if (ticket.ClosedDate == null)
            {                
                ticket.ClosedDate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            else
            {
                TempData["msg"] = "<script>alert('This ticket has already been closed.');</script>";
                return RedirectToAction("Index");
            }
        }
    }
}