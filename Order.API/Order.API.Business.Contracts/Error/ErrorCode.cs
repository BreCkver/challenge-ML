using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.API.Business.Contracts.Error
{
    public static class ErrorCode
    {
        public const string API_FAILED = "400_0001";
        public const string REQUEST_NULL = "400_0002";
        public const string REQUEST_EMPTY = "400_0003";
        public const string USERNAME_EXISTS = "409_004";
        public const string PASSWORD_DIFFERENT = "400_0005";
        public const string USERNAME_NOEXISTS = "401_006";
        public const string EXTERNALAPI_EMPTY = "400_0007";
    }
}
