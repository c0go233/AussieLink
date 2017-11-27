using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.ViewModels
{
    public class GoogleAccessToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires { get; set; }
        public string id_token { get; set; }
        public string refresh_token { get; set; }
    }
}
