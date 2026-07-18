using System;
using SmartInventory.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Services
{
    public static class ProductService
    {
        public static readonly string[] Categories =
        { "電子", "生活", "文具", "食品" };

        public static (decimal,int) GetTotalValue(List<Product> all)
        {
            decimal total = 0;
            int qty = 0;

            foreach (Product product in all)
            {
                total += product.Price;
                qty += product.Quantity;
            }

            return (total,qty) ;
        }
        public static List<Product> GetLowStock(List<Product> all , int lowStack = 10)
        {
           

            var result = new List<Product>();

            foreach (var p in all)
            {
                if(p.Quantity < lowStack)
                {
                    result.Add(p);
                    //Console.WriteLine($"{p.Name} {p.Quantity}");
                }
            }

            return result;
        }

        public static List<Product> Search(List<Product> all ,string keyword, string category)
        {
            //1.判斷是否都為空字串
            if(keyword==string.Empty && category==string.Empty) return all;

            //2.是否搜尋關鍵字
            //3.是否是搜尋分類
            var result = new List<Product>();
            foreach(var p in all)
            {
                if(!p.Name.Contains(keyword)) continue;

                if(category == "全部" || p.Category.Contains(category))
                {
                    result.Add(p);
                }
                
            
            }

            //4.回傳篩選後的商品集合
            return result;
        }
    }
}

