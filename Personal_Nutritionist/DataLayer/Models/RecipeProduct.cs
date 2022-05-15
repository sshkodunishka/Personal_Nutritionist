using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Nutritionist.DataLayer
{
    public class RecipeProduct 
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
