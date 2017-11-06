using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPServer.Exceptions
{
    public class UserloginException : Exception
    {
        public string LID { get; set; }

    }

    public class UserNotfindException : UserloginException
    {


    }

    public class UserPwdErrorException : UserloginException
    {

    }

}
