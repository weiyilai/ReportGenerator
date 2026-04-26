namespace Palmmedia.ReportGenerator.Core.Parser.Analysis.LineCoverage
{
    /// <summary>
    /// Interface for line information. This interface is implemented by <see cref="ArrayBasedLineInfo{T}"/> and <see cref="DictionaryBasedLineInfo{T}"/> and is used to expose line information in a read-write manner.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the line information.</typeparam>
    internal interface ILineInfo<T> : IReadOnlyLineInfo<T>
    {
        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The element at the specified index.</returns>
        new T this[long index] { get; set; }

        /// <summary>
        /// Resizes the current line information to the specified length.
        /// </summary>
        /// <param name="length">The new length of the line. Must be greater than or equal to zero.</param>
        /// <returns>An instance of <see cref="ILineInfo{T}"/> representing the resized line information.</returns>
        ILineInfo<T> Resize(long length);

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <remarks>The returned clone is typically a deep copy, meaning changes to the clone do not
        /// affect the original instance and vice versa. The exact cloning behavior depends on the
        /// implementation.</remarks>
        /// <returns>A new <see cref="ILineInfo{T}"/> instance that is a copy of this instance.</returns>
        ILineInfo<T> Clone();
    }
}
