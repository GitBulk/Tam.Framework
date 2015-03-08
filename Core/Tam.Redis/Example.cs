using Tam.JsonManager;

namespace Tam.Redis
{
    public class Example
    {
        private void Do()
        {
            IRedisServerSettings settings = new RedisServerSettings();
            IRedisConnection connection = new SingletonRedisConnection(settings.ConnectionStringOrName);
            var redisManager = new RedisConnectionManager(settings, connection);
            //connectionFactory.Create()
            ISerializer serializer = new JilJsonSerializer();
            IRedisClient redisClient = new RedisClient(redisManager, settings, serializer);

            //System.Console.WriteLine("dong ket noi");
            //connection.Close();
            redisManager.Close();
            //System.Console.WriteLine(connection.IsConnectedToServer());
        }
    }
}