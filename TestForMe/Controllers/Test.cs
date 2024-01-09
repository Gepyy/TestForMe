using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using TestForMe.Models;

namespace TestForMe.Controllers
{
   
    [ApiController]
    public class Test : Controller
    {
        private static List<Groups> collection = new List<Groups>();
        [HttpGet("/index")]
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }
        [HttpGet("/create")]
        public IActionResult CallCreate()
        {
            return View("CreateCompany");
        }
        [HttpPost("/create")]
        public IActionResult CreateCompany([FromForm]Groups group)
        {
            if (ModelState.IsValid)
            {
                if(collection.FindIndex(x => x.Id == group.Id) == -1)
                {
                    collection.Add(group);
                    return RedirectToAction("List");
                }
                else
                {
                    return View("CreateCompany", group);
                }
            }
            //// Log ModelState errors for debugging
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Error: {error.ErrorMessage}");
            }
            //// If ModelState is not valid, return to the same view with validation errors
            return View("CreateCompany", group);
        }
        [HttpGet("/edit/{id:int}")]
        public IActionResult CallEdit(int id)
        {
            int index = collection.FindIndex(x => x.Id == id);
            return View("EditCompany", collection[index]);
        }
        //[HttpPut("/put/{id:int}")]
        //public IActionResult EditCompany(int id, [FromBody]Groups group)
        //{

        //    if (id >= 0 && id < collection.Count)
        //    {
        //        var obj = collection[id];
        //        obj.NameofCompany = group.NameofCompany;
        //        obj.Description = group.Description;
        //        obj.Owner = group.Owner;

        //        collection[id] = obj;
        //    }
        //    return RedirectToAction("List");
        //}
        //
        [HttpPost("/editcomplete")]
        public IActionResult EditCompany([FromForm] Groups group)
        {
            int index = collection.FindIndex(x => x.Id == group.Id);

            if (index >= 0 && index < collection.Count)
            {
                var obj = collection[index];
                obj.NameofCompany = group.NameofCompany;
                obj.Description = group.Description;
                obj.Owner = group.Owner;

                collection[index] = obj;
            }

            return RedirectToAction("List");
        }
        [HttpGet("/details/{id:int}")]
        public IActionResult CallDetails(int id)
        {
            int index = collection.FindIndex(x => x.Id == id);
            return View("DetailsCompany", collection[index]);
        }
        [HttpGet("/delete/{id:int}")]
        public IActionResult CallDelete(int id)
        {
            int index = collection.FindIndex(x => x.Id == id);
            return View("DeleteCompany", collection[index]);
        }
        //[HttpDelete("/deletecomplete/{id:int}")]
        //public IActionResult DeleteCompany(int id)
        //{
        //    collection.RemoveAt(id);
        //    return RedirectToAction("List");
        //}
        [HttpPost("/deletecomplete")]
        public IActionResult DeleteCompany([FromForm] Groups g)
        {
            int index = collection.FindIndex(x => x.Id == g.Id);
            collection.RemoveAt(index);
            return RedirectToAction("List");
        }
        [HttpGet("/list")]
        public IActionResult List()
        {
            return View("ListOfCompany", collection);
        }
    }
}
