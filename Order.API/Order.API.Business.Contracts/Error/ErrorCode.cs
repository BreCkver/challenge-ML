
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
        public const string WISHLIST_EXISTS = "400_008";
        public const string USER_NOEXISTS = "400_009";
        public const string WISHLIST_NOEXISTS = "400_010";
        public const string PRODUCT_EXISTS = "400_011";
        public const string PRODUCT_NOEXISTS = "400_012";
        public const string WISHLIST_EMPTY = "400_013";
        public const string REQUEST_NOALPHANUMERIC = "400_014";
    }
}
