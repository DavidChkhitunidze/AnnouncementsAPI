using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnnouncementsAPI.Helpers
{
    public class AnnouncementsResourceParameters
    {
        const int maxPageSize = 4;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 4;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }

        public string SearchString { get; set; }

        public string Fields { get; set; }
    }
}
