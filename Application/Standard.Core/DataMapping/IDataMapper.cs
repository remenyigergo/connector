namespace Standard.Core.DataMapping
{
    public interface IDataMapper<TSource, TTarget>
    {
        TTarget Map(TSource source);
        TSource Map(TTarget source);
    }
}
