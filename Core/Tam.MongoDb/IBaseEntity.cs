
namespace Tam.MongoDb
{
    public interface IBaseEntity<T>
    {
        T Id { get; set; }
        bool Deleted { get; set; }
    }
}
