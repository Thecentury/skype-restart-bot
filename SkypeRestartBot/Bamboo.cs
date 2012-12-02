using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SkypeRestartBot
{
	public sealed class Bamboo : IBamboo
	{
		private readonly string _bambooHost;
		private readonly int _port;
		private readonly string _login;
		private readonly string _password;
		private readonly string _api = "/rest/api/latest/";
		private readonly HttpClient _client;

		public Bamboo( string bambooHost, int port, string login, string password )
		{
			_bambooHost = bambooHost;
			_port = port;
			_login = login;
			_password = password;

			_client = new HttpClient();
			_client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue( "application/json" ) );
		}

		private string GetUri( string url )
		{
			string uri = "http://" + _bambooHost + ":" + _port + _api + url;
			string auth = String.Format( "os_authType=basic&os_username={0}&os_password={1}", _login, _password );
			if ( uri.Contains( "?" ) )
			{
				uri += String.Format( "&{0}", auth );
			}
			else
			{
				uri += "?" + auth;
			}
			return uri;
		}

		public async void GetQueue()
		{
			var uri = GetUri( "queue" );
			var response = await _client.GetAsync( uri );
			var content = await response.Content.ReadAsStringAsync();

			Console.WriteLine( content );
		}

		public async void ListProjects()
		{
			var uri = GetUri( "project" );
			var response = await _client.GetAsync( uri );
			var content = await response.Content.ReadAsStringAsync();

			Console.WriteLine( content );
		}

		public async void Queue( string projectKey, string buildKey, string stage )
		{
			var uri = GetUri( string.Format( "queue/{0}-{1}?stage={2}", projectKey, buildKey, stage ) );

			Console.WriteLine( uri );

			var response = await _client.PostAsync( uri, new FormUrlEncodedContent( Enumerable.Empty<KeyValuePair<string, string>>() ) );
			var content = await response.Content.ReadAsStringAsync();

			Console.WriteLine( content );
		}
	}
}