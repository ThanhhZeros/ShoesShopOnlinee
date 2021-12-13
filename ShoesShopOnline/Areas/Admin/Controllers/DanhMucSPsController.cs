using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoesShopOnline.Models;

namespace ShoesShopOnline.Areas.Admin.Controllers
{
    public class DanhMucSPsController : Controller
    {
        private Shoes db = new Shoes();

        // GET: Admin/DanhMucSPs
        public ActionResult Index()
        {
            return View(db.DanhMucSPs.ToList());
        }

        // GET: Admin/DanhMucSPs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMucSP danhMucSP = db.DanhMucSPs.Find(id);
            if (danhMucSP == null)
            {
                return HttpNotFound();
            }
            return View(danhMucSP);
        }

        // GET: Admin/DanhMucSPs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/DanhMucSPs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDM,TenDanhMuc")] DanhMucSP danhMucSP)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Tạo mã danh mục
                    string madm="";
                    var list = db.DanhMucSPs.ToList();
                    var danhmuc=list.LastOrDefault();
                    if (danhmuc == null)
                    {
                        madm = "DM01";
                    }
                    else
                    {
                        int index = int.Parse(danhmuc.MaDM.Substring(2, 2)) + 1;
                        madm = "DM" + string.Format(CultureInfo.CreateSpecificCulture("da-DK"), "{0:00}", index);
                    }
                    danhMucSP.MaDM = madm;
                    db.DanhMucSPs.Add(danhMucSP);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(danhMucSP);
            }
            catch(Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu !" + ex.Message;
                return View(danhMucSP);
            }
        }

        // GET: Admin/DanhMucSPs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMucSP danhMucSP = db.DanhMucSPs.Find(id);
            if (danhMucSP == null)
            {
                return HttpNotFound();
            }
            return View(danhMucSP);
        }

        // POST: Admin/DanhMucSPs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDM,TenDanhMuc")] DanhMucSP danhMucSP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(danhMucSP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(danhMucSP);
        }

        // GET: Admin/DanhMucSPs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhMucSP danhMucSP = db.DanhMucSPs.Find(id);
            if (danhMucSP == null)
            {
                return HttpNotFound();
            }
            return View(danhMucSP);
        }

        // POST: Admin/DanhMucSPs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DanhMucSP danhMucSP = db.DanhMucSPs.Find(id);
            db.DanhMucSPs.Remove(danhMucSP);
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
