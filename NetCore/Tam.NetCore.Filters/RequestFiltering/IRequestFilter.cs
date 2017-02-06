namespace Tam.NetCore.Filters.RequestFiltering
{
    public interface IRequestFilter
    {
        void ApplyFilter(RequestFilteringContext context);
    }

    public interface IRequestFilter<T>: IRequestFilter { }
}
