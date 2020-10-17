using System;

namespace SeedWorks
{
    public static class MonadExtentions
    {
        public static TResult PipeTo<TSource, TResult>(this TSource source, Func<TSource, TResult> func)
            => func(source);

        public static TOutput Either<TInput, TOutput>(this TInput o, Func<TInput, bool> condition, Func<TInput, TOutput> ifTrue, Func<TInput, TOutput> ifFalse)
            => condition(o) ? ifTrue(o) : ifFalse(o);

        public static TOutput Either<TInput, TOutput>(this TInput o, Func<TInput, TOutput> ifTrue, Func<TInput, TOutput> ifFalse)
            => o.Either(x => x != null, ifTrue, ifFalse);

        public static T Do<T>(this T obj, Action<T> action)
        {
            if (obj != null)
            {
                action(obj);
            }

            return obj;
        }
    }
}
