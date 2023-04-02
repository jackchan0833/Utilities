using System;
using System.Collections.Generic;
using System.Text;

namespace JC.Utilities
{
    /// <summary>
    /// Represents the search criteria.
    /// </summary>
    public class SearchCriteria
    {
        /// <summary>
        /// The page index for searching.
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// The page size for searching; Default value is 10.
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
