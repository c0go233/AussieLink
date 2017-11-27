using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AussieLink.Contracts.Enums
{
    public enum ErrorCode
    {


        [Description("This email is already registered")]
        DUPLICATEEMAIL = 0,

        [Description("Login fails try again")]
        LOGINFAIL = 1,

        [Description("We can't proccess your request for some reasons. Please try to use other social accounts")]
        SOCIALSIGNUPERROR = 2,

        [Description("Sorry, this email and password combination is not known. Please try again.")]
        NOEMAIL = 3,

        [Description("Sorry, this email and password combination is not known. Please try again.")]
        CANCELEDUSER = 4,

        [Description("Sorry, this email and password combination is not known. Please try again.")]
        PASSWORDNOTMATCH = 5,

        [Description("This email is not registred")]
        FORGOTPWDNOEMAIL = 6,

        [Description("Error occured")]
        COMMENTBADREQUEST = 7,

        [Description("Error occured")]
        MANAGEADBADREQUEST = 8,

        [Description("no error")]
        NOERROR,

        [Description("This account's email can be changed.")]
        CHANGEEMAILNOTALLOWED,

        [Description("Unknown error occured")]
        BADREQUEST
    }
}
