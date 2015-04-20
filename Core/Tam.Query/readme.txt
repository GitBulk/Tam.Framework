class Program
{
    static void Main(string[] args)
    {
        var db = new GreatBlogEntities();
        //var result = db.Traffic.ToList();

        var traffic = new TrafficProjection(db);

        // good
        //var items = traffic.GetItems<TrafficDto>();

        // good
        //var filter = traffic.GetItems<TrafficDto>(f => f.Name == "Toan");

        // good
        //var order = traffic.GetItems<TrafficDto>(f => f.Name == "Toan", or => or.OrderByDescending(t => t.Id));

        // good
        //var getListTask = traffic.GetItemsAsync<TrafficDto>();
        //Task.WaitAll(getListTask);
        //var items = getListTask.Result;

        // good
        //var getListTask = traffic.GetItemsAsync<TrafficDto>(f => f.Name == "Toan");
        //Task.WaitAll(getListTask);
        //var filterAsync = getListTask.Result;

        // good
        var getListTask = traffic.GetItemsAsync<TrafficDto>(f => f.Name == "Toan", or => or.OrderByDescending(t => t.Id));
        Task.WaitAll(getListTask);
        var orderAsync = getListTask.Result;

        System.Console.WriteLine("end");
    }

}

public partial class TrafficDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

class TrafficProjection : QueryProjection<Traffic>
{
    DbContext context;
    public TrafficProjection(DbContext context)
        : base(context)
    {
        this.context = context;
    }
}