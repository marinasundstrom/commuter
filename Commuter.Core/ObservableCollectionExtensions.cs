using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Commuter
{
    public static class ObservableCollectionExtensions
    {
        public static void Sort<T>(this ObservableCollection<T> observable) where T : IComparable<T>, IEquatable<T>
        {
            SortBy<T, T>(observable, x => x);
        }

        public static void SortBy<T, TKey>(this ObservableCollection<T> observable, Func<T, TKey> predicate) where TKey : IComparable<TKey>, IEquatable<TKey>
        {
            var sorted = observable.OrderBy(predicate).ToList();

            var ptr = 0;
            while (ptr < sorted.Count)
            {
                if (!observable[ptr]!.Equals(sorted[ptr]))
                {
                    var t = observable[ptr];
                    observable.RemoveAt(ptr);
                    observable.Insert(sorted.IndexOf(t), t);
                }
                else
                {
                    ptr++;
                }
            }
        }
    }
}
