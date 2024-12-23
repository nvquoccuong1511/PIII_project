using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIII_Project_RestaurantApp.Models
{
    internal interface ISalesManagement
    {
        decimal GetDailySales(DateTime date);
        decimal GetTotalSales(DateTime startDate, DateTime endDate);
        

    }
}
