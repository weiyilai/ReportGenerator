namespace Palmmedia.ReportGenerator.Core.Parser.Analysis.LineCoverage
{
    /// <summary>
    /// Factory for creating instances of <see cref="ILineInfo{T}"/>. The factory decides which implementation to use based on the length of the line info to create.
    /// </summary>
    internal static class LineInfoFactory
    {
        /// <summary>
        /// Creates a new instance of an <see cref="ILineInfo{T}"/> implementation with the specified length and default value.
        /// </summary>
        /// <remarks>For smaller lengths, an array-based implementation is used for faster enumeration.
        /// For larger or sparse data sets, a dictionary-based implementation is used for improved memory
        /// efficiency.</remarks>
        /// <typeparam name="T">The type of the values stored in the line information.</typeparam>
        /// <param name="length">The total number of elements in the line information. Must be non-negative.</param>
        /// <param name="defaultValue">The value to assign to all elements initially.</param>
        /// <returns>An <see cref="ILineInfo{T}"/> instance containing the specified number of elements, each initialized to the given default
        /// value.</returns>
        public static ILineInfo<T> Create<T>(long length, T defaultValue)
        {
            // Array-based implementation is faster when enumerating the results. The dictionary-based implementation is more memory efficient for large lengths with sparse data
            if (length < 2000)
            {
                return new ArrayBasedLineInfo<T>(length, defaultValue);
            }
            else
            {
                return new DictionaryBasedLineInfo<T>(length, defaultValue);
            }
        }
    }
}
