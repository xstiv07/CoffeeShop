using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace Restbucks.Filters
{

    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
       public override void OnActionExecuting(HttpActionContext actionContext)
       {
          if (!actionContext.ModelState.IsValid)
          {
              //PrettyHttpError error = new PrettyHttpError (actionContext.ModelState);
              //actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest,error);
              actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
          }
       }
    }

    public class PrettyHttpError
    {
        public PrettyHttpError(ModelStateDictionary modelState)
        {
            Message = "Your request is invalid.";
            Errors = new Dictionary<string, IEnumerable<string>>();

            foreach (var item in modelState)
            {
                var itemErrors = new List<string>();
                foreach (var childItem in item.Value.Errors)
                {
                    if (childItem.Exception.InnerException != null)
                    {
                        itemErrors.Add(childItem.ErrorMessage);
                        itemErrors.Add(childItem.Exception.InnerException.Message);
                    }
                    else
                    {
                        itemErrors.Add(childItem.ErrorMessage);
                    }
                }
                Errors.Add(item.Key, itemErrors);
            }
        }

        public string Message { get; set; }

        public IDictionary<string, IEnumerable<string>> Errors { get; set; }
    }

}