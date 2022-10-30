using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using u19205547_HW06.Models;
using PagedList;
using System.Data.Entity;
using Newtonsoft.Json;

namespace u19205547_HW06.Controllers
{
    public class ordersController : Controller
    {
        private BikeStoresEntities db = new BikeStoresEntities();

        //GET: orders
        public ActionResult Orders(string sortOrder, string currentFilter, string searchDate, int? page)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchDate != null)
            {
                page = 1;
            }
            else
            {
                searchDate = currentFilter;
            }

            ViewBag.CurrentFilter = searchDate;
            //create temprorary tables of all product, order item and order objects

            var productS = from prod in db.products
                           select prod;
            var orderItemS = from orI in db.order_items
                             select orI;
            var orderS = from or in db.orders
                         select or;

            if (!String.IsNullOrEmpty(searchDate)) //if there is a date to search for
            { //selects only ordera whose date matches
                orderS = orderS.Where(o => o.order_date.ToString().Contains(searchDate));
            }

            var joinResult = (from p in productS
                          join i in orderItemS on p.product_id equals i.product_id into tempTable1
                          from i in tempTable1.ToList()
                          join o in orderS on i.order_id equals o.order_id into tempTable2
                          from o in tempTable2.ToList()
                          select new ProductOrderJoin
                          {
                              ProductDetails = p,
                              OrderItemDetails = i,
                              OrderDetails = o
                          });

            switch (sortOrder)              
            {
                case "date_desc":
                    joinResult = joinResult.OrderByDescending(s => s.OrderDetails.order_date);
                    break;

                default:
                    joinResult = joinResult.OrderBy(p => p.OrderDetails.order_id);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1); //(page ?? 1) means return the value of page if it has a value, or return 1 if page is null = null-coalescing operator

            return View(joinResult.ToPagedList(pageNumber, pageSize));

        }



    }
}