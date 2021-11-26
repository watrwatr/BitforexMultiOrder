using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace BitforexMultiOrder
{
	class Program
	{
		enum Sides : int
		{
			Buy = 1,
			Sell = 2
		}
		struct Order
		{
			public int Side;
			public decimal Price;
			public decimal Amount;

			public Order(Sides side, decimal price, decimal amount)
			{
				Side = (int)side;
				Price = price;
				Amount = amount;
			}
			public override string ToString()
			{
				return string.Concat(
						"{\"price\":", Price,
						",\"amount\":", Amount,
						",\"side\":", Side, "}");
			}
		}
		
		static string Crypt(string text, string apiSec)
		{
			HMACSHA256 hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(apiSec));
			byte[] messageBytes = Encoding.UTF8.GetBytes(text);
			byte[] computeHash = hmacsha256.ComputeHash(messageBytes);
			return BitConverter.ToString(computeHash).Replace("-", "").ToLower();
		}
		
		static void Main(string[] args)
		{
			string apiKey = "<PLACE-HERE-YOUR-APIKEY>";
			string apiSec = "<PLACE-HERE-YOUR-APISECRET>";
			
			// ================ request data ================
			// host
			string host = "https://api.bitforex.com";
			// uri of request
			string uri = "/api/v1/trade/placeMultiOrder";
			
				// symbol
			string symbol = "coin-usdt-xrp";
			// 1st order to place
			Order order1 = new Order(side: Sides.Buy, price: 0.9m, amount: 15m);
			// 2nd order to place
			Order order2 = new Order(side: Sides.Buy, price: 0.91m, amount: 15m);
			// ordersData
			string ordersData = "["+ order1.ToString()+ ","+ order2.ToString()+ "]";

			// preparing request body
			var nonce = (long)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalMilliseconds;
			
			string body = 
					"accessKey=" + apiKey+
					"&nonce=" + nonce+
					"&ordersData=" + ordersData+
					"&symbol=" + symbol;

			// encrypt request
			// how to create signed API Call -  see link https://github.com/githubdev2020/API_Doc_en/wiki/API-Call-Description
			string signData = Crypt(uri+"?"+body, apiSec);

			body = body + "&signData=" + signData;
			
			Console.WriteLine("REQ_BODY : " + body);
			
			// serialize body to bytes array
			byte[] bodyBuffer = Encoding.ASCII.GetBytes(body);
			
			// http request
			string url = host + uri;
			Console.WriteLine("REQ_URL  : " + url);
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.ContentLength = bodyBuffer.Length;
			
			// send body as byte buffer
			httpWebRequest.GetRequestStream().Write(bodyBuffer, 0,bodyBuffer.Length);

			// get response
			WebResponse response = httpWebRequest.GetResponse();
			StreamReader reader = new StreamReader(response.GetResponseStream());
			// read response
			string responseText = reader.ReadToEnd();

			// outputs
			Console.WriteLine("RESP_CODE: " + ((HttpWebResponse)response).StatusCode);
			Console.WriteLine("RESP_TEXT: " + responseText);
		}
	}
}