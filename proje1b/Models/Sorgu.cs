using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proje1b.Models
{
    public class Sorgu
    {
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public string BaslangicSaat { get; set; }
        public string BitisSaat { get; set; }
        public string BaslangicDakika { get; set; }
        public int BitisDakika { get; set; }

    }
}