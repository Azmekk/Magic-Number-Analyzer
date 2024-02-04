using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Martin.MimeTools
{
	internal static class MimeTypeConstants
	{
		//Application files
		public const string SevenZip = "application/x-7z-compressed";
		public const string Gzip = "application/gzip";
		public const string Rar = "application/vnd.rar";
		public const string Exe = "application/x-msdownload";


		//Media
		public const string Mp3 = "audio/mpeg";
		public const string Mp4 = "video/mp4";
		public const string Mov = "video/quicktime";
		public const string Avi = "video/x-msvideo";
		public const string Wav = "audio/wav";
		public const string Webm = "video/webm";
		public const string Flv = "video/x-flv";
		public const string M4v = "video/x-m4v";
	}
}
