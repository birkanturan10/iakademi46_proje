using Microsoft.EntityFrameworkCore;

namespace iakademi46_proje.Models
{
    public class cls_Status
    {
        iakademi46Context context = new iakademi46Context();

        public async Task<List<Status>> StatusSelect()
        {
            List<Status> statuses = await context.Statuses.ToListAsync();
            return statuses;
        }


        public static bool StatusInsert(Status status)
        {
            using (iakademi46Context context = new iakademi46Context())
            {
                try
                {
                    context.Add(status);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }


        public async Task<Status> StatusDetails(int? id)
        {
            Status? status = await context.Statuses.FirstOrDefaultAsync(c => c.StatusID == id);
            return status;
        }

        public static bool StatusUpdate(Status status)
        {
            try
            {
                //metod static oldugu icin , context burada tanımlamak zorundayım
                using (iakademi46Context context = new iakademi46Context())
                {
                    context.Update(status);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool StatusDelete(int id)
        {
            //metod static oldugu icin , context direk gelmez
            using (iakademi46Context context = new iakademi46Context())
            {
                try
                {
                    Status? status = context.Statuses.FirstOrDefault(c => c.StatusID == id);
                    status.Active = false;

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
