using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartHome.ViewModel;
using SmartHome.Service;

namespace RESTService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            if (Session["Request"] != null)
            {
                var viewModel = Session["Request"] as RequestSession;
                ViewBag.SessionID = viewModel.SessionID;
                ViewBag.SecretKey = viewModel.SecretKey;
            }

            return View();
        }

        public JsonResult RequestSession(string sessionID, string secretKey)
        {
            var viewModel = new RequestSession() { SessionID = sessionID, SecretKey = secretKey, UserID = "userid", Pin = "1234" };
            viewModel.Hash = HashMD5.GetMd5Hash(viewModel.Pin + viewModel.SecretKey);
            Session["Request"] = viewModel;
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateNewKey(string secretKey)
        {
            if (Session["Request"] != null)
            {
                var viewModel = Session["Request"] as RequestSession;
                var sessionid = viewModel.SessionID;
                var pin = viewModel.Pin;
                viewModel.SecretKey = secretKey;
                viewModel.Hash = HashMD5.GetMd5Hash(viewModel.Pin + secretKey);
                Session["Request"] = viewModel;
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GenerateRequest()
        {
            if (Session["Request"] != null)
            {
                var viewModel = Session["Request"] as RequestSession;
                var userid = viewModel.UserID;
                var sessionid = viewModel.SessionID;
                var hash = viewModel.Hash;
                var result = new Dictionary<string,string>();
                result.Add("userid", userid);
                result.Add("sessionid", sessionid);
                result.Add("hash", hash);
                var requestString = string.Format("UserID={0}&SessionID={1}&Hash={2}", viewModel.UserID, viewModel.SessionID, viewModel.Hash);
                return Json(requestString, JsonRequestBehavior.AllowGet);
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

    }
}
