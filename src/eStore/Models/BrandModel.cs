using System.Collections.Generic;
using System.Linq;

namespace eStore.Models
{
    public class BrandModel
    {
        private AppDbContext _db;
        public BrandModel(AppDbContext ctx)
        {
            _db = ctx;
        }
        public List<Brand> GetAll()
        {
            return _db.Brands.ToList<Brand>();
        }
        // Retrieves the name as well as the ID for catagoryModel
        public string GetName(int id)
        {
            Brand bra = _db.Brands.First(b => b.Id == id);
            return bra.Name;
        }
    }
}
