using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelManagementEntity1.Models;

namespace HotelManagementEntity1.Controllers
{
    public class Guest1Controller : Controller
    {
        private GuestRoomDBEntities1 db = new GuestRoomDBEntities1();

        // GET: /Guest1/
        public ActionResult Index()
        {
            var guests = db.Guests.Include(g => g.Room);
            return View(guests.ToList());
        }

        // GET: /Guest1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = db.Guests.Find(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            return View(guest);
        }

        // GET: /Guest1/Create
        public ActionResult Create()
        {
            ViewBag.roomId = new SelectList(db.Rooms, "roomId", "roomType");
            return View();
        }

        // POST: /Guest1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="guestId,guestName,guestContact,guestAddress,guestRoomType,guestQuantity,guestCheckinDate,guestNoOfDaysStay,roomId,guestCost")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                db.Guests.Add(guest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.roomId = new SelectList(db.Rooms, "roomId", "roomType", guest.roomId);
            return View(guest);
        }

        // GET: /Guest1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = db.Guests.Find(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            ViewBag.roomId = new SelectList(db.Rooms, "roomId", "roomType", guest.roomId);
            return View(guest);
        }

        // POST: /Guest1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="guestId,guestName,guestContact,guestAddress,guestRoomType,guestQuantity,guestCheckinDate,guestNoOfDaysStay,roomId,guestCost")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.roomId = new SelectList(db.Rooms, "roomId", "roomType", guest.roomId);
            return View(guest);
        }

        // GET: /Guest1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = db.Guests.Find(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            return View(guest);
        }

        // POST: /Guest1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Guest guest = db.Guests.Find(id);
            db.Guests.Remove(guest);
            db.SaveChanges();
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
