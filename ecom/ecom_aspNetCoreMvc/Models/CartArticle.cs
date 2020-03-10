using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecom_aspNetCoreMvc.Models
{
    public class CartArticle
    {
        private Article article;
        private int quantity;

        public Article Article { get => article; set => article = value; }
        public int Quantity { get => quantity; set => quantity = value; }


    }
}
