using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Util.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class AjaxResultMessage
    {
        /// <summary>
        /// The status200 ok
        /// </summary>
        public const string Status200OK = "成功";
        /// <summary>
        /// The status400 bad request
        /// </summary>
        public const string Status400BadRequest = "错误的请求";
        /// <summary>
        /// The status401 unauthorized
        /// </summary>
        public const string Status401Unauthorized = "没有登录状态";
        /// <summary>
        /// The status403 forbidden
        /// </summary>
        public const string Status403Forbidden = "缺少权限";
        /// <summary>
        /// The status404 not found
        /// </summary>
        public const string Status404NotFound = "找不到资源";
        /// <summary>
        /// The status409 conflict
        /// </summary>
        public const string Status409Conflict = "存在冲突";
        /// <summary>
        /// The status500 internal server error
        /// </summary>
        public const string Status500InternalServerError = "服务器错误";
    }

    //
    // 摘要:
    //     A collection of constants for HTTP status codes. Status Codes listed at http://www.iana.org/assignments/http-status-codes/http-status-codes.xhtml
    /// <summary>
    /// 
    /// </summary>
    public static class StatusCodes
    {
        //
        // 摘要:
        //     HTTP status code 100.
        /// <summary>
        /// The status100 continue
        /// </summary>
        public const int Status100Continue = 100;
        //
        // 摘要:
        //     HTTP status code 412.
        /// <summary>
        /// The status412 precondition failed
        /// </summary>
        public const int Status412PreconditionFailed = 412;
        //
        // 摘要:
        //     HTTP status code 413.
        /// <summary>
        /// The status413 request entity too large
        /// </summary>
        public const int Status413RequestEntityTooLarge = 413;
        //
        // 摘要:
        //     HTTP status code 413.
        /// <summary>
        /// The status413 payload too large
        /// </summary>
        public const int Status413PayloadTooLarge = 413;
        //
        // 摘要:
        //     HTTP status code 414.
        /// <summary>
        /// The status414 request URI too long
        /// </summary>
        public const int Status414RequestUriTooLong = 414;
        //
        // 摘要:
        //     HTTP status code 414.
        /// <summary>
        /// The status414 URI too long
        /// </summary>
        public const int Status414UriTooLong = 414;
        //
        // 摘要:
        //     HTTP status code 415.
        /// <summary>
        /// The status415 unsupported media type
        /// </summary>
        public const int Status415UnsupportedMediaType = 415;
        //
        // 摘要:
        //     HTTP status code 416.
        /// <summary>
        /// The status416 requested range not satisfiable
        /// </summary>
        public const int Status416RequestedRangeNotSatisfiable = 416;
        //
        // 摘要:
        //     HTTP status code 416.
        /// <summary>
        /// The status416 range not satisfiable
        /// </summary>
        public const int Status416RangeNotSatisfiable = 416;
        //
        // 摘要:
        //     HTTP status code 417.
        /// <summary>
        /// The status417 expectation failed
        /// </summary>
        public const int Status417ExpectationFailed = 417;
        //
        // 摘要:
        //     HTTP status code 418.
        /// <summary>
        /// The status418 im a teapot
        /// </summary>
        public const int Status418ImATeapot = 418;
        //
        // 摘要:
        //     HTTP status code 419.
        /// <summary>
        /// The status419 authentication timeout
        /// </summary>
        public const int Status419AuthenticationTimeout = 419;
        //
        // 摘要:
        //     HTTP status code 422.
        /// <summary>
        /// The status421 misdirected request
        /// </summary>
        public const int Status421MisdirectedRequest = 421;
        //
        // 摘要:
        //     HTTP status code 422.
        /// <summary>
        /// The status422 unprocessable entity
        /// </summary>
        public const int Status422UnprocessableEntity = 422;
        //
        // 摘要:
        //     HTTP status code 423.
        /// <summary>
        /// The status423 locked
        /// </summary>
        public const int Status423Locked = 423;
        //
        // 摘要:
        //     HTTP status code 424.
        /// <summary>
        /// The status424 failed dependency
        /// </summary>
        public const int Status424FailedDependency = 424;
        //
        // 摘要:
        //     HTTP status code 426.
        /// <summary>
        /// The status426 upgrade required
        /// </summary>
        public const int Status426UpgradeRequired = 426;
        //
        // 摘要:
        //     HTTP status code 428.
        /// <summary>
        /// The status428 precondition required
        /// </summary>
        public const int Status428PreconditionRequired = 428;
        //
        // 摘要:
        //     HTTP status code 429.
        /// <summary>
        /// The status429 too many requests
        /// </summary>
        public const int Status429TooManyRequests = 429;
        //
        // 摘要:
        //     HTTP status code 431.
        /// <summary>
        /// The status431 request header fields too large
        /// </summary>
        public const int Status431RequestHeaderFieldsTooLarge = 431;
        //
        // 摘要:
        //     HTTP status code 451.
        /// <summary>
        /// The status451 unavailable for legal reasons
        /// </summary>
        public const int Status451UnavailableForLegalReasons = 451;
        //
        // 摘要:
        //     HTTP status code 500.
        /// <summary>
        /// The status500 internal server error
        /// </summary>
        public const int Status500InternalServerError = 500;
        //
        // 摘要:
        //     HTTP status code 501.
        /// <summary>
        /// The status501 not implemented
        /// </summary>
        public const int Status501NotImplemented = 501;
        //
        // 摘要:
        //     HTTP status code 502.
        /// <summary>
        /// The status502 bad gateway
        /// </summary>
        public const int Status502BadGateway = 502;
        //
        // 摘要:
        //     HTTP status code 503.
        /// <summary>
        /// The status503 service unavailable
        /// </summary>
        public const int Status503ServiceUnavailable = 503;
        //
        // 摘要:
        //     HTTP status code 504.
        /// <summary>
        /// The status504 gateway timeout
        /// </summary>
        public const int Status504GatewayTimeout = 504;
        //
        // 摘要:
        //     HTTP status code 505.
        /// <summary>
        /// The status505 HTTP version notsupported
        /// </summary>
        public const int Status505HttpVersionNotsupported = 505;
        //
        // 摘要:
        //     HTTP status code 506.
        /// <summary>
        /// The status506 variant also negotiates
        /// </summary>
        public const int Status506VariantAlsoNegotiates = 506;
        //
        // 摘要:
        //     HTTP status code 507.
        /// <summary>
        /// The status507 insufficient storage
        /// </summary>
        public const int Status507InsufficientStorage = 507;
        //
        // 摘要:
        //     HTTP status code 508.
        /// <summary>
        /// The status508 loop detected
        /// </summary>
        public const int Status508LoopDetected = 508;
        //
        // 摘要:
        //     HTTP status code 411.
        /// <summary>
        /// The status411 length required
        /// </summary>
        public const int Status411LengthRequired = 411;
        //
        // 摘要:
        //     HTTP status code 510.
        /// <summary>
        /// The status510 not extended
        /// </summary>
        public const int Status510NotExtended = 510;
        //
        // 摘要:
        //     HTTP status code 410.
        /// <summary>
        /// The status410 gone
        /// </summary>
        public const int Status410Gone = 410;
        //
        // 摘要:
        //     HTTP status code 408.
        /// <summary>
        /// The status408 request timeout
        /// </summary>
        public const int Status408RequestTimeout = 408;
        //
        // 摘要:
        //     HTTP status code 101.
        /// <summary>
        /// The status101 switching protocols
        /// </summary>
        public const int Status101SwitchingProtocols = 101;
        //
        // 摘要:
        //     HTTP status code 102.
        /// <summary>
        /// The status102 processing
        /// </summary>
        public const int Status102Processing = 102;
        //
        // 摘要:
        //     HTTP status code 200.
        /// <summary>
        /// The status200 ok
        /// </summary>
        public const int Status200OK = 200;
        //
        // 摘要:
        //     HTTP status code 201.
        /// <summary>
        /// The status201 created
        /// </summary>
        public const int Status201Created = 201;
        //
        // 摘要:
        //     HTTP status code 202.
        /// <summary>
        /// The status202 accepted
        /// </summary>
        public const int Status202Accepted = 202;
        //
        // 摘要:
        //     HTTP status code 203.
        /// <summary>
        /// The status203 non authoritative
        /// </summary>
        public const int Status203NonAuthoritative = 203;
        //
        // 摘要:
        //     HTTP status code 204.
        /// <summary>
        /// The status204 no content
        /// </summary>
        public const int Status204NoContent = 204;
        //
        // 摘要:
        //     HTTP status code 205.
        /// <summary>
        /// The status205 reset content
        /// </summary>
        public const int Status205ResetContent = 205;
        //
        // 摘要:
        //     HTTP status code 206.
        /// <summary>
        /// The status206 partial content
        /// </summary>
        public const int Status206PartialContent = 206;
        //
        // 摘要:
        //     HTTP status code 207.
        /// <summary>
        /// The status207 multi status
        /// </summary>
        public const int Status207MultiStatus = 207;
        //
        // 摘要:
        //     HTTP status code 208.
        /// <summary>
        /// The status208 already reported
        /// </summary>
        public const int Status208AlreadyReported = 208;
        //
        // 摘要:
        //     HTTP status code 226.
        /// <summary>
        /// The status226 im used
        /// </summary>
        public const int Status226IMUsed = 226;
        //
        // 摘要:
        //     HTTP status code 300.
        /// <summary>
        /// The status300 multiple choices
        /// </summary>
        public const int Status300MultipleChoices = 300;
        //
        // 摘要:
        //     HTTP status code 301.
        /// <summary>
        /// The status301 moved permanently
        /// </summary>
        public const int Status301MovedPermanently = 301;
        //
        // 摘要:
        //     HTTP status code 302.
        /// <summary>
        /// The status302 found
        /// </summary>
        public const int Status302Found = 302;
        //
        // 摘要:
        //     HTTP status code 303.
        /// <summary>
        /// The status303 see other
        /// </summary>
        public const int Status303SeeOther = 303;
        //
        // 摘要:
        //     HTTP status code 304.
        /// <summary>
        /// The status304 not modified
        /// </summary>
        public const int Status304NotModified = 304;
        //
        // 摘要:
        //     HTTP status code 305.
        /// <summary>
        /// The status305 use proxy
        /// </summary>
        public const int Status305UseProxy = 305;
        //
        // 摘要:
        //     HTTP status code 306.
        /// <summary>
        /// The status306 switch proxy
        /// </summary>
        public const int Status306SwitchProxy = 306;
        //
        // 摘要:
        //     HTTP status code 307.
        /// <summary>
        /// The status307 temporary redirect
        /// </summary>
        public const int Status307TemporaryRedirect = 307;
        //
        // 摘要:
        //     HTTP status code 308.
        /// <summary>
        /// The status308 permanent redirect
        /// </summary>
        public const int Status308PermanentRedirect = 308;
        //
        // 摘要:
        //     HTTP status code 400.
        /// <summary>
        /// The status400 bad request
        /// </summary>
        public const int Status400BadRequest = 400;
        //
        // 摘要:
        //     HTTP status code 401.
        /// <summary>
        /// The status401 unauthorized
        /// </summary>
        public const int Status401Unauthorized = 401;
        //
        // 摘要:
        //     HTTP status code 402.
        /// <summary>
        /// The status402 payment required
        /// </summary>
        public const int Status402PaymentRequired = 402;
        //
        // 摘要:
        //     HTTP status code 403.
        /// <summary>
        /// The status403 forbidden
        /// </summary>
        public const int Status403Forbidden = 403;
        //
        // 摘要:
        //     HTTP status code 404.
        /// <summary>
        /// The status404 not found
        /// </summary>
        public const int Status404NotFound = 404;
        //
        // 摘要:
        //     HTTP status code 405.
        /// <summary>
        /// The status405 method not allowed
        /// </summary>
        public const int Status405MethodNotAllowed = 405;
        //
        // 摘要:
        //     HTTP status code 406.
        /// <summary>
        /// The status406 not acceptable
        /// </summary>
        public const int Status406NotAcceptable = 406;
        //
        // 摘要:
        //     HTTP status code 407.
        /// <summary>
        /// The status407 proxy authentication required
        /// </summary>
        public const int Status407ProxyAuthenticationRequired = 407;
        //
        // 摘要:
        //     HTTP status code 409.
        /// <summary>
        /// The status409 conflict
        /// </summary>
        public const int Status409Conflict = 409;
        //
        // 摘要:
        //     HTTP status code 511.
        /// <summary>
        /// The status511 network authentication required
        /// </summary>
        public const int Status511NetworkAuthenticationRequired = 511;
    }
}
