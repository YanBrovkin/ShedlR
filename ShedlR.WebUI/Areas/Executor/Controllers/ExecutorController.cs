using ShedlR.Domain.DAL;
using ShedlR.Domain.Interfaces;
using ShedlR.Domain.Models;
using ShedlR.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using System.Web.Http.Description;
using System.Web;
using System.Web.Mvc;

namespace ShedlR.WebUI.Areas.Executor.Controllers
{
    //[System.Web.Mvc.Authorize(Roles = "Executor")]
    public class ExecutorController : Controller
    {
        private ILogger _logger;
        private string _classname;
        //
        // GET: /Executor/Executor/
        public ExecutorController()
        {
            _classname = this.ToString();
            _logger = DependencyResolver.Current.GetService<ILogger>();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetTasks(string selectedDate="")
        {
            try
            {
                using (EfUnitOfWork unitOfWork = new EfUnitOfWork())
                {
                    DateTime currentDT = DateTime.Now;
                    DateTime selected;
                    if (DateTime.TryParseExact(selectedDate,"dd.MM.yyyy hh:m:ss",null, DateTimeStyles.None, out selected))
                    { 
                        //Возвратим результаты представлению
                        return Json(unitOfWork.Get<IEFRepository<TaskItem>>().Get(filter: s => (s.RegisteredAt.Year == selected.Year && 
                                                                                                s.RegisteredAt.Month == selected.Month && 
                                                                                                s.RegisteredAt.Day == selected.Day &&
                                                                                                s.RegisteredAt.Hour == selected.Hour &&
                                                                                                s.RegisteredAt.Minute == selected.Minute)).ToList());
                    }
                    else  //Возвратим результаты представлению
                        return Json(unitOfWork.Get<IEFRepository<TaskItem>>().Get(filter: s => (s.RegisteredAt.Year == currentDT.Year && 
                                                                                                s.RegisteredAt.Month == currentDT.Month && 
                                                                                                s.RegisteredAt.Day == currentDT.Day //&&
                                                                                                //s.RegisteredAt.Hour == currentDT.Hour &&
                                                                                                //s.RegisteredAt.Minute == currentDT.Minute
                                                                                                )).ToList());
                }
            }
            catch (Exception ex)
            {
                string methodname = System.Reflection.MethodBase.GetCurrentMethod().Name;

                _logger.Error(ex.Message, classname: _classname, methodname: methodname);

                return Json(new { Errors = ex.Message });
            }

        }
        [HttpPost]
        public JsonResult Create(TaskItem task)
        {
            try
            {
                if (task != null && ModelState.IsValid)
                {
                    using (EfUnitOfWork unitOfWork = new EfUnitOfWork())
                    {
                        task.Executor = User.Identity.Name;
                        unitOfWork.Get<IEFRepository<TaskItem>>().Insert(task);
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

                return Json(new { status = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Edit(TaskItem task, int? id = 0)
        {
            try
            {
                if (task != null && ModelState.IsValid)
                {
                    using (EfUnitOfWork unitOfWork = new EfUnitOfWork())
                    {
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

                return Json(new { status = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Delete(int? id = 0)
        {
            try
            {
                    // Создаем экземпляр прокси-класса сервиса
                    using (EfUnitOfWork unitOfWork = new EfUnitOfWork())
                    {
                        var task = unitOfWork.Get<IEFRepository<TaskItem>>().GetByID(id);
                        if (task != null)
                        {
                            unitOfWork.Get<IEFRepository<TaskItem>>().Delete(task);
                            unitOfWork.Commit();
                            return Json(task);
                        }
                        else
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
