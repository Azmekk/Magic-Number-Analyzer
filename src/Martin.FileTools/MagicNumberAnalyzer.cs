﻿using Martin.FileTools.Constants;
using Martin.FileTools.Types;
using static System.Net.Mime.MediaTypeNames;

namespace Martin.FileTools;

/// <summary>
/// Static class that offers mime type detection functionality based on magic numbers.
/// </summary>
public static class MagicNumberAnalyzer
{
	private readonly static List<MagicNumber> MagicNumbers =
	[
		new(Image.Jpeg, [new([0xFF, 0xD8], 0)]),
		new(Image.Png, [new([0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A], 0)]),
		new(Image.Bmp, [new([0x42, 0x4D], 0)]),
		new(Image.Gif, [new([0x47, 0x49, 0x46, 0x38, 0x37, 0x61], 0)]),
		new(Image.Gif, [new([0x47, 0x49, 0x46, 0x38, 0x39, 0x61], 0)]),
		new(Image.Icon, [new([0x00, 0x00, 0x01, 0x00], 0)]),
		new(Image.Svg, [new([0x3C, 0x3F, 0x78, 0x6D, 0x6C, 0x20], 0)]),
		new(Image.Tiff, [new([0x49, 0x49, 0x2A, 0x00], 0)]),
		new(Image.Tiff, [new([0x4D, 0x4D, 0x00, 0x2A], 0)]),
		new(Image.Tiff, [new([0x4D, 0x4D, 0x00, 0x2B], 0)]),
		new(Application.Pdf, [new([0x25, 0x50, 0x44, 0x46], 0)]),
		new(Application.Zip, [new([0x50, 0x4B, 0x03, 0x04], 0)]),
		new(MimeTypeConstants.Rar, [new([0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00], 0)]),
		new(MimeTypeConstants.Rar, [new([0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x01, 0x00], 0)]),
		new(MimeTypeConstants.SevenZip, [new([0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C], 0)]),
		new(MimeTypeConstants.Gzip, [new([0x1F, 0x8B, 0x08], 0)]),
		new(MimeTypeConstants.Mp3, [new([0x49, 0x44, 0x33], 0)]),
		new(MimeTypeConstants.Mp4, [new([0x66, 0x74, 0x79, 0x70, 0x4D, 0x53, 0x4E, 0x56], 4)]),
		new(MimeTypeConstants.Mp4, [new([0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D], 4)]),
		new(MimeTypeConstants.Mov, [new([0x66, 0x74, 0x79, 0x70, 0x71, 0x74, 0x20, 0x20], 4)]),
		new(MimeTypeConstants.Mov, [new([0x6D, 0x6F, 0x6F, 0x76], 0)]),
		new(MimeTypeConstants.Avi, [new([0x52, 0x49, 0x46, 0x46], 0), new([0x41, 0x56, 0x49, 0x20, 0x4C, 0x49, 0x53, 0x54], 8)]),
		new(MimeTypeConstants.Wav, [new([0x52, 0x49, 0x46, 0x46], 0), new([0x57, 0x41, 0x56, 0x45, 0x66, 0x6D, 0x74, 0x20], 8)]),
		new(Image.Webp, [new([0x52, 0x49, 0x46, 0x46], 0), new([0x57, 0x45, 0x42, 0x50,], 8)]),
		new(MimeTypeConstants.Webm, [new([0x1A, 0x45, 0xDF, 0xA3], 0)]),
		new(MimeTypeConstants.Flv, [new([0x46, 0x4C, 0x56, 0x01], 0)]),
		new(MimeTypeConstants.M4v, [new([0x66, 0x74, 0x79, 0x70, 0x4D, 0x34, 0x56, 0x20], 4)]),
		new(MimeTypeConstants.M4v, [new([0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34, 0x32], 4)]),
		new(MimeTypeConstants.Exe, [new([0x4D, 0x5A], 0)]),
	];

	private readonly static List<MagicNumber> CustomMagicNumbers = [];

	/// <summary>
	/// Scans a <see cref="Stream"/> or a derived class such as <see cref="MemoryStream"/> or <see cref="FileStream"/> for predefined magic numbers to identify file types.
	/// </summary>
	/// <returns>
	/// Returns a string representing the detected file mime type if a match is found, or a general "application/octet-stream" if no match is identified.
	/// </returns>
	static public string GetFileMimeType(Stream stream)
	{
		ArgumentNullException.ThrowIfNull(stream);

		string result = DetermineMimeString(stream);
		stream.Position = 0;

		return result;
	}

	/// <summary>
	/// Scans a <see cref="byte"/> array for known magic numbers.
	/// </summary>
	/// <returns>
	/// Returns a string representing the detected file mime type if a match is found, or a general "application/octet-stream" if no match is identified.
	/// </returns>
	static public string GetFileMimeType(byte[] byteArr)
	{
		ArgumentNullException.ThrowIfNull(byteArr);

		return DetermineMimeString(byteArr);
	}

	static private string DetermineMimeString(byte[] byteArr)
	{
		(bool success, string result) = MatchBytesToMagicNumbers(CustomMagicNumbers, byteArr);

		if(success)
		{
			return result;
		}

		(success, result) = MatchBytesToMagicNumbers(MagicNumbers, byteArr);

		if(success)
		{
			return result;
		}

		return Application.Octet;
	}

	static private (bool, string) MatchBytesToMagicNumbers(List<MagicNumber> magicNumbers, byte[] byteArr)
	{
		foreach(MagicNumber magicNumber in magicNumbers)
		{
			bool allSequencesMatch = true;
			foreach(KnownByteSequence knownByteSequence in magicNumber.KnownByteSequences)
			{
				if(knownByteSequence.StartOffset + knownByteSequence.ByteArr.Length > byteArr.Length)
				{
					allSequencesMatch = false;
					break;
				}

				if(!CompareBytes(byteArr, knownByteSequence.ByteArr, knownByteSequence.StartOffset))
				{
					allSequencesMatch = false;
					break;
				}
			}

			if(allSequencesMatch)
			{
				return (true, magicNumber.MimeType);
			}
		}

		return (false, Application.Octet);
	}

	static private string DetermineMimeString(Stream stream)
	{
		ArgumentNullException.ThrowIfNull(stream);

		(bool success, string result) = MatchBytesToMagicNumbers(CustomMagicNumbers, stream);

		if(success)
		{
			return result;
		}

		(success, result) = MatchBytesToMagicNumbers(MagicNumbers, stream);

		if(success)
		{
			return result;
		}

		return Application.Octet;
	}

	private static (bool success, string result) MatchBytesToMagicNumbers(List<MagicNumber> magicNumbers, Stream stream)
	{
		foreach(MagicNumber magicNumber in magicNumbers)
		{
			bool allSequencesMatch = true;
			foreach(KnownByteSequence knownByteSequence in magicNumber.KnownByteSequences)
			{
				if(knownByteSequence.StartOffset + knownByteSequence.ByteArr.Length > stream.Length)
				{
					allSequencesMatch = false;
					break;
				}

				if(!CompareBytes(stream, knownByteSequence.ByteArr, knownByteSequence.StartOffset))
				{
					allSequencesMatch = false;
					break;
				}
			}

			if(allSequencesMatch)
			{
				return (true, magicNumber.MimeType);
			}
		}

		return (false, Application.Octet);
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

	private static bool CompareBytes(Stream stream, byte[] byteArr, int startOffset)
	{
		stream.Position = startOffset;

		for(int i = 0; i < byteArr.Length; i++)
		{
			if(stream.ReadByte() != byteArr[i])
			{
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Registers a new magic number to the custom scanning list. This allows the addition of custom magic numbers or the override of default ones.
	/// </summary>
	/// <param name="magicNumber">An instance of the MagicNumber class representing the new magic number to be added.</param>
	static public void AddCustomMagicNumber(MagicNumber magicNumber)
	{
		ArgumentNullException.ThrowIfNull(magicNumber);

		CustomMagicNumbers.Add(magicNumber);
	}

	/// <summary>
	/// Registers a collection of magic numbers to the custom scanning list. This method enables the addition of custom magic numbers or the override of default ones.
	/// </summary>
	/// <param name="magicNumbers">A List of MagicNumber instances representing the new magic numbers to be added.</param>
	static public void AddCustomMagicNumbersRange(List<MagicNumber> magicNumbers)
	{
		CustomMagicNumbers.AddRange(magicNumbers);
	}
}
