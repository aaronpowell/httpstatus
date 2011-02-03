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

get %r{/\d{3}} do
	"You hit a status"
end

get '/' do
	haml :index
end