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

get %r{/(\d{3})} do
	code = params[:captures].first
	statuses = options.db.view('status/by_status', :key => code)
	if(statuses['rows'].length === 1)
		status = statuses['rows'].first['value']
		return code.to_i, "#{code} #{status['description']}"
	else
		return 652, {'Content-Type' => 'text/plain', 'Content-Length' => '18'}, "652 Unknown Status"
	end
end

get '/' do
	statuses = options.db.view('status/by_status')
	haml :index, :locals => {:statuses => statuses}
end