namespace Martin.FileTools.Structs
{
	/// <summary>
	/// Represents a magic number with information about the associated MIME type and known byte sequences in a file.
	/// </summary>
	public class MagicNumber
	{
		/// <summary>
		/// Gets or sets the string name representing the MIME type associated with the magic number.
		/// </summary>
		public string MimeType { get; set; }

		/// <summary>
		/// Gets or sets an array of known byte sequences in the file associated with the magic number.
		/// Each sequence is defined by a <see cref="KnownByteSequence"/>.
		/// </summary>
		public KnownByteSequence[] KnownByteSequences { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MagicNumber"/> class with the specified MIME type and known byte sequences.
		/// </summary>
		/// <param name="mimeType">The string name representing the MIME type.</param>
		/// <param name="knownByteSequences">An array of known byte sequences in the file.</param>
		public MagicNumber(string mimeType, KnownByteSequence[] knownByteSequences)
		{
			MimeType = mimeType;
			KnownByteSequences = knownByteSequences;
		}
	}
}
