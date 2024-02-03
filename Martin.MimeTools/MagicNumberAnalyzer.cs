using System;
using System.Reflection.Metadata.Ecma335;
using static System.Net.Mime.MediaTypeNames;

namespace Martin.MimeTools;

public static class MagicNumberAnalyzer
{
	/// <summary>
	/// Checks a memory stream for known magic numbers for image types.
	/// </summary>
	/// <returns>
	/// A string which should be compared to the constants provided by System.Net.Mime.MediaTypeNames or an empty string if no match.;
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
	/// A string which should be compared to the constants provided by System.Net.Mime.MediaTypeNames or an empty string if no match.;
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
	/// A string which should be compared to the constants provided by System.Net.Mime.MediaTypeNames or an empty string if no match.;
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
		else if(FileIsWebp(byteArr))
		{
			return Image.Webp;
		}

		return "";
	}

	static private bool FileIsJpeg(byte[] byteArr)
	{
		List<byte> knownStartingBytes = [0xFF, 0xD8, 0xFF,];

		List<List<byte>> byteArrays =
		[
			[0xE0],
			[0x4A, 0x46, 0x49, 0x46, 0x00],
			[0xE1],
			[0x45, 0x78, 0x69, 0x66, 0x00],
			[0xE8],
			[0x53, 0x50, 0x49, 0x46, 0x46, 0x00],
		];

		for(int i = 0; i < 3; i++)
		{
			if(byteArr[i] != knownStartingBytes[i])
			{
				return false;
			}
		}

		int matchingArray = -1;
		for(int i = 0; i < byteArrays.Count; i += 2)
		{
			if(byteArr[3] == byteArrays[i][0])
			{
				matchingArray = i + 1;
				break;
			}
		}

		if(matchingArray == -1)
		{
			return false;
		}

		for(int i = 0; i < byteArrays[matchingArray].Count; i++)
		{
			if(byteArr[6 + i] != byteArrays[matchingArray][i])
			{
				return false;
			}
		}

		return true;
	}

	static private bool FileIsPng(byte[] byteArr)
	{
		List<byte> knownStartingBytes = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A,];

		return CompareBytes(byteArr, knownStartingBytes);
	}

	static private bool FileIsBmp(byte[] byteArr)
	{
		List<byte> knownStartingBytes = [0x42, 0x4D];

		return CompareBytes(byteArr, knownStartingBytes);
	}

	static private bool FileIsGif(byte[] byteArr)
	{
		List<List<byte>> knownStartingBytesList =
		[
			[0x47, 0x49, 0x46, 0x38, 0x37, 0x61,],
			[0x47, 0x49, 0x46, 0x38, 0x39, 0x61,]
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

	static private bool FileIsIcon(byte[] byteArr)
	{
		List<byte> knownStartingBytes = [0x00, 0x00, 0x01, 0x00];

		return CompareBytes(byteArr, knownStartingBytes);
	}

	static private bool FileIsSvg(byte[] byteArr)
	{
		List<byte> knownStartingBytes = [0x3C, 0x3F, 0x78, 0x6D, 0x6C, 0x20];

		return CompareBytes(byteArr, knownStartingBytes);
	}

	static private bool FileIsTiff(byte[] byteArr)
	{
		List<List<byte>> knownStartingBytesList =
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

	static private bool FileIsWebp(byte[] byteArr)
	{
		List<byte> knownStartingBytes = [0x52, 0x49, 0x46, 0x46,];
		List<byte> knownEndingBytes = [0x57, 0x45, 0x42, 0x50,];

		bool startingBytesMatch = CompareBytes(byteArr, knownStartingBytes);

		byte[] endingBytesBuffer = new byte[4];
		for(int i = 8; i < 12; i++)
		{
			endingBytesBuffer[i - 8] = byteArr[i];
		}

		bool endingBytesMatch = CompareBytes(endingBytesBuffer, knownEndingBytes);

		return startingBytesMatch && endingBytesMatch;
	}

	static private bool CompareBytes(IEnumerable<byte> p1, IEnumerable<byte> p2)
	{
		var arr1 = p1.ToArray();
		var arr2 = p2.ToArray();
		if(arr1.Length > arr2.Length)
		{
			for(int i = 0; i < arr2.Length; i++)
			{
				if(arr1[i] != arr2[i])
				{
					return false;
				}
			}
		}
		else
		{
			for(int i = 0; i < arr1.Length; i++)
			{
				if(arr1[i] != arr2[i])
				{
					return false;
				}
			}
		}

		return true;
	}
}
