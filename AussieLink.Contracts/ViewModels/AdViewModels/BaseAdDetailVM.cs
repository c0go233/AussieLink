using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels.AdViewModels
{
    public class BaseAdDetailVM
    {
        public string DeleteCompleteReturnUrl { get; private set; }
        public string EditReturnUrl { get; private set; }
        public bool IsPostOwned { get; set; }
        public bool NeedBackToList { get; private set; }
        public string CurrentPostId { get; set; }
        public string PostType { get; set; }

        public BaseAdDetailVM(string returnUrl, bool needBackToList, string postType)
        {
            NeedBackToList = needBackToList;
            PostType = postType;
            EditReturnUrl = returnUrl;

            //temporal fix.. need to fix architecture
            DeleteCompleteReturnUrl = needBackToList ? GetDeleteCompleteReturnUrl(returnUrl) : returnUrl;
        }

        private string GetDeleteCompleteReturnUrl(string returnUrl)
        {
            var questionSplit = returnUrl.Split('?');
            var ampersandSplit = questionSplit[1].Split('&');
            var deleteCompleteQueryString = questionSplit[0];

            for(int i = 0; i < ampersandSplit.Length; i++)
            {
                if (!ampersandSplit[i].Contains("_CurrentPostId"))
                {
                    if (i == 0)
                        deleteCompleteQueryString += "?" + ampersandSplit[i];
                    else
                        deleteCompleteQueryString += "&" + ampersandSplit[i];
                }
            }
            return  deleteCompleteQueryString;
        }
    }
}
