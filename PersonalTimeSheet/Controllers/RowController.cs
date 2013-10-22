using System;
using System.Web.Mvc;
using PersonalTimeSheet.DBUtils;
using PersonalTimeSheet.Models;
using System.Linq;

namespace PersonalTimeSheet.Controllers
{
    public class RowController : Controller
    {
        private DbService _dbService = new DbService();

        public int PageSize = 6;

        public ViewResult List(int page = 1)
        {
            var viewModel = new RowListViewModel()
                {
                    Rows = _dbService.Rows
                                     .OrderBy(i => i.Id)
                                     .Skip((page - 1)*PageSize)
                                     .Take(PageSize).ToList(),

                    Info = new PagingInfo()
                        {
                            CurrentPage = page,
                            ItemsPerPage = PageSize,
                            TotalItems = _dbService.Rows.Count
                        }
                };

            Session["curPage"] = page;

            return View(viewModel);
        }

        public ViewResult EditRow(int rowId=1)
        {
            var row = _dbService.Rows.SingleOrDefault(r => r.Id == rowId);
            if (null != row)
            {
                return View("EditRow2", row);
            }

            return View("EditRow2", new Row());
        }

        [HttpPost]
        public RedirectToRouteResult EditRow(Row row)
        {
            if (ModelState.IsValid)
            {
                _dbService.UpdateTableRow(row);
                return RedirectToAction("List", new { page = Convert.ToInt16(Session["curPage"]) });
            }

            return RedirectToAction("EditRow", new { rowId = row.Id });
        }

        public RedirectToRouteResult RemoveRow(int rowId, string returnUrl)
        {
            _dbService.RemoveTableRow(rowId);

            return RedirectToAction("List", new {page = Convert.ToInt16(Session["curPage"])});
        }

        public ViewResult NewRow()
        {
            return View(new Row());
        }

        [HttpPost]
        public RedirectToRouteResult NewRow(Row row)
        {
            if (ModelState.IsValid)
            {
                _dbService.AddTableRow(row);
                return RedirectToAction("List", new { page = Convert.ToInt16(Session["curPage"]) });
            }

            return RedirectToAction("NewRow");
        }

    }
}
