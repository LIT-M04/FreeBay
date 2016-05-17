using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FreeBay.Data;
using FreeBay.Models;

namespace FreeBay.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var manager = new ItemsManager(Properties.Settings.Default.ConStr);
            var viewModel = new IndexViewModel();
            viewModel.Items = manager.GetItems();
            if (Request.Cookies["itemids"] != null)
            {
                viewModel.MyIds = Request.Cookies["itemids"].Value.Split(',').Select(int.Parse);
            }

            return View(viewModel);
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string description, string name, string phone)
        {
            var manager = new ItemsManager(Properties.Settings.Default.ConStr);
            int newId = manager.Add(new Item
             {
                 Description = description,
                 Name = name,
                 PhoneNumber = phone
             });

            string cookieString;
            if (Request.Cookies["itemids"] != null)
            {
                cookieString = Request.Cookies["itemids"].Value + "," + newId;
            }
            else
            {
                cookieString = newId.ToString();
            }
            var cookie = new HttpCookie("itemids", cookieString);
            cookie.Expires = DateTime.Today.AddDays(14);
            Response.Cookies.Add(cookie);
            return Redirect("/home/index");
        }

        [HttpPost]
        public ActionResult Delete(int itemId)
        {
            var manager = new ItemsManager(Properties.Settings.Default.ConStr);
            manager.Delete(itemId);
            List<int> itemIds = Request.Cookies["itemids"].Value.Split(',').Select(int.Parse).ToList();
            itemIds.Remove(itemId);
            var cookie = new HttpCookie("itemids", String.Join(",", itemIds));
            DateTime expirationDate = itemIds.Count == 0 ? DateTime.Now.AddDays(-1) : DateTime.Today.AddDays(14);
            cookie.Expires = expirationDate;
            Response.Cookies.Add(cookie);
            return Redirect("/home/index");
        }

    }
}
