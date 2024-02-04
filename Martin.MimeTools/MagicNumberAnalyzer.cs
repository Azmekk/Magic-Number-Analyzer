using System;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.X86;
using static System.Net.Mime.MediaTypeNames;

namespace Martin.MimeTools;

public static class MagicNumberAnalyzer
{
	/// <summary>
	/// Checks a memory stream for known magic numbers for image types.
	/// </summary>
	/// <returns>
	/// A string representing a known file mime type or a general application/octet-stream if no match.
	/// </returns>
	static public string GetFileMimeType(MemoryStream stream)
	{
		if(stream == null)
		{
			return "";
		}

		if(stream.Length < 50)
		{
			return "";
		}

		byte[] buffer = new byte[50];
		stream.Read(buffer, 0, buffer.Length);

		return DetermineMimeString(buffer);
	}

	/// <summary>
	/// Checks a byte array for known magic numbers for image types.
	/// </summary>
	/// <returns>
	/// /// A string representing a known file mime type or a general application/octet-stream if no match.
	/// </returns>
	static public string GetFileMimeType(byte[] byteArr)
	{
		if(byteArr == null)
		{
			return "";
		}

		if(byteArr.Length < 50)
		{
			return "";
		}

		return DetermineMimeString(byteArr);
	}

	/// <summary>
	/// Checks a file stream for known magic numbers for image types.
	/// </summary>
	/// <returns>
	/// /// A string representing a known file mime type or a general application/octet-stream if no match.
	/// </returns>
	static public string GetFileMimeType(FileStream fileStream)
	{
		if(fileStream == null)
		{
			return "";
		}

		if(fileStream.Length < 50)
		{
			return "";
		}

		byte[] buffer = new byte[50];
		fileStream.Read(buffer, 0, buffer.Length);

		return DetermineMimeString(buffer);
	}

	static private string DetermineMimeString(byte[] byteArr)
	{
		if(FileIsJpeg(byteArr))
		{
			return Image.Jpeg;
		}
		else if(FileIsPng(byteArr))
		{
			return Image.Png;
		}
		else if(FileIsBmp(byteArr))
		{
			return Image.Bmp;
		}
		else if(FileIsGif(byteArr))
		{
			return Image.Gif;
		}
		else if(FileIsIcon(byteArr))
		{
			return Image.Icon;
		}
		else if(FileIsSvg(byteArr))
		{
			return Image.Svg;
		}
		else if(FileIsTiff(byteArr))
		{
			return Image.Tiff;
		}
		else if(FileIsPDF(byteArr))
		{
			return Application.Pdf;
		}
		else if(FileIsZip(byteArr))
		{
			return Application.Zip;
		}
		else if(FileIs7z(byteArr))
		{
			return MimeTypeConstants.SevenZip;
		}
		else if(FileIsGzip(byteArr))
		{
			return MimeTypeConstants.Gzip;
		}
		else if(FileIsMp3(byteArr))
		{
			return MimeTypeConstants.Mp3;
		}
		else if(FileIsMp4(byteArr))
		{
			return MimeTypeConstants.Mp4;
		}
		else if(FileIsMov(byteArr))
		{
			return MimeTypeConstants.Mov;
		}
		else if(FileIsAvi(byteArr))
		{
			return MimeTypeConstants.Avi;
		}
		else if(FileIsWav(byteArr))
		{
			return MimeTypeConstants.Wav;
		}
		else if(FileIsWebp(byteArr))
		{
			return Image.Webp;
		}
		else if(FileIsWebm(byteArr))
		{
			return MimeTypeConstants.Webm;
		}
		else if(FileIsFlv(byteArr))
		{
			return MimeTypeConstants.Flv;
		}
		else if(FileIsM4v(byteArr))
		{
			return MimeTypeConstants.M4v;
		}


		return Application.Octet;
	}

	static private bool FileIsJpeg(byte[] byteArr)
	{
		byte[] knownStartingBytes = [0xFF, 0xD8, 0xFF,];

		if(!CompareBytes(knownStartingBytes, byteArr))
		{
			return false;
		}

		byte[] knownFourthBytes = [0xE0, 0xE1, 0xE8,];

		if(knownFourthBytes.Contains(byteArr[3]))
		{
			return true;
		}

		return false;
	}

	static private bool FileIsPng(byte[] byteArr)
	{
		byte[] knownStartingBytes = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A,];

		return CompareBytes(byteArr, knownStartingBytes);
	}

	static private bool FileIsBmp(byte[] byteArr)
	{
		byte[] knownStartingBytes = [0x42, 0x4D];

		return CompareBytes(byteArr, knownStartingBytes);
	}

	static private bool FileIsGif(byte[] byteArr)
	{
		byte[][] knownStartingBytesList =
		[
			[0x47, 0x49, 0x46, 0x38, 0x37, 0x61,],
			[0x47, 0x49, 0x46, 0x38, 0x39, 0x61,]
		];

		foreach(byte[] knownStartingBytes in knownStartingBytesList)
		{
			if(CompareBytes(byteArr, knownStartingBytes))
			{
				return true;
			}
		}

		return false;
	}

	static private bool FileIsIcon(byte[] byteArr)
	{
		byte[] knownStartingBytes = [0x00, 0x00, 0x01, 0x00];

		return CompareBytes(byteArr, knownStartingBytes);
	}

	static private bool FileIsSvg(byte[] byteArr)
	{
		byte[] knownStartingBytes = [0x3C, 0x3F, 0x78, 0x6D, 0x6C, 0x20];

		return CompareBytes(byteArr, knownStartingBytes);
	}

	static private bool FileIsTiff(byte[] byteArr)
	{
		byte[][] knownStartingBytesList =
		[
			[0x49, 0x49, 0x2A, 0x00],
			[0x4D, 0x4D, 0x00, 0x2A],
			[0x4D, 0x4D, 0x00, 0x2B],
		];

		foreach(var knownStartingBytes in knownStartingBytesList)
		{
			if(CompareBytes(byteArr, knownStartingBytes))
			{
				return true;
			}

		}

		return false;
	}

	static private bool FileIsPDF(byte[] byteArr)
	{
		byte[] knownStartingBytes = [0x25, 0x50, 0x44, 0x46];
		return CompareBytes(byteArr, knownStartingBytes);
	}

	static private bool FileIsZip(byte[] byteArr)
	{
		byte[] knownStartingBytes = [0x50, 0x4B, 0x03, 0x04];
		return CompareBytes(byteArr, knownStartingBytes);
	}

	static private bool FileIs7z(byte[] byteArr)
	{
		byte[] knownStartingBytes = [0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C];
		return CompareBytes(byteArr, knownStartingBytes);
	}

	static private bool FileIsGzip(byte[] byteArr)
	{
		byte[] knownStartingBytes = [0x1F, 0x8B, 0x08];
		return CompareBytes(byteArr, knownStartingBytes);
	}

	static private bool FileIsMp3(byte[] byteArr)
	{
		byte[] knownStartingBytes = [0x49, 0x44, 0x33];
		return CompareBytes(byteArr, knownStartingBytes);
	}

	static private bool FileIsMp4(byte[] byteArr)
	{
		byte[][] knownStartingBytesArr = [
			[0x66, 0x74, 0x79, 0x70, 0x4D, 0x53, 0x4E, 0x56],
			[0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D],

		];

		foreach(byte[] knownStartingBytes in knownStartingBytesArr)
		{
			if(CompareBytes(byteArr, knownStartingBytes, 4))
			{
				return true;
			}

		}

		return false;
	}

	static private bool FileIsMov(byte[] byteArr)
	{
		byte[][] knownStartingBytesArr = [
			[0x66, 0x74, 0x79, 0x70, 0x71, 0x74, 0x20, 0x20],
			[0x6D, 0x6F, 0x6F, 0x76]
		];
		foreach(byte[] knownStartingBytes in knownStartingBytesArr)
		{
			if(CompareBytes(byteArr, knownStartingBytes, 4))
			{
				return true;
			}

		}
		return false;
	}

	static private bool FileIsAvi(byte[] byteArr)
	{
		byte[] knownStartingBytes = [0x52, 0x49, 0x46, 0x46];
		byte[] knownEndingBytes = [0x41, 0x56, 0x49, 0x20, 0x4C, 0x49, 0x53, 0x54];

		bool startingBytesMatch = CompareBytes(byteArr, knownStartingBytes);
		bool endingBytesMatch = CompareBytes(byteArr, knownEndingBytes, 8);

		return startingBytesMatch && endingBytesMatch;
	}

	static private bool FileIsWav(byte[] byteArr)
	{
		byte[] knownStartingBytes = [0x52, 0x49, 0x46, 0x46];
		byte[] knownEndingBytes = [0x57, 0x41, 0x56, 0x45, 0x66, 0x6D, 0x74, 0x20];

		bool startingBytesMatch = CompareBytes(byteArr, knownStartingBytes);
		bool endingBytesMatch = CompareBytes(byteArr, knownEndingBytes, 8);

		return startingBytesMatch && endingBytesMatch;
	}

	static private bool FileIsWebp(byte[] byteArr)
	{
		byte[] knownStartingBytes = [0x52, 0x49, 0x46, 0x46,];
		byte[] knownEndingBytes = [0x57, 0x45, 0x42, 0x50,];

		bool startingBytesMatch = CompareBytes(byteArr, knownStartingBytes);
		bool endingBytesMatch = CompareBytes(byteArr, knownEndingBytes, 8);

		return startingBytesMatch && endingBytesMatch;
	}

	static private bool FileIsWebm(byte[] byteArr)
	{
		byte[] knownStartingBytes = [0x1A, 0x45, 0xDF, 0xA3];
		return CompareBytes(byteArr, knownStartingBytes);
	}

	static private bool FileIsFlv(byte[] byteArr)
	{
		byte[] knownStartingBytesArr = [0x46, 0x4C, 0x56, 0x01];

		return CompareBytes(byteArr, knownStartingBytesArr);
	}

	static private bool FileIsM4v(byte[] byteArr)
	{
		byte[][] knownStartingBytesArr = [
			[0x66, 0x74, 0x79, 0x70, 0x4D, 0x34, 0x56, 0x20],
			[0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34, 0x32],

		];

		foreach(byte[] knownStartingBytes in knownStartingBytesArr)
		{
			if(CompareBytes(byteArr, knownStartingBytes, 4))
			{
				return true;
			}

		}

		return false;
	}

	static private bool CompareBytes(byte[] arr1, byte[] arr2)
	{
		int minLength = Math.Min(arr1.Length, arr2.Length);

		for(int i = 0; i < minLength; i++)
		{
			if(arr1[i] != arr2[i])
			{
				return false;
			}
		}

		return true;
	}

	static private bool CompareBytes(byte[] arr1, byte[] arr2, int startingPos)
	{
		int minLength = Math.Min(arr1.Length, arr2.Length);

		for(int i = 0; i < minLength; i++)
		{
			if(arr1.Length > arr2.Length)
			{
				if(arr1[i + startingPos] != arr2[i])
				{
					return false;
				}
			}
			else
			{
				if(arr1[i] != arr2[i + startingPos])
				{
					return false;
				}
			}
		}

		return true;
	}
}
