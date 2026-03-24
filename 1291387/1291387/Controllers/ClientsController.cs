using _1291387.Models;
using _1291387.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _1291387.Controllers
{
    public class ClientsController : Controller
    {
        MSDbContext db = new MSDbContext();
        // GET: Clients
        public ActionResult Index()
        {
            var clnts = db.clients.Include(x => x.orders.Select(b => b.product)).OrderByDescending(x => x.clientId).ToList();
            return View(clnts);
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult AddNewProduct(int? id)
        {
            ViewBag.products = new SelectList(db.products.ToList(), "productId", "productName", (id != null) ? id.ToString() : "");
            return PartialView("_addNewProduct");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientVM clientVM, int[] productId)
        {
            if (ModelState.IsValid)
            {
                client client = new client()
                {
                    clientName = clientVM.clientName,
                    birthDate = clientVM.birthDate,
                    age = clientVM.age,
                    insideDhaka = clientVM.insideDhaka
                };

                //Image
                HttpPostedFileBase file = clientVM.pictureFile;
                if (file != null)
                {
                    string filePath = Path.Combine("/Images/", DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(filePath));
                    client.picture = filePath;
                }


                foreach (var item in productId)
                {
                    order order = new order()
                    {
                        client = client,
                        clientId = client.clientId,
                        productId = item
                    };
                    db.orders.Add(order);
                }
                db.SaveChanges();
                return PartialView("_success");
            }
            return PartialView("_error");
        }

        public ActionResult Edit(int? id)
        {
            client client = db.clients.First(x => x.clientId == id);
            var clientSpot = db.orders.Where(x => x.clientId == client.clientId).ToList();

            ClientVM clientVM = new ClientVM()
            {
                clientId = client.clientId,
                clientName = client.clientName,
                birthDate = client.birthDate,
                age = client.age,
                picture = client.picture,
                insideDhaka = client.insideDhaka
            };
            Session["imPath"] = client.picture;
            if (clientSpot.Count() > 0)
            {
                foreach (var item in clientSpot)
                {
                    clientVM.ProductList.Add(item.productId);
                }
            }
            return View(clientVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientVM clientVM, int[] productId)
        {
            if (ModelState.IsValid)
            {
                client client = new client()
                {
                    clientId = clientVM.clientId,
                    clientName = clientVM.clientName,
                    birthDate = clientVM.birthDate,
                    age = clientVM.age,
                    insideDhaka = clientVM.insideDhaka
                };

                //for image
                HttpPostedFileBase file = clientVM.pictureFile;
                var oldPic = clientVM.picture;

                if (file != null)
                {
                    string filePath = Path.Combine("/Images/", DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(filePath));
                    client.picture = filePath;
                }
                else
                {
                    client.picture = oldPic;
                }

                //
                var spotEntry = db.orders.Where(x => x.clientId == client.clientId).ToList();

                foreach (var order in spotEntry)
                {
                    db.orders.Remove(order);
                }
                foreach (var item in productId)
                {
                    order order = new order()
                    {
                        clientId = client.clientId,
                        productId = item
                    };
                    db.orders.Add(order);
                }
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return PartialView("_success");
            }
            return PartialView("_error");
        }



        public ActionResult Delete(int? id)
        {
            client client = db.clients.First(x => x.clientId == id);
            var clientSpot = db.orders.Where(x => x.clientId == client.clientId).ToList();

            ClientVM clientVM = new ClientVM()
            {
                clientId = client.clientId,
                clientName = client.clientName,
                birthDate = client.birthDate,
                age = client.age,
                picture = client.picture,
                insideDhaka = client.insideDhaka
            };
            if (clientSpot.Count() > 0)
            {
                foreach (var item in clientSpot)
                {
                    clientVM.ProductList.Add(item.productId);
                }
            }
            return View(clientVM);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            client client = db.clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            var productEntry = db.orders.Where(x => x.clientId == client.clientId).ToList();

            foreach (var order in productEntry)
            {
                db.orders.Remove(order);
            }
            db.Entry(client).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}