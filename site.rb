['rubygems', 'sinatra', 'haml', 'couchrest'].each {|gem| require gem}
require 'sinatra/reloader' if development?
require 'newrelic_rpm' if production?

if ENV['CLOUDANT_URL']
  set :db, CouchRest.database!( ENV['CLOUDANT_URL'] + '/htteapot' )
else
  set :db, CouchRest.database!( 'http://localhost:5984/htteapot' )
end

helpers do
  include Rack::Utils

  def h(source)
    escape_html(source).gsub(' ', '%20')
  end

end

set :haml, :format => :html5

def processStatusCode()
	code = params[:captures].first
	statuses = options.db.view('status/by_status', :key => code)
	if(statuses['rows'].length === 1)
		status = statuses['rows'].first['value']
		headers = { "Content-Type" => "text/plain", "Content-Length" => "42" }
		if(status['headers'])
			customHeaders = status['headers']
			customHeaders.each {|key, value| headers[key] = value }
		end
		if(status['exclude'])
			status['exclude'].each {|key| headers[key] = nil }
		end
		bodyText = status['excludeBody'] ? nil : "#{code} #{status['description']}"
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
	statuses = options.db.view('status/by_status')
	haml :index, :locals => {:statuses => statuses['rows']}
end

not_found do
	haml :not_found
end