using System;
using System.Collections.Generic;
using System.Text;

namespace JC.Utilities
{
    /// <summary>
    /// Represents the search result.
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// The page index, the value is start from 1.
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// The page size to search.
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// The total record count for searched result.
        /// </summary>
        public int TotalCount { get; set; }
    }
}
