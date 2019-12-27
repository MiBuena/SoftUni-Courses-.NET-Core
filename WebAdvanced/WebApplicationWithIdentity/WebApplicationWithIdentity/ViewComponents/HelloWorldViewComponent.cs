using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationWithIdentity.Services;

namespace WebApplicationWithIdentity.ViewComponents
{
    public class HelloWorldViewComponent : ViewComponent
    {

        private readonly IDataService dataService;

        public HelloWorldViewComponent(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public IViewComponentResult Invoke()
        {
            var a = new ComponentModel()
            {
                Name = "aa"
            };

            return View(a);
        }
    }

    public class ComponentModel
    {
        public string Name { get; set; }
    }
}


