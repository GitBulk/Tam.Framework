namespace Tam.Repository.Contraction
{
    public interface IBaseRepository<T> : ICrudAsyncRepository<T>,
        ICrudRepository<T> where T : class
    {

    }
}
