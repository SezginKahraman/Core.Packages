using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistance.Dynamic
{
    public class Filter
    {
        public Filter()
        {
            Field = string.Empty;
            Operator = string.Empty;
        }

        public Filter(string field, string @operator)
        {
            Field = field;
            Operator = @operator;
        }

        public string Field { get; set; } // brand

        public string? Value { get; set; } // categoryId = 5

        public string Operator { get; set; } // = 

        public string? Logic { get; set; } // && , || etc.

        public IEnumerable<Filter> Filters { get; set; }
    }
}
