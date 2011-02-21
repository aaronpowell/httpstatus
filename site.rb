['rubygems', 'sinatra', 'haml'].each {|gem| require gem}
require 'sinatra/reloader' if development?
require 'newrelic_rpm' if production?

helpers do
  include Rack::Utils

  def h(source)
    escape_html(source).gsub(' ', '%20')
  end

end

set :haml, :format => :html5

$codes = { '200' => { 'status' => '200', 'description' => 'OK'  },
           '201' => { 'status' => '201', 'description' => 'Created' },
           '202' => { 'status' => '202', 'description' => 'Accepted' },
           '203' => { 'status' => '203', 'description' => 'Non-Authoritative Information' },
           '204' => { 'status' => '204', 'description' => 'No Content', 'exclude' => [ 'Content-Type', 'Content-Length'], 'excludeBody' => 'true' },
           '205' => { 'status' => '205', 'description' => 'Reset Content', 'exclude' => [ 'Content-Type', 'Content-Length'], 'excludeBody' => 'true' },
           '206' => { 'status' => '206',	'description' => 'Partial Content', 'headers' => { 'Content-Range' => '0-30' } },
           '300' => { 'status' => '300', 'description' => 'Multiple Choices' },
           '301' => { 'status' => '301', 'description' => 'Moved Permanently', 'headers' => { 'Location' => 'http://httpstat.us' } },
           '302' => { 'status' => '302', 'description' => 'Found', 'headers' => { 'Location' => 'http://httpstat.us' } },
           '303' => { 'status' => '303', 'description' => 'See Other', 'headers' => { 'Location' => 'http://httpstat.us' } },
           '304' => { 'status' => '304',	'description' => 'Not Modified', 'exclude' => [ 'Content-Type', 'Content-Length'], 'excludeBody' => 'true' },
           '305' => { 'status' => '305', 'description' => 'Use Proxy', 'headers' => { 'Location' => 'http://httpstat.us' } },
           '306' => { 'status' => '306', 'description' => 'Unused' },
           '307' => { 'status' => '307', 'description' => 'Temporary Redirect', 'headers' => { 'Location' => 'http://httpstat.us' } },
           '400' => { 'status' => '400', 'description' => 'Bad Request' },
           '401' => { 'status' => '401', 'description' => 'Unauthorized', 'headers' => { 'WWW-Authenticate' => 'Basic realm=\"Fake Realm\"' } },
           '402' => { 'status' => '402', 'description' => 'Payment Required' },
           '403' => { 'status' => '403', 'description' => 'Forbidden' },
           '404' => { 'status' => '404', 'description' => 'Not Found' },
           '405' => { 'status' => '405', 'description' => 'Method Not Allowed' },
           '406' => { 'status' => '406', 'description' => 'Not Acceptable' },
           '407' => { 'status' => '407', 'description' => 'Proxy Authentication Required', 'headers' => { 'Proxy-Authenticate' => 'Basic realm=\"Fake Realm\"' } },
           '408' => { 'status' => '408', 'description' => 'Request Timeout' },
           '409' => { 'status' => '409', 'description' => 'Conflict' },
           '410' => { 'status' => '410', 'description' => 'Gone' },
           '411' => { 'status' => '411', 'description' => 'Length Required' },
           '412' => { 'status' => '412', 'description' => 'Precondition Required' },
           '413' => { 'status' => '413', 'description' => 'Request Entity Too Large' },
           '414' => { 'status' => '414', 'description' => 'Request-URI Too Long' },
           '415' => { 'status' => '415', 'description' => 'Unsupported Media Type' },
           '416' => { 'status' => '416', 'description' => 'Requested Range Not Satisfiable' },
           '417' => { 'status' => '417', 'description' => 'Expectation Failed' },
           '418' => { 'status' => '418', 'description' => 'I\'m a teapot', 'link' => 'http://www.ietf.org/rfc/rfc2324.txt' },
           '419' => { 'status' => '500', 'description' => 'Internal Server Error' },
           '501' => { 'status' => '501', 'description' => 'Not Implemented' },
           '502' => { 'status' => '502', 'description' => 'Bad Gateway' },
           '503' => { 'status' => '503', 'description' => 'Service Unavailable' },
           '504' => { 'status' => '504', 'description' => 'Gateway Timeout' },
           '505' => { 'status' => '505', 'description' => 'HTTP Version Not Supported' } }

def processStatusCode()
	code = params[:captures].first
	status = $codes[code]

	unless(status.nil?)
		headers = { }
		if(status['headers'])
			customHeaders = status['headers']
			customHeaders.each {|key, value| headers[key] = value }
		end
		if (!status['excludeBody'])
			headerText = headers.keys.map {|k| "#{k}: #{headers[k]}"}.join("\r\n")
			if (headerText.length > 0)
				headerText = "\r\n\r\n#{headerText}"
			end
			bodyText = "#{code} #{status['description']}#{headerText}"
			headers["Content-Type"] = "text/plain"
		end
		headers["Access-Control-Allow-Origin"] = "*"
		return code.to_i, headers, bodyText
	else
		return code.to_i, "#{code} Unknown Status"
	end
end

get %r{/(\d{3})} do processStatusCode() end
post %r{/(\d{3})} do processStatusCode() end
put %r{/(\d{3})} do processStatusCode() end
delete %r{/(\d{3})} do processStatusCode() end

get '/im-a-teapot' do
	redirect 'http://www.ietf.org/rfc/rfc2324.txt'
end

get '/' do
		 haml :index, :locals => {:statuses => $codes}
end

not_found do
	haml :not_found
end