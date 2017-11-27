using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels.AdViewModels
{
    public class Pager
    {
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public string PageUrl { get; set; }

        public Pager(int totalItems, int? currentPageIndex, int pageSize)
        {
            this.TotalItems = totalItems;
            this.TotalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);
            this.PageSize = pageSize;
            this.CurrentPage = currentPageIndex != null ? currentPageIndex.Value : 1;
            SetStartEndPage();
        }

        private void SetStartEndPage()
        {
            var startPage = this.CurrentPage - 2;
            var endPage = this.CurrentPage + 4;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > this.TotalPages)
            {
                endPage = this.TotalPages;
                if (endPage > 7)
                {
                    startPage = endPage - 6;
                }
            }

            this.StartPage = startPage;
            this.EndPage = endPage;
        }
    }
}

