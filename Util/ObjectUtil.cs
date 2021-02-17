﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace TvpMain.Util
{
    /// <summary>
    /// Standard object utilities and extension methods.
    /// </summary>
    public static class ObjectUtil
    {
        /// <summary>
        /// Creates singleton enumerable of one item.
        /// </summary>
        /// <typeparam name="T">Item type (provided).</typeparam>
        /// <param name="thisObject">This object (provided).</param>
        /// <returns>Object wrapped in singleton enumerable.</returns>
        public static IEnumerable<T> ToSingletonEnumerable<T>(this T thisObject) =>
            Enumerable.Repeat(thisObject, 1);
    }
}
