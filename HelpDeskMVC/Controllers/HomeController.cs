using HelpDeskMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;

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
                                       || s.TicketNumber.ToString().Contains(searchString));
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