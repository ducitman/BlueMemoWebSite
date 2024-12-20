using BlueMemoWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlueMemoWeb.Controllers
{
    public class HoldGenbaMaterialController : Controller
    {
        // GET: HoldGenbaMaterial
        string _result = "NG";
        string _description = "";
        DbBusiness _dbBusiness = new DbBusiness();
        public ActionResult Index()
        {
            _dbBusiness.SetConnectionString();
            return View();
        }

        [ActionName("Hold_Genba_Material")]
        public ActionResult Hold_Genba_Material(string operatorID, string ef)
        {
            _result = "";
            _description = "";
            //if (_dbBusiness.HoldEfuTag(ef).StartsWith("NG"))
            //{
            //    _result = "NG";
            //    _description += "Không tồn tại dữ liệu của barcode Efu trên" + Environment.NewLine;
            //    _description += "Vui lòng thử lại barcode Efu thẻ khác" + Environment.NewLine;
            //}
            //else
            //{
            //    _description += "Xác nhận thông tin\nVà Đăng kí BlueMemo" + Environment.NewLine;
            //}
            return Content(_result + "#" + _description);
        }

        [ActionName("Get_EFU_Data")]
        public ActionResult Get_EFU_Data(string ef)
        {
            _result = _dbBusiness.StatusOfEfu(ef);
            _description = "";
            if (_result.StartsWith("NG"))
            {
                _result = "NG";
                _description += "Không tồn tại dữ liệu của barcode Efu trên" + Environment.NewLine;
                _description += "Vui lòng thử lại barcode Efu thẻ khác" + Environment.NewLine;
            }
            else
            {
                _description += "Chọn lý do trước khi\nđăng kí BlueMemo" + Environment.NewLine;
            }
            var content = _result + "#" + _description;
            return Content(content);
        }
    }
}