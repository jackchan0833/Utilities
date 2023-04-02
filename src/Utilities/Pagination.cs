using System;
using System.Collections.Generic;
using System.Text;

namespace JC.Utilities
{
    /// <summary>
    /// Represents the pagination.
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// Gets the page number list.
        /// </summary>
        /// <param name="totalRecordCount">The total count of records.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="currentPageNo">Current page index, the value is start from 1.</param>
        /// <param name="maxPageNumbers">The page number to display for grouping.</param>
        /// <returns>The page number list.</returns>
        public static List<int> GetPageNumbers(int totalRecordCount, int pageSize, int currentPageNo, int maxPageNumbers = 5)
        {
            List<int> pageNumbers = new List<int>();
            if (pageSize == 0)
            {
                return pageNumbers;
            }
            int totalPageCount = (totalRecordCount / pageSize) + ((totalRecordCount % pageSize) == 0 ? 0 : 1);
            int pageGroupIndex = (currentPageNo / maxPageNumbers) + ((currentPageNo % maxPageNumbers) == 0 ? 0 : 1);
            if (pageGroupIndex > 0)
            {
                int startPageNumber = (pageGroupIndex - 1) * maxPageNumbers + 1;
                int endPageNumber = startPageNumber + maxPageNumbers - 1;
                if (endPageNumber > totalPageCount)
                {
                    endPageNumber = totalPageCount;
                }
                for (int i = startPageNumber; i <= endPageNumber; i++)
                {
                    pageNumbers.Add(i);
                }
            }
            return pageNumbers;
        }
    }
}
