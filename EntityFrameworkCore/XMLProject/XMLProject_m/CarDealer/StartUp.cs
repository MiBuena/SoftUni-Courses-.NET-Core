using CarDealer.Data;
using Microsoft.EntityFrameworkCore;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new CarDealerContext())
            {
                context.Database.Migrate();
            }
        }
    }
}