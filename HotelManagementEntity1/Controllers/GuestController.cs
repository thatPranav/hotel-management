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
    [Authorize]
    public class GuestController : Controller
    {
        private GuestRoomDBEntities1 db = new GuestRoomDBEntities1();

        // GET: /Guest/
        public ActionResult Index()
        {
            var guests = db.Guests.Include(g => g.Room);
            return View(guests.ToList());
        }

        // PGET: /Guest/Search
        public ActionResult Search()
        {
            return View();
        }

        // POST: /Guest/Search
        [HttpPost]
        public ActionResult Search(FormCollection collection)
        {
            /*  string contactTemp = collection["guestContact"];
              List<Guest> guest = db.Guests.ToList();
              List<Guest> guestSameName = new List<Guest>();
              string temp = " ";
              foreach (Guest g in guest)
              {
                  if (g.guestContact == contactTemp)
                  {
                      //temp = g.guestId.ToString();
                      //guestSameName.Add(g);
                  }
              }

              //ViewData["data"] = guestSameName;
              //string view = "/Guest/Details/"+temp+"";
              return View(); */
            string name = collection["Name"];
            return View("Index", db.Guests.Where(g => g.guestName.Contains(name)).ToList());
            //list.Where(x => x.myTextColumn.Contains('abc'));


        }

        // GET: /Guest/Details/5
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

        // GET: /Guest/Create
        public ActionResult Create()
        {
            ViewBag.roomId = new SelectList(db.Rooms, "roomId", "roomType");
            return View();
        }

        // POST: /Guest/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="guestId,guestName,guestContact,guestAddress,guestRoomType,guestQuantity,guestCheckinDate,guestNoOfDaysStay,roomId")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                List<Guest> gsts = db.Guests.ToList();
                List<Room> roomslst = db.Rooms.ToList();
                Room findRoomType = (Room)roomslst.First(i => i.roomType.Contains(guest.guestRoomType));
                guest.roomId = findRoomType.roomId;
                int roomAvail = (int)findRoomType.roomNumber;
                int newRoomAvail = 0;
                // string roomType = rr.roomType.Single(s => s == guest.guestRoomType)
                List<Room> rooms = db.Rooms.ToList();
                Room roomTypeNeeded = rooms.Single(s => s.roomType == guest.guestRoomType);
                guest.guestCost = guest.guestNoOfDaysStay * roomTypeNeeded.roomPrice * guest.guestQuantity;
                

                bool isEmpty = !gsts.Any();

                if (isEmpty)
                {
                    guest.guestId = 1;
                }
                else
                {
                    guest.guestId = gsts.Last().guestId + 1;
                }



                if (roomAvail >= guest.guestQuantity)
                {
                    newRoomAvail = (int)(roomAvail - guest.guestQuantity);
                    var query = "Update [Room] SET roomNumber = {0} WHERE roomId = {1}";
                    db.Database.ExecuteSqlCommand(query, newRoomAvail, guest.roomId);
                    db.Guests.Add(guest);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                else
                {
                    //return View("../Shared/Error");
                    return Content("<script language='javascript' type='text/javascript'>alert('Only " + roomAvail + " " + guest.guestRoomType + " rooms left');</script>");

                }
            }

            ViewBag.roomId = new SelectList(db.Rooms, "roomId", "roomType", guest.roomId);
            return View(guest);
        }

        // GET: /Guest/Edit/5
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

        // POST: /Guest/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="guestId,guestName,guestContact,guestAddress,guestRoomType,guestQuantity,guestCheckinDate,guestNoOfDaysStay,roomId")] Guest guest)
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

        // GET: /Guest/Delete/5
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

        // POST: /Guest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //  Room rr = new Room();
            Guest guest = db.Guests.Find(id);


            List<Room> rooms = db.Rooms.ToList();
            Room roomTypeNeeded = rooms.Single(s => s.roomType == guest.guestRoomType);
            if (guest.guestRoomType == roomTypeNeeded.roomType)
            {
                roomTypeNeeded.roomNumber += guest.guestQuantity;
            }

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
