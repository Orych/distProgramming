using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Valuator
{
    public class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer _connection;
        public const string SERVER = "localhost";
        public const string PORT = "6379";

        private readonly IConfiguration Configuration;

        public RedisService(IConfiguration configuration)
        {
            Configuration = configuration;
            _connection = ConnectionMultiplexer.Connect($"{Configuration["RedisConfig:Server"]}:{Configuration["RedisConfig:Port"]}");
        }

        public string Get(string key)
        {
            var db = _connection.GetDatabase();

            return db.StringGet(key);
        }
        public List<string> Gets()
        {
            var keys = _connection.GetServer($"{Configuration["RedisConfig:Server"]}:{Configuration["RedisConfig:Port"]}").Keys();

            return keys.Select(x => x.ToString()).ToList();
        }

        public void Put(string key, string value)
        {
            var db = _connection.GetDatabase();

            db.StringSet(key, value);
        }
    }
}