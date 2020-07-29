using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Linq;

namespace Yaeher
{
    public class ListViewModel<T>:Pageable<T>
    {
        ConcurrentDictionary<Guid, Action<ListViewModel<T>>> Makers = new ConcurrentDictionary<Guid, Action<ListViewModel<T>>>();
        public string SortBy { get; set; }
        public string SortDirection { get; set; }

        public void MakeIt()
        {
            var type = this.GetType();
            var props = type.GetProperties();
            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttribute<FieldFilterAttribute>();
                if (attr == null) continue;
                var fieldnames = attr.Fieldnames.Length == 0 ? new string[] { prop.Name } : attr.Fieldnames;
                foreach (var fieldname in fieldnames)
                {

                }
            }
        }
    }
}
