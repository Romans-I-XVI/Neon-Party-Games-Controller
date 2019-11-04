using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NeonPartyGamesController
{
	public static class RokuECP
	{
		private static HttpClient _client = null;
		private static HttpClient Client {
			get {
				if (_client == null) {
					_client = new HttpClient();
					_client.Timeout = TimeSpan.FromMilliseconds(1000);
				}
				return _client;
			}
		}

		private static string GetURI(string roku_ip, string path = "") {
			return "http://" + roku_ip + ":8060/" + path;
		}

		public static async Task<bool> PingRoku(string roku_ip) {
			try {
				var response = await Client.GetAsync(GetURI(roku_ip));
				return response.IsSuccessStatusCode;
			} catch {
				return false;
			}
		}

		public static async Task<string> GetRokuName(string roku_ip) {
			string name = "";
			string res = null;

			try {
				byte[] response_bytes = await RokuECP.Client.GetByteArrayAsync("http://" + roku_ip + ":8060/");
				res = Encoding.ASCII.GetString(response_bytes, 0, response_bytes.Length - 1);
			} catch {}

			if (res != null) {
				try {
					int p_from = res.IndexOf("<friendlyName>") + "<friendlyName>".Length;
					int p_to = res.LastIndexOf("</friendlyName>");
					name = res.Substring(p_from, p_to - p_from);
				} catch {}

				if (name.Trim() == "") {
					try {
						int p_from = res.IndexOf("<modelName>") + "<modelName>".Length;
						int p_to = res.LastIndexOf("</modelName>");
						name = res.Substring(p_from, p_to - p_from);
					} catch {}
				}
			}

			return WebUtility.HtmlDecode(name);
		}
	}
}
