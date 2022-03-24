using proje1b.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using System.Web.Security;

namespace proje1b.Controllers
{

    public class HomeController : Controller
    {

        private static int counter = 0;

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(Yonetim yonetim)
        {
            string connStr = "connection";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = $"SELECT * FROM Kullanicilar WHERE kullaniciAdi = '{yonetim.UserName}' AND sifre = '{yonetim.Password}';";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    rdr.Close();
                    conn.Close();
                    System.Diagnostics.Debug.WriteLine("Doru");
                    // haritaya yolla
                    FormsAuthentication.SetAuthCookie(yonetim.UserName, false);
                    ViewBag.Kullanici = yonetim.UserName;
                    return RedirectToAction("Index", "Harita", new { kullaniciAdi = yonetim.UserName });
                    // kullanici adi yollanacak

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            System.Diagnostics.Debug.WriteLine("Yannis");
            counter++;
            System.Diagnostics.Debug.WriteLine("counter =" + counter);
            ViewBag.Message = string.Format("Kullanıcı Adı veya parola yanlış. Hatalı Deneme: " + counter);

            if (counter == 3)
            {
                return View("NotFound");
            }
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        public ViewResult NotFound()
        {
            Response.StatusCode = 404;  //you may want to set this to 200
            return View("NotFound");
        }
        [Authorize]
        public ActionResult Cikis()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}
