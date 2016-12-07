using ShedlR.Domain.DAL;
using ShedlR.Domain.Interfaces;
using ShedlR.Domain.Models;
using ShedlR.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShedlR.WebUI.Areas.Customer.Controllers
{
    //[System.Web.Mvc.Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private ILogger _logger;
        private string _classname;

        //
        // GET: /Customer/Customer/
        public CustomerController()
        {
            _classname = this.ToString();
            _logger = DependencyResolver.Current.GetService<ILogger>();

        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetTasks()
        {
            try
            {
                using (EfUnitOfWork unitOfWork = new EfUnitOfWork())
                {
                    //Возвратим результаты представлению
                    return Json(unitOfWork.Get<IEFRepository<TaskItem>>().Get().ToList());
                }
            }

            catch (Exception ex)
            {
                string methodname = System.Reflection.MethodBase.GetCurrentMethod().Name;

                _logger.Error(ex.Message, classname: _classname, methodname: methodname);

                return Json(new { Errors = ex.Message });
            }

        }

        public JsonResult ApproveTask(int? id = 0)
        {
            try
            {
                if (id != null && ModelState.IsValid)
                {

                    using (EfUnitOfWork unitOfWork = new EfUnitOfWork())
                    {
                        var task = unitOfWork.Get<IEFRepository<TaskItem>>().GetByID(id);
                        task.Approved = true;
                        unitOfWork.Get<IEFRepository<TaskItem>>().Update(task);
                        unitOfWork.Commit();
                        return Json(task);
                    }
                }
                else
                {
                    return Json(0);
                }

            }
            catch (Exception ex)
            {
                string methodname = System.Reflection.MethodBase.GetCurrentMethod().Name;

                _logger.Error(ex.Message, classname: _classname, methodname: methodname);

                return Json(new { Errors = ex.Message });
            }

        }

        public JsonResult DisApproveTask(int? id = 0)
        {
            try
            {
                if (id != null && ModelState.IsValid)
                {

                    using (EfUnitOfWork unitOfWork = new EfUnitOfWork())
                    {
                        var task = unitOfWork.Get<IEFRepository<TaskItem>>().GetByID(id);
                        task.Approved = false;
                        unitOfWork.Get<IEFRepository<TaskItem>>().Update(task);
                        unitOfWork.Commit();
                        return Json(task);
                    }
                }
                else
                {
                    return Json(0);
                }
            }
            catch (Exception ex)
            {
                string methodname = System.Reflection.MethodBase.GetCurrentMethod().Name;

                _logger.Error(ex.Message, classname: _classname, methodname: methodname);

                return Json(new { Errors = ex.Message });
            }

        }

    }
}
