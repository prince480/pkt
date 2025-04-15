using Microsoft.AspNetCore.Mvc;
using PKT.DataAccess;
using PKT.Models;
using System.Diagnostics;
using System.Collections.Generic;
using System.Data;

namespace PKT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBContext _DBContext;
            
        public HomeController(ILogger<HomeController> logger, DBContext DBContext)
        {
            _DBContext = DBContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            UserInfo.UserDetails _UserDetails = new UserInfo.UserDetails();
            SessionHandler.ValidateSession(HttpContext, ref _UserDetails);
            ViewBag.UserName = _UserDetails.userName;
            ViewBag.CostName = _UserDetails.CostName;
            ViewBag.DOJ = _UserDetails.DOJ;
            ViewBag.PrivilegeName = _UserDetails.PrivilegeName;
            ViewBag.Privilege = Convert.ToInt32(_UserDetails.privilege);
            return View();
        }

        public IActionResult Agent()
        {
            UserInfo.UserDetails _UserDetails = new UserInfo.UserDetails();
            SessionHandler.ValidateSession(HttpContext, ref _UserDetails);
            ViewBag.UserName = _UserDetails.userName;
            ViewBag.CostName = _UserDetails.CostName;
            ViewBag.DOJ = _UserDetails.DOJ;
            ViewBag.PrivilegeName = _UserDetails.PrivilegeName;
            ViewBag.Privilege = Convert.ToInt32(_UserDetails.privilege);
            return View();

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        


    }
}