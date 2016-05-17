using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FreeBay.Data;

namespace FreeBay.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<int> MyIds { get; set; } 
    }
}