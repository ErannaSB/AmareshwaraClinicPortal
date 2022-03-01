using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MedicinesController : Controller
    {
        private ClinicManagementEntities db = new ClinicManagementEntities();

        // GET: Medicines
        public ActionResult Index(string search, int? j)
        {

            var medicines = db.Medicines.ToList();
            try
            {
                if (search != null && search != "")
                {
                    var mob = medicines.Select(x => x.MedicineFullName);
                    var Pname = medicines.Select(x => x.MedicineShortName);
                    if (mob != null && Pname != null)
                        medicines = medicines.Where(x => x.MedicineFullName.ToLower().Contains(search.ToLower()) || x.MedicineShortName.ToLower().Contains(search.ToLower())).ToList();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return View(medicines.ToPagedList(j ?? 1, 15));
        }

        // GET: Medicines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicine medicine = db.Medicines.Find(id);
            if (medicine == null)
            {
                return HttpNotFound();
            }
            return View(medicine);
        }

        // GET: Medicines/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Medicines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MedicineId,MedicineShortName,MedicineFullName")] Medicine medicine)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Medicines.Add(medicine);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(medicine);
        }

        // GET: Medicines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicine medicine = db.Medicines.Find(id);
            if (medicine == null)
            {
                return HttpNotFound();
            }
            return View(medicine);
        }

        // POST: Medicines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MedicineId,MedicineShortName,MedicineFullName")] Medicine medicine)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(medicine).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(medicine);
        }

        // GET: Medicines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicine medicine = db.Medicines.Find(id);
            if (medicine == null)
            {
                return HttpNotFound();
            }
            return View(medicine);
        }

        // POST: Medicines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Medicine medicine = db.Medicines.Find(id);
                db.Medicines.Remove(medicine);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
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
