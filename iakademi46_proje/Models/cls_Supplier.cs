using Microsoft.EntityFrameworkCore;

namespace iakademi46_proje.Models
{
    public class cls_Supplier
    {
        iakademi46Context context = new iakademi46Context();

        public async Task<List<Supplier>> SupplierSelect()
        {
            List<Supplier> suppliers = await context.Suppliers.ToListAsync();
            return suppliers;
        }

        public static bool SupplierInsert(Supplier supplier)
        {
            using (iakademi46Context context = new iakademi46Context())
            {
                try
                {
                    context.Add(supplier);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public async Task<Supplier> SupplierDetails(int? id)
        {
            Supplier? supplier = await context.Suppliers.FirstOrDefaultAsync(c => c.SupplierID == id);
            return supplier;
        }

        public static bool SupplierUpdate(Supplier supplier)
        {
            try
            {
                //metod static oldugu icin , context burada tanımlamak zorundayım
                using (iakademi46Context context = new iakademi46Context())
                {
                    context.Update(supplier);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool SupplierDelete(int id)
        {
            //metod static oldugu icin , context direk gelmez
            using (iakademi46Context context = new iakademi46Context())
            {
                try
                {
                    Supplier? supplier = context.Suppliers.FirstOrDefault(c => c.SupplierID == id);
                    supplier.Active = false;

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
