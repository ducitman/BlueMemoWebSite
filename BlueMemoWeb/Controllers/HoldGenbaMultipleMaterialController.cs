using BlueMemoWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlueMemoWeb.Controllers
{
    public class HoldGenbaMultipleMaterialController : Controller
    {
        // GET: HoldGenbaMaterial
        string _result = "NG";
        string _description = "";
        DbBusiness dbBusiness = new DbBusiness();
        public ActionResult Index()
        {
            dbBusiness.SetConnectionString();
            return View();
        }

        [ActionName("Hold_Genba_Multiple_Material")]
        public ActionResult Hold_Genba_Material(string operatorID, string holdID, string ef)
        {
            _result = "NG";
            _description = "";
            if (String.IsNullOrEmpty(holdID))
            {
                _description += "Chua dang ky Hold ID" + Environment.NewLine;
                _description += "Vui long bam Nhap lai" + Environment.NewLine;
            }
            else
            {
                
            }
            return Content(_result + "#" + _description);
        }
    }
}