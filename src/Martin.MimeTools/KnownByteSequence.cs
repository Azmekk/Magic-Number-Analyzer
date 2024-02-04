using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martin.MimeTools
{
    /// <summary>
    /// Simple struct representing a sequence of bytes consisting of a byte array (ByteArr) and a starting offset (StartOffset).
    /// </summary>
	public struct KnownByteSequence
	{
        public byte[] ByteArr;
        public int StartOffset;

        public KnownByteSequence(byte[] byteArr, int startOffset)
        {
            ByteArr = byteArr;
            StartOffset = startOffset;
        }
    }
}
