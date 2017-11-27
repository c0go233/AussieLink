using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Enums
{
    public enum Account
    {
        [Description("userId")]
        USERID
    }

    public enum SocialAccount
    {
        [Description("SignInFacebookCallBack")]
        FACEBOOKSIGNINCALLBACK,

        [Description("SignUpFacebookCallBack")]
        FACEBOOKSIGNUPCALLBACK,

        [Description("FacebookReturnUrl")]
        FACEBOOKRETURNURL,

        [Description("GoogleReturnUrl")]
        GOOGLERETURNURL,

        [Description("SignInGoogleCallBack")]
        GOOGLESIGNINCALLBACK,

        [Description("SignUpGoogleCallBack")]
        GOOGLESIGNUPCALLBACK,

        [Description("socialSignupError")]
        SOCIALSIGNUPERROR
    }

    public enum PostType
    {
        [Description("Job")]
        JOB,

        [Description("Share")]
        SHARE,

        [Description("Second Hand")]
        SECONDHAND
    }

    public enum Size
    {
        [Description("Maximum")]
        MAXIMUM,

        [Description("Minimum")]
        MINIMUM
    }

    public enum MyAccountMenu
    {
        [Description("Profile")]
        PROFILE,

        [Description("ManageAds")]
        MANAGEADS,

        [Description("Message")]
        MESSAGE
    }

    public enum PlaceId
    {
        Sydney = 1,
        Melbourne = 2,
        Brisbane = 3,
        Adelaide = 4,
        Perth = 5
    }
}
