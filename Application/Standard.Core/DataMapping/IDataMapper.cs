namespace Standard.Core.DataMapping
{
    public interface IDataMapper<TSource, TTarget>
    {
        TTarget Map(TSource obj);
        TSource Map(TTarget obj);
    }
}
