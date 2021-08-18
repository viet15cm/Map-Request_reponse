using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace MapReqRep
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.Map("/Admin", (app1) => {
                app1.UseRouting();
                app1.UseEndpoints(endpoints =>
                {
                    endpoints.MapGet("/", async context =>
                    {
                        var menu = HtmlHelper.MenuTop(
                               HtmlHelper.DefaultMenuTopItems(),
                                context.Request
                            );

                        var html = HtmlHelper.HtmlDocument("Xin Chao", menu + HtmlHelper.HtmlTrangchu());

                        await context.Response.WriteAsync(html);
                    });
                    endpoints.MapGet("/RequestInfo", async (context) => {

                        var menu = HtmlHelper.MenuTop(
                               HtmlHelper.DefaultMenuTopItems(),
                                context.Request
                            );
                        string html = HtmlHelper.HtmlDocument("RequestInfo", menu + RepuestInfor.TakeRequest(context.Request));

                        await context.Response.WriteAsync(html);
                    });
                    endpoints.MapGet("/Cookies/{*action}", async (context) => {
                        var menu = HtmlHelper.MenuTop(
                               HtmlHelper.DefaultMenuTopItems(),
                                context.Request
                            );

                        /* var action = context.GetRouteValue("action") ?? "";
                         var message = "";
                         if (action.ToString() == "Write")
                         {
                             var option = new CookieOptions()
                             {
                                 Path = "/",
                                 Expires = DateTime.Now.AddDays(1),
                                 HttpOnly = true
                             };
                             context.Response.Cookies.Append("MaSP", "2321321", option);
                             message = "Cookie da duoc ghi";
                         }
                         else
                         {
                             var listcokie = context.Request.Cookies.Select((header) => $"{header.Key}: {header.Value}");
                             message = string.Join("", listcokie).HtmlTag("ul");
                         }

                         message = message.HtmlTag("div", "alert alert-danger");
                         */
                        var action = context.GetRouteValue("action") ?? "";
                        string message = Cookie.Cookies(context.Request, context.Response).HtmlTag("div", "container alert alert-danger");

                        //                    var button = "<a class=\"btn btn-danger\" href =\"/Cookies/Read\">Doc Cookies</a><br/><a class=\"btn btn-success\" href=\"/Cookies/Write\">Ghi Cookies</a><a class =\"btn btn-success\" href =\"/Cookies/Delete\">Xóa Cookie</a>"
                        var button = ("Doc Cookies".a("/Admin/Cookies/Read", "btn btn-danger")
                                     + "<br/>" + "Ghi Cookies".a("/Admin/Cookies/Write", "btn btn-success")
                                     + "<br/>" + "Xoa Cookies".a("/Admin/Cookies/Delete", "btn btn-success"))
                        .HtmlTag("div", "container mt-4");

                        string html = HtmlHelper.HtmlDocument("Cookies : " + action, menu + button + message);


                        await context.Response.WriteAsync(html);
                    });
                    endpoints.MapGet("/Encoding", async (context) => {
                        await context.Response.WriteAsync("Encoding");
                    });
                    endpoints.MapGet("/Json", async (context) => {
                        var ob = new
                        {
                            TenSp = "DongHo",
                            DonGia = 50000,
                            NgaySx = DateTime.Now
                        };
                        context.Response.ContentType = "application/json";
                        var json = JsonConvert.SerializeObject(ob);

                        await context.Response.WriteAsync(json);
                    });
                    endpoints.MapMethods("/Form", new[] { "GET", "POST" }, async (context) => {

                        var menu = HtmlHelper.MenuTop(
                               HtmlHelper.DefaultMenuTopItems(),
                                context.Request
                            );

                        var objectClass = RequestForm.ProcessForm(context.Request);
                        var htmlForm = File.ReadAllText("formtext.html").HtmlTag("div", "container");
                        var formatform = string.Format(htmlForm, objectClass.hovaten, objectClass.email, objectClass.luachon ? "checked" : "");
                        var jsonString = JsonConvert.SerializeObject(objectClass).HtmlTag("div", "container");
                        string html = HtmlHelper.HtmlDocument("RequestInfo", menu + formatform + jsonString);
                        await context.Response.WriteAsync(html);
                    });

                    endpoints.MapMethods("/DisPlay", new[] { "GET", "POST" }, async (context) => {

                        var objectClass = RequestForm.ProcessForm(context.Request);
                        var c = JsonConvert.SerializeObject(objectClass);
                        await context.Response.WriteAsync(c);
                    });

                    
                });

                

                
            });

            
           
        }
    }
}
