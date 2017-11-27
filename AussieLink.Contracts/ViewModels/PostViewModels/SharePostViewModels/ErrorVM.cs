using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels.PostViewModels.SharePostViewModels
{
    public class ErrorVM
    {
        public string Key { get; set; }
        public string ErrorMsg { get; set; }

        public ErrorVM() { }
        public ErrorVM(string key, string errorMsg)
        {
            this.Key = key;
            this.ErrorMsg = errorMsg;
        }
    }
}
