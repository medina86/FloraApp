using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraApp.Model.SearchObjects
{
    public class ProductSearchObject : BaseSearchObject
    {
        public string? Code { get; set; }
        public string? CodeGTE { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
