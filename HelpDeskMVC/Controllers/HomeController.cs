using HelpDeskMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity;

namespace HelpDeskMVC.Controllers
{
    public class HomeController : Controller
    {
        private TicketsEntities db = new TicketsEntities();

        // GET: Tickets
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NumberSortParm = String.IsNullOrEmpty(sortOrder) ? "number" : "";
            ViewBag.CreatorSortParm = String.IsNullOrEmpty(sortOrder) ? "creator" : "";
            ViewBag.PrioritySortParm = String.IsNullOrEmpty(sortOrder) ? "priorityDesc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "dateDesc" : "Date";            

            if(searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var tickets = from t in db.Tickets
                          select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                tickets = tickets.Where(s => s.Creator.Contains(searchString)
                                       || s.TicketNumber.ToString().Contains(searchString)
                                       || s.Summary.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "number":
                    tickets = tickets.OrderBy(t => t.TicketNumber);
                    break;                    
                case "creator":
                    tickets = tickets.OrderBy(t => t.Creator);
                    break;
                case "priorityDesc":
                    tickets = tickets.OrderByDescending(t => t.TicketPriority);
                    break;
                case "dateDesc":
                    tickets = tickets.OrderBy(t => t.ClosedDate);
                    break;
                case "Date":
                    tickets = tickets.OrderByDescending(t => t.ClosedDate);
                    break;
                default:
                    tickets = tickets.OrderByDescending(t => t.TicketNumber);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(tickets.ToPagedList(pageNumber, pageSize));            
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
            "Creator,ClosedDate,TicketPriority,ClosingComments")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.TicketGuid = Guid.NewGuid();
                ticket.TicketNumber = db.Tickets.Max(t => t.TicketNumber) + 1;
                ticket.CreationDate = DateTime.Now;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                TempData["Message"] = "Ticket successfully created!";
                return RedirectToAction("Index");
            }
            return View(ticket);
        }

        // GET : Tickets/Close
        public ActionResult Close(Guid? id)
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
                
        // POST: Tickets/Close/        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Close([Bind(Include = "TicketGuid,TicketNumber,Summary,Description,CreationDate," +
            "Creator,ClosedDate,TicketPriority,ClosingComments")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.ClosedDate = DateTime.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "Ticket successfully closed!";
                return RedirectToAction("Index");
            }
            return View(ticket);
        }

        // POST: Tickets/Reopen
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reopen(Guid? id)
        {
            if (ModelState.IsValid)
            {
                Ticket ticket = db.Tickets.Find(id);
                ticket.ClosedDate = null;
                db.SaveChanges();
                TempData["Message"] = "Ticket successfully re-opened!";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}