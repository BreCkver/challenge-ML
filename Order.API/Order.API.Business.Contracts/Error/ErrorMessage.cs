﻿
namespace Order.API.Business.Contracts.Error
{
    public static class ErrorMessage
    {
        public const string API_FAILED = "API call was not successful";
        public const string REQUEST_NULL = "Request has element(s) not defined";
        public const string REQUEST_EMPTY = "There are any elements without value assigned ";
        public const string USERNAME_EXISTS = "The username is already taken";
        public const string PASSWORD_DIFFERENT = "The password value is different to password confirm";
        public const string USERNAME_NOEXISTS = "The user provided invalid authentication credentials";
        public const string EXTERNALAPI_EMPTY = "The external search doesn't has any result";
        public const string WISHLIST_EXISTS = "The wishlist name is already taken";
        public const string USER_NOEXISTS = "The user provided no exists";
        public const string WISHLIST_NOEXISTS = "The WishList provided no exists";
    }
}
