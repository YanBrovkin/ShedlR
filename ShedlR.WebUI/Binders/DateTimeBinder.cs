using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShedlR.WebUI.Binders
{
    public class DateTimeBinder: IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            DateTime currentValue = DateTime.MinValue;
            string modelName = bindingContext.ModelName;

            var value = GetValue(bindingContext, "", bindingContext.ModelName);
            try
            {
                if (value.Contains("Date"))
                {
                    currentValue = jsonDateDeserialize(value);
                }
                else
                {
                    currentValue = DateTime.Parse(value);
                }
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex);
            }
            return currentValue;
        }
        private string GetValue(ModelBindingContext context, string prefix, string key)
        {
            ValueProviderResult vpr;
            if (prefix == "")
            {
                vpr = context.ValueProvider.GetValue(key);
                return vpr == null ? null : vpr.AttemptedValue;
            }
            else
            {
                vpr = context.ValueProvider.GetValue(prefix + "[" + key + "]");
                if (vpr == null)
                {
                    prefix = prefix.Replace(".", "");
                    vpr = context.ValueProvider.GetValue(prefix + "[" + key + "]");
                }
                return vpr == null ? null : vpr.AttemptedValue;
            }

        }
        private DateTime jsonDateDeserialize(string date_value)
        {
            if (date_value.Contains("Date"))
                date_value = date_value.Replace("/Date(", "").Replace(")/", "");
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(date_value));
            dt = dt.ToLocalTime();
            return dt;
        }
    }
}