using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Api.Filters
{
    public class LogActionFilter : IActionFilter
    {
        private readonly ILogger _logger;
        public LogActionFilter(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger<LogActionFilter>();
        }

        /// <summary>
        /// Controller output for a given ActionContext
        /// </summary>
        /// <param name="context">value for the ActionContext</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            StringBuilder message = new StringBuilder("Output : ");
            foreach (var item in context.RouteData.Values)
            {
                message.Append(item);
            }
            _logger.LogInformation($"{message} Http Request Information: {context.HttpContext.Request.Method}");
        }

        /// <summary>
        /// Controller intput for a given ActionContext
        /// </summary>
        /// <param name="context">value for the ActionContext</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            StringBuilder message = new StringBuilder("Input : ");

            foreach (var item in context.RouteData.Values)
            {
                message.Append(item);
            }
            _logger.LogInformation($"{message} Http Request Information: {context.HttpContext.Request.Method}");
        }

        /// <summary>
        /// Exeception for a given ActionContext
        /// </summary>
        /// <param name="context">value for the ActionContext</param>
        public void OnException(ExceptionContext context)
        {
            _logger.LogError("Error : " + context.Exception);
        }
    }
}
