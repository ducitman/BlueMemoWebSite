using BlueMemoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlueMemoWeb.Controllers
{
    public class HomeController : Controller
    {
        DbBusiness dbBusiness = new DbBusiness();
        public ActionResult Index()
        {
            dbBusiness.SetConnectionString();
            return View();
        }
        [ActionName("CheckOP")]
        public ActionResult CheckOP(string operatorId, string programName, string functionName)
        {                      
            string result = "NG";
            string description = "";
            try
            {
                if (!dbBusiness.CheckAuthorization(operatorId, programName, functionName))
                {
                    result = "NG";
                    description = "Ma OP nay khong co quyen";
                }
                else
                {
                    result = "OK";
                    description = DateTime.Now.ToString("ddMMyy-HHmmss");
                }
            }
            catch
            {
                result = "NG";
                description = "Khong the ket noi toi may chu";
            }
            return Content(result + "#" + description);
        }
    }
}