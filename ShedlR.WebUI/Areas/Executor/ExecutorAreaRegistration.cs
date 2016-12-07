using System.Web.Mvc;

namespace ShedlR.WebUI.Areas.Executor
{
    public class ExecutorAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Executor";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Executor_default",
                "Executor/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
