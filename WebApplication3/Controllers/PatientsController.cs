using cloudscribe.Pagination.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;
using PagedList;
using PagedList.Mvc;
using System.Web.UI;

namespace WebApplication3.Controllers
{
    [Authorize(Roles = "Admin")]
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class PatientsController : Controller
    {
        private ClinicManagementEntities db = new ClinicManagementEntities();

        // GET: Patients

        public ActionResult Index(string search,int?  j)
        {
            var patients = db.Patients.Include(p => p.Medicine).ToList();
           
            Patient res = new Patient();
            if (patients!=null)
            {
                foreach(var i in patients)
                {
                    if (!(i.Medicines==null))
                    {
                        StringBuilder sb = new StringBuilder();
                        res.Medicines = String.Concat(i.Medicines);
                        int[] midint = res.Medicines.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                        foreach (var t in midint)
                        {
                            var id = db.Medicines.Where(x => x.MedicineId == t);
                            var val = id.Select(x => x.MedicineShortName).FirstOrDefault();
                            i.Medicines = val;
                            if(!string.IsNullOrEmpty(i.Medicines))
                            sb.Append(val + ",");

                        }
                        if(sb.Length>0)
                        sb.Remove(sb.Length - 1, 1);
                        i.Medicines = sb.ToString();

                        //i.PID = i.PatientId + i.PatientName.Substring(0, 3);
                        sb.Clear();
                    }
                    if (i.PatientId>0 && i.PatientName.Length>=3)
                        i.PID = i.PatientId + i.PatientName.Substring(0, 3);

                }
            }
            if (search != null && search != "")
            {
                var mob=patients.Select(x => x.MobileNumber);
                var Pname=patients.Select(x => x.PatientName);
                if(mob!=null && Pname!=null)
                patients = patients.Where(x => x.PatientName.ToLower().Contains(search.ToLower()) || x.MobileNumber.ToLower().Contains(search.ToLower())).ToList();
            }
            return View(patients.ToPagedList(j??1,3));
        }

        // GET: Patients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            Patient res = new Patient();
            StringBuilder sb = new StringBuilder();
            if (patient!=null)
            {
                    res.Medicines = String.Concat(patient.Medicines);
                    if(!string.IsNullOrEmpty(res.Medicines))
                   {
                    int[] midint = res.Medicines.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                    foreach (var t in midint)
                    {
                        var pid = db.Medicines.Where(x => x.MedicineId == t);
                        var val = pid.Select(x => x.MedicineShortName).FirstOrDefault();
                        patient.Medicines = val;
                        sb.Append(val + ",");
                    }
                    patient.Medicines = sb.ToString();
                   }
                  
            }
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            ViewBag.MedicineId = new MultiSelectList(db.Medicines, "MedicineId", "MedicineShortName");
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientId,PatientName,Age,Address,Sex,Date,MobileNumber,Diagnosis,Medicines,Investigation,Note,MedicineId,SelectedIDMedicines")] Patient patient)
        {
            string result = null;
            if (patient.SelectedIDMedicines != null)
            {
                result = string.Join(",", patient.SelectedIDMedicines);
            }
            patient.Medicines = result;
            if (ModelState.IsValid)
                {

                        db.Patients.Add(patient);
                        db.SaveChanges();
                     
                    
                return RedirectToAction("Index");
            }
            

            ViewBag.MedicineId = new SelectList(db.Medicines, "MedicineId", "MedicineShortName", patient.MedicineId);
            return View(patient);
        }

        // GET: Patients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            Patient res = new Patient();
            StringBuilder sb = new StringBuilder();
            if (patient != null)
            {
                res.Medicines = String.Concat(patient.Medicines);
                if(!string.IsNullOrEmpty(res.Medicines))
                {
                    int[] midint = res.Medicines.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                    foreach (var t in midint)
                    {
                        var pid = db.Medicines.Where(x => x.MedicineId == t);
                        var val = pid.Select(x => x.MedicineShortName).FirstOrDefault();
                        patient.Medicines = val;
                        sb.Append(val + ",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    patient.Medicines = sb.ToString();
                }
            }
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewBag.MedicineId = new SelectList(db.Medicines, "MedicineId", "MedicineShortName", patient.MedicineId);
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatientId,PatientName,Age,Address,Sex,Date,MobileNumber,Diagnosis,Medicines,Investigation,Note,MedicineId,SelectedIDMedicines")] Patient patient)
        {
            string result=null;
            if (patient.SelectedIDMedicines!=null)
            {
               result = string.Join(",", patient.SelectedIDMedicines);
            }

            patient.Medicines = result;
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MedicineId = new SelectList(db.Medicines, "MedicineId", "MedicineShortName", patient.MedicineId);
            return View(patient);
        }

        // GET: Patients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            Patient res = new Patient();
            StringBuilder sb = new StringBuilder();
            if (patient != null)
            {
                res.Medicines = String.Concat(patient.Medicines);
                if(!string.IsNullOrEmpty(res.Medicines))
                {
                    int[] midint = res.Medicines.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                    foreach (var t in midint)
                    {
                        var pid = db.Medicines.Where(x => x.MedicineId == t);
                        var val = pid.Select(x => x.MedicineShortName).FirstOrDefault();
                        patient.Medicines = val;
                        sb.Append(val + ",");
                    }
                    patient.Medicines = sb.ToString();
                }
            }
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
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
