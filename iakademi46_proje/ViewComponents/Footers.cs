using iakademi46_proje.Models;
using Microsoft.AspNetCore.Mvc;

namespace iakademi46_proje.ViewComponents
{
    public class Footers : ViewComponent
    {
        iakademi46Context context = new iakademi46Context();

        public IViewComponentResult Invoke()
        {
            List<Supplier> suppliers = context.Suppliers.ToList();
            return View(suppliers);
        }
    }
}
