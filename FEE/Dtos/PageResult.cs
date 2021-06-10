using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FEE.Dtos
{
    public class PageResult<T>
    {
        public List<T> ListItem { get; set; }
        public int TotalRecords { get; set; }
    }
}