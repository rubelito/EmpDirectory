using System;
using System.Collections.Generic;

namespace BCS.Models
{
    public class LoginResult
    {
        public LoginResult()
        {
            ErrorMessages = new List<LoginErrorMessage>();
        }

        public bool Success { get; set; }
        public List<LoginErrorMessage> ErrorMessages { get; set; }
    }

    public class LoginErrorMessage
    {
        public string Header { get; set; }
        public string Message { get; set; }
    }

}
