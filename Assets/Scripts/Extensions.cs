using System.Collections.Generic;

public static class Extensions {
    public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, out TKey key, out TValue value) {
        key = pair.Key;
        value = pair.Value;
    }

    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable) {
        return new HashSet<T>(enumerable);
    }
}
