using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapReqRep
{
   public class ObjectFrom
    {
        public string hovaten { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public bool luachon { get; set; }        
    }
    public static class RequestForm
    {
        public static ObjectFrom ProcessForm(HttpRequest request)
        {
            ObjectFrom form = null;
            var c = request.Method;
            if (request.Method == "POST")
            {
                IFormCollection _form = request.Form;
                form = new ObjectFrom()
                {
                    email = _form["email"].FirstOrDefault() ?? "",
                    hovaten =_form["hovaten"].FirstOrDefault()?? "",
                    password =_form["password"].FirstOrDefault()?? "",
                    luachon = (_form["luachon"].FirstOrDefault()== "on")
                    
                };
                 
            }
            
            return form ?? new ObjectFrom() {
                
                email = "",
                hovaten = "",
                password ="",
                luachon = false,
            
            } ;

        }
    }
}
