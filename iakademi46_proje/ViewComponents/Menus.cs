using Microsoft.AspNetCore.Mvc;
using iakademi46_proje.Models;

namespace iakademi46_proje.ViewComponents
{
    public class Menus : ViewComponent
    {

        iakademi46Context context = new iakademi46Context();

        public IViewComponentResult Invoke()
        {
            List<Category> categories = context.Categories.ToList();
            return View(categories);
        }

    }
}
