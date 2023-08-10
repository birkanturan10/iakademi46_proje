using Microsoft.EntityFrameworkCore;

namespace iakademi46_proje.Models
{
    public class cls_Category
    {
        iakademi46Context context = new iakademi46Context();

        #region MyRegion
        //bütün kategori listesi
        #endregion
        public async Task<List<Category>> CategorySelect()
        {
            List<Category> categories = await context.Categories.ToListAsync();
            return categories;
        }


        #region MyRegion
        //sadece ana kategori listesi
        #endregion
        public List<Category> CategorySelectMain()
        {
            List<Category> categories = context.Categories.Where(c => c.ParentID == 0).ToList();
            return categories;
        }

        public static bool CategoryInsert(Category category)
        {
            //metod static oldugu icin , context direk gelmez
            using (iakademi46Context context = new iakademi46Context())
            {
                try
                {
                    context.Add(category);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public async Task<Category> CategoryDetails(int? id)
        {
            Category? category = await context.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);
            return category;
        }


        public static bool CategoryUpdate(Category category)
        {
            //metod static oldugu icin , context direk gelmez
            using (iakademi46Context context = new iakademi46Context())
            {
                try
                {
                    context.Update(category);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }



        public static bool CategoryDelete(int id)
        {
            //metod static oldugu icin , context direk gelmez
            using (iakademi46Context context = new iakademi46Context())
            {
                try
                {
                    Category category = context.Categories.FirstOrDefault(c => c.CategoryID == id);
                    category.Active = false;

                    //alt kategorileride false yapıyoruz
                    List<Category> categories = context.Categories.Where(c => c.ParentID == id).ToList();

                    foreach (var item in categories)
                    {
                        item.Active = false;
                    }

                    context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }


    }
}
