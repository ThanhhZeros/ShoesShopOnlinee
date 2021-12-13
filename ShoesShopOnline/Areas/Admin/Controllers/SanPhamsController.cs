using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoesShopOnline.Models;

namespace ShoesShopOnline.Areas.Admin.Controllers
{
    public class SanPhamsController : Controller
    {
        private Shoes db = new Shoes();

        // GET: Admin/SanPhams
        public ActionResult Index()
        {
            //var sanPhams = db.SanPhams.Include(s => s.DanhMucSP);
            var result = db.SanPhams.Join(db.AnhMoTas, p => p.MaSP, a => a.MaSP, (p, a) => new ProductDetail
            {
                MaSP=p.MaSP,
                TenSP = p.TenSP,
                MaDM = p.DanhMucSP.TenDanhMuc,
                Anh = a.HinhAnh,
                MoTa = p.MoTa,
                GiaBan = p.GiaBan,
                New = p.New

            }).ToList();
            var productDetails = new List<ProductDetail>();
            foreach(var item in result)
            {
                int dem = 0;
                foreach (var t in productDetails)
                {
                    if (item.TenSP.Equals(t.TenSP))
                        dem++;
                }
                if(dem==0)
                    productDetails.Add(item);               
            }
          
            return View(productDetails);
        }

        // GET: Admin/SanPhams/Details/id
        public ActionResult Details(string id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                SanPham sanPham = db.SanPhams.Find(id);
                if (sanPham == null)
                {
                    return HttpNotFound();
                }
                var images = db.AnhMoTas.Where(p => p.MaSP == id).ToList();
                string filePath = "";
                if (images != null)
                {
                    foreach (var item in images )
                    {
                        filePath += item.HinhAnh+";";
                    }    
                }
                filePath = filePath.Substring(0, filePath.Length - 1);
                ViewBag.filePath = filePath;
                return View(sanPham);

            }
            catch(Exception ex)
            {
                ViewBag.Error = "Lỗi !" + ex.Message;
                return View();
            }
            
        }

        // GET: Admin/SanPhams/Create
        public ActionResult Create()
        {
            ViewBag.MaDM = new SelectList(db.DanhMucSPs, "MaDM", "TenDanhMuc");
            //ViewBag.MaDM = db.DanhMucSPs.ToList();

            return View();
        }

        // POST: Admin/SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductDetail productDetail, List<HttpPostedFileBase> uploadFile)
        {
            try
            {
                productDetail.Anh = "";
               // HttpFileCollectionBase images = Request.Files;
               //var f=Request.Files["ImageFile"];
               
                //var t = Request.Files["ImageFile"];
                //if (f != null)
                //{
                //    for(int i=0;i<f.Count;i++)
                //    {
                //        var t = f[i];
                //    }    
                    //string FileName = System.IO.Path.GetFileName(f.);
                    //string UploadPath = Server.MapPath("~/wwwroot/Images/" + FileName);
                    //f.SaveAs(UploadPath);
                    //productDetail.Anh = FileName;
                //}
                if (ModelState.IsValid)
                {
                    SanPham sanPham = new SanPham();
                    string maSP = "";
                    var list = db.SanPhams.ToList();
                    var sanpham = list.LastOrDefault();
                    if (sanpham == null)
                    {
                        maSP = "SP001";
                    }
                    else
                    {
                        int index = int.Parse(sanpham.MaSP.Substring(2, 3)) + 1;
                        maSP = "SP" + string.Format(CultureInfo.CreateSpecificCulture("da-DK"), "{0:000}", index);
                    }
                    sanPham.MaSP = maSP;
                    sanPham.TenSP = productDetail.TenSP;
                    sanPham.MaDM = productDetail.MaDM;
                    sanPham.MoTa = productDetail.MoTa;
                    sanPham.GiaBan = productDetail.GiaBan;
                    sanPham.New = productDetail.New;
                    db.SanPhams.Add(sanPham);
                    db.SaveChanges();
                    //Add Anh
                    string[] sizes = productDetail.Size.Split(';');
                    foreach (var item in uploadFile)
                    {
                        AnhMoTa anhMoTa = new AnhMoTa();
                        string FileName = item.FileName;
                        string filePath = Path.Combine(HttpContext.Server.MapPath("/wwwroot/Images"),
                                                       Path.GetFileName(item.FileName));
                        item.SaveAs(filePath);
                        anhMoTa.HinhAnh = FileName;
                        anhMoTa.MaSP = maSP;
                        db.AnhMoTas.Add(anhMoTa);
                        db.SaveChanges();
                        //Add san pham chi tiet
                        for (int j = 0; j < sizes.Length; j++)
                        {
                            ChiTietSanPham chiTietSanPham = new ChiTietSanPham();
                            chiTietSanPham.MaAnh = anhMoTa.MaAnh;
                            chiTietSanPham.KichCo = int.Parse(sizes[j]);
                            db.ChiTietSanPhams.Add(chiTietSanPham);
                            db.SaveChanges();
                        }
                    }

                    return RedirectToAction("Index");
                }
                ViewBag.MaDM = new SelectList(db.DanhMucSPs, "MaDM", "TenDanhMuc", productDetail.MaDM);
                return View(productDetail);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu !" + ex.Message;
                ViewBag.MaDM = new SelectList(db.DanhMucSPs, "MaDM", "TenDanhMuc", productDetail.MaDM);
                return View(productDetail);

            }

        }

        // GET: Admin/SanPhams/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            int maAnh=0;
            var images = db.AnhMoTas.Where(p => p.MaSP == id).ToList();
            string filePath = "";
            if (images != null)
            {
                foreach (var item in images)
                {
                    maAnh = item.MaAnh;
                    filePath += item.HinhAnh + ";";
                }
            }
            filePath = filePath.Substring(0, filePath.Length - 1);
            ViewBag.filePath = filePath;
            string listSize = "";
            var sizes = db.ChiTietSanPhams.Where(p => p.MaAnh == maAnh).ToList();
            if (sizes != null)
            {
                foreach (var item in sizes)
                {
                    listSize += item.KichCo + ";";
                }
            }
            listSize = listSize.Substring(0, listSize.Length - 1);
            ViewBag.listSize = listSize;
            ViewBag.MaDM = new SelectList(db.DanhMucSPs, "MaDM", "TenDanhMuc", sanPham.MaDM);
            ProductDetail productDetail = new ProductDetail()
            {
                MaSP = sanPham.MaSP,
                TenSP = sanPham.TenSP,
                GiaBan = sanPham.GiaBan,
                Anh = filePath,
                Size = listSize,
                New = sanPham.New,
                MoTa = sanPham.MoTa,
                MaDM = sanPham.MaDM,
                DanhMucSP = sanPham.DanhMucSP
            };
            return View(productDetail);
        }

        // POST: Admin/SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductDetail productDetail, List<HttpPostedFileBase> uploadFile)
        {
            try
            {
                if (productDetail.Anh == null && uploadFile == null)
                {
                    ViewBag.Error = "Chọn ảnh bạn ơi !";
                    ViewBag.MaDM = new SelectList(db.DanhMucSPs, "MaDM", "TenDanhMuc", productDetail.MaDM);
                    return View(productDetail);
                }
                if (ModelState.IsValid)
                {
                    SanPham sanPham = new SanPham();
                    sanPham.MaSP = productDetail.MaSP;
                    db.Entry(sanPham).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.MaDM = new SelectList(db.DanhMucSPs, "MaDM", "TenDanhMuc", productDetail.MaDM);
                return View(productDetail);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu !" + ex.Message;
                ViewBag.MaDM = new SelectList(db.DanhMucSPs, "MaDM", "TenDanhMuc", productDetail.MaDM);
                return View(productDetail);

            }
        }

        // GET: Admin/SanPhams/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
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
