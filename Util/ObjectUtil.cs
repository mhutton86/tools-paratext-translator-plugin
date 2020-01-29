﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvpMain.Util
{
    /// <summary>
    /// Standard object utilities and extension methods.
    /// </summary>
    public static class ObjectUtil
    {
        /// <summary>
        /// Creates immutable, singleton list of one item.
        /// </summary>
        /// <typeparam name="T">Item type (provided).</typeparam>
        /// <param name="thisObject">This object (provided).</param>
        /// <returns>Object wrapped in immutable, singleton list.</returns>
        public static IList<T> ToSingletonList<T>(this T thisObject)
        {
            return Enumerable.Repeat(thisObject, 1).ToImmutableList();
        }
    }
}
