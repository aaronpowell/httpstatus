namespace Teapot.Web.Models
{
    /// <summary>
    /// The list of status codes not listed in StatusCodes in .net core
    /// </summary>
    public class StatusCodesAddl
    {
        public const int Status100Continue = 100;
        public const int Status101SwitchingProtocols = 101;
        public const int Status102Processing = 102;
        public const int Status208AlreadyReported = 208;
        public const int Status226IMUsed = 226;
        public const int Status421MisdirectRequest = 421;
        public const int Status426UpgradeRequired = 426;
        public const int Status428PreConditionRequired = 428;
        public const int Status431RequestHeaderFieldsTooLarge = 431;
        public const int Status444ConnectionClosedWithoutResponse = 444;
        public const int Status499ClientClosed = 499;
        public const int Status508LoopDetected = 508;
        public const int Status510NotExtended = 510;
        public const int Status511NetworkAuthenticationRequired = 511;
        public const int Status520WebServerIsReturningAnUnknownError = 520;
        public const int Status522ConnectionTimedOut = 522;
        public const int Status524ATimeoutOccured = 524;
        public const int Status599NetworkConnectTimeoutError = 599;
    }
}
