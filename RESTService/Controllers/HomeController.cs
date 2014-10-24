using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartHome.ViewModel;
using SmartHome.Repository;

namespace RESTService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            if (Session["Request"] != null)
            {
                var viewModel = Session["Request"] as IndexViewModel;
                ViewBag.SessionID = viewModel.SessionID;
                ViewBag.SecretKey = viewModel.SecretKey;
            }

            return View();
        }

        public JsonResult RequestSession(string sessionID,string secretKey)
        {
            var viewModel = new IndexViewModel() {SessionID = sessionID, SecretKey = secretKey, UserID = "userid", Pin ="1234"};
            viewModel.Hash = HashMD5.GetMd5Hash(viewModel.Pin + viewModel.SecretKey);
            Session["Request"] = viewModel;
            return Json(viewModel);
        }

        public JsonResult SendRequest()
        {
            if (Session["Request"] != null)
            {
                var viewModel = Session["Request"] as IndexViewModel;
                var userid = viewModel.UserID;
                var sessionid = viewModel.SessionID;
                var hash = viewModel.Hash;
                var result = new Dictionary<string,string>();
                result.Add("userid", userid);
                result.Add("sessionid", sessionid);
                result.Add("hash", hash);
                return Json(result);
            }
            return Json(string.Empty);
        }

    }
}
