using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher
{
    public class FieldFilterAttribute : Attribute
    {
        public FieldFilterAttribute(FieldFilterTypes type, params string[] fieldnames) {
            this.FilterType = type;
            this.Fieldnames = fieldnames;
        }

        public FieldFilterTypes FilterType { get;private set; }
        public string[] Fieldnames {get;private set;}
    }
}
