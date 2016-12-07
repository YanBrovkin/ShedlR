using ShedlR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShedlR.WebUI.Binders
{
    public class TaskItemBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            NumberFormatInfo nfi = NumberFormatInfo.CurrentInfo;
            string CurrentDecimalSeparator = nfi.CurrencyDecimalSeparator;

            string currObject = "";
            string registeredAt = "";

            TaskItem taskItem = new TaskItem();
            // проверяем используется ли в модели префикс
            bool hasPrefix = bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName);
            // если да, то добавляем к нему .
            string searchPrefix = (hasPrefix) ? bindingContext.ModelName + "." : "";

            try
            {
                currObject = "Id";
                taskItem.Id = Convert.ToInt32(GetValue(bindingContext, searchPrefix, "Id"));
                currObject = "Customer";
                taskItem.Customer = GetValue(bindingContext, searchPrefix, "Customer");
                currObject = "Executor";
                taskItem.Executor = GetValue(bindingContext, searchPrefix, "Executor");
                currObject = "RegisteredAt";
                registeredAt = GetValue(bindingContext, searchPrefix, "RegisteredAt");
                if (registeredAt.Contains("Date"))
                {
                    taskItem.RegisteredAt = jsonDateDeserialize(registeredAt);
                }
                else
                {
                    taskItem.RegisteredAt = DateTime.Parse(registeredAt);
                }
                currObject = "Description";
                taskItem.Description = GetValue(bindingContext, searchPrefix, "Description");
                currObject = "ExecutionTime";
                taskItem.ExecutionTime = Convert.ToInt32(GetValue(bindingContext, searchPrefix, "ExecutionTime"));
                currObject = "Approved";
                taskItem.Approved = Boolean.Parse(GetValue(bindingContext, searchPrefix, "Approved"));
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError(currObject, ex);
            }
            return taskItem;
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

        private string Conversion(string str1, string str2)
        {
            if (str1.Contains(".") && (str2 != "."))
                return str1.Replace('.', ',');
            if (str1.Contains(",") && (str2 != ","))
                return str1.Replace(',', '.');
            return str1;
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