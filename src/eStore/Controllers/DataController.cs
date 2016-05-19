using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using eStore.Models;
using System.Net.Http;

namespace eStore.Controllers
{
    public class DataController : Controller
    {
        AppDbContext _db;
        public DataController(AppDbContext context)
        {
            _db = context;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Json()
        {
            ProductModel model = new ProductModel(_db);
            string rawJsonString = await getMenuItemJsonFromWeb();
            bool categoriesLoaded = model.loadBrands(rawJsonString);
            bool menuItemsLoaded = model.loadProducts(rawJsonString);
            if (categoriesLoaded && menuItemsLoaded)
                ViewBag.JsonLoadedMsg = "Json Loaded Successfully";
            else
                ViewBag.JsonLoadedMsg = "Json NOT Loaded";
            return View("Index");
        }
        private async Task<String> getMenuItemJsonFromWeb()
        {
            string url = "https://raw.githubusercontent.com/woliphant/eStore/master/src/eStore/JsonData/casestudy.json";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}