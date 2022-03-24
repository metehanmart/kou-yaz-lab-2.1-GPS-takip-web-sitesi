using MongoDB.Driver;
using proje1b.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver.Linq;
using MySql.Data.MySqlClient;
using MongoDB.Bson;

namespace proje1b.Controllers
{
    public class HaritaController : Controller
    {
        private IMongoCollection<Araba> _collection;
        private static string kullanici;// düzeltmeye çalış
        private static DateTime tempTime = DateTime.Now;
        private static DateTime Tarih = tempTime.AddMinutes(-30);
        private static string b1saat;
        private static string b2saat;
        private static string arabaId = "secilmedi";
        private static double b3saat = 0;


        /*
        public HaritaController(IMongoClient client)
        {   


            var database = client.GetDatabase("taksi");
            _collection = database.GetCollection<Araba>("arabalar");
        }
        */
        // GET: Harita
        [Authorize]
        [HttpGet]
        public ActionResult Index(string kullaniciAdi)
        {
            ViewBag.kullaniciAdi = kullaniciAdi;

            kullanici = kullaniciAdi;
            System.Diagnostics.Debug.WriteLine("controller " + kullanici);

            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Index(DateTime BaslangicTarihi, string baslangicSaat, string bitisSaat, string secilenarabaID)
        {
            System.Diagnostics.Debug.WriteLine("baslangicSaat " + baslangicSaat);
            System.Diagnostics.Debug.WriteLine("tarih" + BaslangicTarihi.ToString());
            //Tarih = Tarih.AddMinutes(-30);
            Tarih = BaslangicTarihi;

            b1saat = baslangicSaat;
            double dsaat = Convert.ToDouble(b1saat);
            b2saat = bitisSaat;
            double d2saat;
            if (b2saat == "")
            {
                b2saat = b1saat;
                d2saat = Convert.ToDouble(b2saat);
                d2saat = d2saat + 1;
            }
            else
            {
                d2saat = Convert.ToDouble(b2saat);
            }

            arabaId = secilenarabaID;

            System.Diagnostics.Debug.WriteLine("arabaId = " + arabaId);


            b3saat = d2saat - dsaat;
            System.Diagnostics.Debug.WriteLine("dsaat= " + dsaat);
            System.Diagnostics.Debug.WriteLine("d2saat= " + d2saat);
            Tarih = Tarih.AddHours(dsaat);
            System.Diagnostics.Debug.WriteLine("Indexteki tarih = " + Tarih.ToString());
            return View();
        }
        [Authorize]
        public JsonResult HaritaIndex()
        {

            string connStr = "connection";
            MySqlConnection conn = new MySqlConnection(connStr);
            string[] araclar = { "a", "b" };
            System.Diagnostics.Debug.WriteLine("jsonControllerdayım");
            System.Diagnostics.Debug.WriteLine("json kullanici = " + kullanici);

            conn.Open();
            //string kullanici5 = "kullanici1";
            try
            {
                string sql = $"select araclar from Kullanicilar WHERE kullaniciAdi = '{kullanici}'";//buraya kullanici gelecek
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                System.Diagnostics.Debug.WriteLine("trydayım");
                if (rdr.Read())
                {

                    System.Diagnostics.Debug.WriteLine("ifteyim");
                    System.Diagnostics.Debug.WriteLine(rdr["araclar"].ToString());

                    string str = rdr["araclar"].ToString();
                    araclar = str.Split(',');

                    System.Diagnostics.Debug.WriteLine("arac 0 = " + araclar[0] + " arac 1 = " + araclar[1]);

                    rdr.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            conn.Close();

            //System.Diagnostics.Debug.WriteLine("arac 0 " + araclar[0] + " arac 1" + araclar[1]);
            var client = new MongoClient("mongodb_connection");
            var database = client.GetDatabase("taksi");
            _collection = database.GetCollection<Araba>("arabalar2");
            //return Json(_collection.Find(s => s.ArabaID == "6").ToList(), JsonRequestBehavior.AllowGet);
            //var result = _collection.AsQueryable<Araba>().Where(c => c.ArabaID == "6").Any();

            // ikinci çözüm
            //var query = new BsonDocument
            //{
            //    {
            //        "date", new BsonDocument
            //        {
            //            {"$gt" , Tarih },
            //            {"$lt" , DateTime.Now }
            //        }
            //    }
            //    //{
            //    //    "aracid", new BsonDocument
            //    //    {
            //    //        {"$eq" , araclar[0]},
            //    //        {"$eq" , araclar[1]}
            //    //    }
            //    //}
            //};

            //ilk çözüm
            //var filter = Builders<Araba>.Filter.Eq(c => c.ArabaID, araclar[0]);
            //filter |= (Builders<Araba>.Filter.Eq(c => c.ArabaID, araclar[1]));
            ////filter &= (Builders<Araba>.Filter.Gt(c => c.Date, Tarih));
            ////filter &= (Builders<Araba>.Filter.Lt(c => c.Date, DateTime.Now));
            //var projection = Builders<Araba>.Projection.Include("lat").Include("long").Include("aracid");
            //var newresult = _collection.Find<Araba>(filter).Project(projection).ToList();

            System.Diagnostics.Debug.WriteLine("Tarih == " + Tarih.ToString());
            if (arabaId == "secilmedi")
            {
                var result2 = _collection.AsQueryable<Araba>().
                Where(c => (c.ArabaID == araclar[0] || c.ArabaID == araclar[1]) && (c.Date > Tarih && c.Date < DateTime.Now)).
                Select(c => new { c.Lat, c.Long, c.ArabaID });
                return Json(result2, JsonRequestBehavior.AllowGet);
            }
            else if (arabaId == "")
            {
                DateTime tmptime = Tarih;
                double doublesaat = Convert.ToDouble(b2saat);
                tmptime = tmptime.AddHours(b3saat);
                System.Diagnostics.Debug.WriteLine("else if = " + tmptime.ToString());
                var result2 = _collection.AsQueryable<Araba>().
                Where(c => (c.ArabaID == araclar[0] || c.ArabaID == araclar[1]) && (c.Date > Tarih && c.Date < tmptime)).
                Select(c => new { c.Lat, c.Long, c.ArabaID });
                return Json(result2, JsonRequestBehavior.AllowGet);
            }
            else
            {
                DateTime tmptime = Tarih;
                double doublesaat = Convert.ToDouble(b3saat);
                tmptime = tmptime.AddHours(doublesaat);
                System.Diagnostics.Debug.WriteLine("else  = " + tmptime.ToString());
                var result1 = _collection.AsQueryable<Araba>().
                Where(c => (c.ArabaID == arabaId) && (c.Date > Tarih && c.Date < tmptime)).
                Select(c => new { c.Lat, c.Long, c.ArabaID });
                return Json(result1, JsonRequestBehavior.AllowGet);
            }






            //return Json(result, JsonRequestBehavior.AllowGet);
        }




    }
}
