using System.Collections.Generic;

namespace Palmmedia.ReportGenerator.Core.Parser.Analysis.LineCoverage
{
    /// <summary>
    /// An implementation of <see cref="ILineInfo{T}"/> that uses a dictionary to store the line information. This implementation is more memory efficient for larger lengths with sparse data, but provides slower enumeration compared to <see cref="ArrayBasedLineInfo{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the line information.</typeparam>
    internal class DictionaryBasedLineInfo<T> : ILineInfo<T>
    {
        /// <summary>
        /// The internal dictionary that holds the line information. The keys of this dictionary represent the line indices, and the values represent the line information for those indices. If a line index is not present in the dictionary, it is assumed to have the default value specified in the constructor.
        /// </summary>
        private readonly Dictionary<long, T> data;

        /// <summary>
        /// The total number of elements in the line information. This value is specified in the constructor and is used to determine the valid range of indices for the line information. Indices outside this range are considered invalid and may result in exceptions if accessed.
        /// </summary>
        private readonly long length;

        /// <summary>
        /// The default value used when retrieving line information for indices that are not present in the dictionary. This value is specified in the constructor and is returned whenever an index is accessed that does not have an explicitly set value in the dictionary.
        /// </summary>
        private readonly T defaultValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryBasedLineInfo&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="length">The number of elements to allocate for the internal data array. Must be non-negative.</param>
        /// <param name="defaultValue">The default value to initialize each element with.</param>
        public DictionaryBasedLineInfo(long length, T defaultValue)
        {
            this.length = length;
            this.defaultValue = defaultValue;

            this.data = new Dictionary<long, T>();
        }

        /// <inheritdoc />
        public int Length
        {
            get
            {
                return (int)this.length;
            }
        }

        /// <inheritdoc />
        public long LongLength
        {
            get
            {
                return this.length;
            }
        }

        /// <inheritdoc />
        public T this[long index]
        {
            get
            {
                if (this.data.TryGetValue(index, out var value))
                {
                    return value;
                }
                else
                {
                    return this.defaultValue;
                }
            }

            set
            {
                if (EqualityComparer<T>.Default.Equals(this.defaultValue, value))
                {
                    return;
                }

                this.data[index] = value;
            }
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            for (long i = 0; i < this.length; i++)
            {
                if (this.data.TryGetValue(i, out var value))
                {
                    yield return value;
                }
                else
                {
                    yield return this.defaultValue;
                }
            }
        }

        /// <inheritdoc />
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc />
        public ILineInfo<T> Resize(long length)
        {
            var newLineInfo = new DictionaryBasedLineInfo<T>(length, this.defaultValue);

            foreach (var kv in this.data)
            {
                newLineInfo.data[kv.Key] = kv.Value;
            }

            return newLineInfo;
        }

        /// <inheritdoc />
        public ILineInfo<T> Clone()
        {
            return this.Resize(this.LongLength);
        }
    }
}
