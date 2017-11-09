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
        public override string Message => "We cant find the Login ID";

    }

    public class UserPwdErrorException : UserloginException
    {
        public override string Message => "Password error";
    }

    public class UserLoginPermissionException : UserloginException
    {
        public override string Message => "User Permission error";
    }

}
