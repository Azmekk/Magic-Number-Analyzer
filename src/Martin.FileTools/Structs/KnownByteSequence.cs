namespace Martin.FileTools.Structs
{
	/// <summary>
	/// Represents a known byte sequence with a byte array and a starting offset.
	/// </summary>
	public class KnownByteSequence
	{
		/// <summary>
		/// Gets or sets the byte array of the known sequence.
		/// </summary>
		public byte[] ByteArr { get; set; }

		/// <summary>
		/// Gets or sets the starting offset of the known sequence.
		/// </summary>
		public int StartOffset { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="KnownByteSequence"/> class with the specified byte array and starting offset.
		/// </summary>
		/// <param name="byteArr">The byte array of the known sequence.</param>
		/// <param name="startOffset">The starting offset of the known sequence.</param>
		public KnownByteSequence(byte[] byteArr, int startOffset)
		{
			ByteArr = byteArr;
			StartOffset = startOffset;
		}
	}
}
