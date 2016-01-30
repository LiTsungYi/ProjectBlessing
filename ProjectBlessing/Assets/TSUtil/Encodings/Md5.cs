using System.Security.Cryptography;
using System.Text;

namespace TeamSignal.Utilities.Encodings
{
	/// <summary>
	/// Ref: http://wiki.unity3d.com/index.php?title=MD5
	/// </summary>
	public static class Md5
	{
		public static readonly int Length = 32;
		
		public static string Sum( string strToEncrypt )
		{
			var ue = new UTF8Encoding();
			byte[] bytes = ue.GetBytes( strToEncrypt );
			
			// encrypt bytes
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			byte[] hashBytes = md5.ComputeHash( bytes );
			
			// Convert the encrypted bytes back to a string (base 16)
			string hashString = "";
			
			for ( int i = 0; i < hashBytes.Length; i++ )
			{
				hashString += System.Convert.ToString( hashBytes[ i ], 16 ).PadLeft( 2, '0' );
			}
			
			return hashString.PadLeft( 32, '0' );
		}
	}
}