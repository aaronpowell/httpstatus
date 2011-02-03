Unless otherwise specified, all return:

    HTTP/1.1 {status code} {status description}
    Content-Type: text/plain
    Content-Length: {something}
    {any custom response headers}

    {status code} {status description}
    {list of any custom response headers we added}

For example:

    HTTP/1.1 307 Temporary Redirect
    Content-Type: text/plain
    Content-Length: 153
    Location: http://httpstat.us

    307 Temporary Redirect
    Location: http://httpstat.us

Here are all the codes we support (and any special responses):

100 Continue
101 Switching Protocols
    Upgrade: HTTP/1.1
    Connection: Upgrade
200 OK
201 Created
202 Accepted
203 Non-Authoritative Information
204 No Content
    Doesn't return a body, Content-Type or Content-Length header
205 Reset Content
    Doesn't return a body, Content-Type or Content-Length header
206 Partial Content
    Content-Range: 0-30
300 Multiple Choices
301 Moved Permanently
    Location: http://httpstat.us
302 Found
    Location: http://httpstat.us
303 See Other
    Location: http://httpstat.us
304 Not Modified
    Doesn't return a body, Content-Type or Content-Length header
305 Use Proxy
    Location: http://httpstat.us
306 Unused
307 Temporary Redirect
    Location: http://httpstat.us
400 Bad Request
401 Unauthorized
    WWW-Authenticate: Basic realm="Fake Realm"
402 Payment Required
403 Forbidden
404 Not Found
405 Method Not Allowed
406 Not Acceptable
407 Proxy Authentication Required
    Proxy-Authenticate: Basic realm="Fake Realm"
408 Request Timeout
409 Conflict
410 Gone
411 Length Required
412 Precondition Required
413 Request Entity Too Large
414 Request-URI Too Long
415 Unsupported Media Type
416 Requested Range Not Satisfiable
417 Expectation Failed
500 Internal Server Error
501 Not Implemented
502 Bad Gateway
503 Service Unavailable
504 Gateway Timeout
505 HTTP Version Not Supported

If you send any other three digit number that's not in that list, we'll return it too.

    HTTP/1.1 652 Unknown Status
    Content-Type: text/plain
    Content-Length: 153

    652 Unknown Status