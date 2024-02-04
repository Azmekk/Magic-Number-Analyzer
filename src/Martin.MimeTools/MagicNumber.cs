using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martin.MimeTools
{
	/// <summary>
	/// This is a struct type representing a magic number. 
	/// The MimeType is the string name it represents while the KnownByteSequences is an array of sequences that are known in a file. E.g. if we know 4 bytes at pos 0 and 8 bytes at offset 13 we can register 2 sequences at both offsets.
	/// </summary>
	public struct MagicNumber
	{
		public string MimeType;
		public KnownByteSequence[] KnownByteSequences;

		public MagicNumber(string mimeType, KnownByteSequence[] knownByteSequences)
		{
			MimeType = mimeType;
			KnownByteSequences = knownByteSequences;
		}
	}
}
