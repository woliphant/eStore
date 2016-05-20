using Microsoft.AspNet.Mvc;
using eStore.Models;
using eStore.ViewModels;

namespace eStore.Controllers
{
    public class BrandController : Controller
    {
        AppDbContext _db;
        public BrandController(AppDbContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            BrandModel model = new BrandModel(_db);
            BrandViewModel viewModel = new BrandViewModel();
            viewModel.Brands = model.GetAll();
            return View(viewModel);
        }
    }
}