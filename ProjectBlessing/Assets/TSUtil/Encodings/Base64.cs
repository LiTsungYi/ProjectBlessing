using System;

namespace TeamSignal.Utilities.Encodings
{
	// Ref. http://stackoverflow.com/questions/11743160/how-do-i-encode-and-decode-a-base64-string
	
	/// <summary>
	/// A Base64 Decoder and Encoder
	/// </summary>
	public static class Base64
	{
		/// <summary>
		/// Encode the given plain text into Base64 encoded string
		/// </summary>
		/// <param name="plainText">Plain text</param>
		/// <returns>Encoded string</returns>
		public static string Encode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return Convert.ToBase64String(plainTextBytes);
		}
		
		/// <summary>
		/// Decode the Base64 Encoded string
		/// </summary>
		/// <param name="encodedData">Encoded string</param>
		/// <returns>Decoded string</returns>
		public static string Decode(string encodedData)
		{
			var base64EncodedBytes = Convert.FromBase64String(encodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}
	}
}