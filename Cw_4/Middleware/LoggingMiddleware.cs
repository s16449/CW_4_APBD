using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cw_4.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            if (context.Request != null)
            {
                string path = context.Request.Path;
                string method = context.Request.Method; //GET, POST
                string queryString = context.Request.QueryString.ToString();
                string bodyString = "";


                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyString = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }

                string MiddlewareLog = "MiddleLog.txt";
                string logText =
                    "Log: " + DateTime.Now.ToString() +
                    "\nPath: " + path +
                    "\nMethod: " + method +
                    "\nQuery: " + queryString +
                    "\nBody: " + bodyString + "\n\n";

                if (File.Exists(MiddlewareLog))
                {
                    File.AppendAllText(MiddlewareLog, logText);
                }
                else
                {
                    File.WriteAllText(MiddlewareLog, logText);
                }

                if (_next != null)
                {
                    await _next(context);
                }
            }
        }
    }
}
