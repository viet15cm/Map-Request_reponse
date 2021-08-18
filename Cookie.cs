using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MapReqRep
{
    public static class Cookie
    {
        public static string Cookies(HttpRequest request, HttpResponse response)
        {

            var message = "";
            switch (request.Path.ToString())
            {
                case "/Cookies/Write":
                    var optionCookie = new CookieOptions()
                    {
                        Path = "/",
                        Expires = DateTime.Now.AddDays(1),
                        HttpOnly = true,
                    };
                    response.Cookies.Append("MaAdim", "fadsafsafa", optionCookie);
                    response.Cookies.Append("MaStudents", "23421", optionCookie);
                    message = "Ghi Cookie thanh cong";
                    break;
                case "/Cookies/Read":

                    var listcokie = request.Cookies.Select((header) => $"{header.Key}: {header.Value}");
                    message = string.Join("", listcokie).HtmlTag("ul");
                    break;
                case "/Cookies/Delete":
                    foreach (var cookie in request.Cookies)
                    {
                        response.Cookies.Delete(cookie.Key);
                    }
                    message = "Xóa Cookie thanh cong";
                    break;

                default:
                    break;
            }


            return message;

          
        }
    }
}
