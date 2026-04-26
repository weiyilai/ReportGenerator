using System;
using System.Collections.Generic;

namespace Palmmedia.ReportGenerator.Core.Parser.Analysis.LineCoverage
{
    /// <summary>
    /// An implementation of <see cref="ILineInfo{T}"/> that uses an array to store the line information. This implementation is more memory efficient for smaller lengths and provides faster enumeration.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the line information.</typeparam>
    internal class ArrayBasedLineInfo<T> : ILineInfo<T>
    {
        /// <summary>
        /// The internal array that holds the line information. The length of this array is determined by the length specified in the constructor. Each element in this array represents the line information for a specific line index.
        /// </summary>
        private readonly T[] data;

        /// <summary>
        /// The default value used when initializing the array elements.
        /// </summary>
        private readonly T defaultValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayBasedLineInfo&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="length">The number of elements to allocate for the internal data array. Must be non-negative.</param>
        /// <param name="defaultValue">The default value to initialize each element with.</param>
        public ArrayBasedLineInfo(long length, T defaultValue)
        {
            this.defaultValue = defaultValue;

            this.data = new T[length];

            if (EqualityComparer<T>.Default.Equals(defaultValue, default(T)))
            {
                return;
            }

            for (long i = 0; i < length; i++)
            {
                this.data[i] = defaultValue;
            }
        }

        /// <inheritdoc />
        public int Length
        {
            get
            {
                return this.data.Length;
            }
        }

        /// <inheritdoc />
        public long LongLength
        {
            get
            {
                return this.data.LongLength;
            }
        }

        /// <inheritdoc />
        public T this[long index]
        {
            get => this.data[index];
            set => this.data[index] = value;
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in this.data)
            {
                yield return item;
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
            var newLineInfo = new ArrayBasedLineInfo<T>(length, this.defaultValue);

            Array.Copy(this.data, newLineInfo.data, Math.Min(this.data.LongLength, newLineInfo.data.LongLength));

            return newLineInfo;
        }

        /// <inheritdoc />
        public ILineInfo<T> Clone()
        {
            return this.Resize(this.LongLength);
        }
    }
}
