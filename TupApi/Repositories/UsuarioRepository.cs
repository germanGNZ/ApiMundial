using MongoDB.Driver;
using Microsoft.Extensions.Options;
using TupApi.Config;
using TupApi.Models;

namespace TupApi.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IMongoCollection<Usuario> _col;

        public UsuarioRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
        {
            var db = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _col = db.GetCollection<Usuario>("Usuarios");

            var indexKeys = Builders<Usuario>.IndexKeys;
            _col.Indexes.CreateOne(new CreateIndexModel<Usuario>(
                indexKeys.Ascending(u => u.Email),
                new CreateIndexOptions { Unique = true }
            ));
        }

        public async Task<Usuario?> GetByEmailAsync(string email) =>
            await _col.Find(u => u.Email == email).FirstOrDefaultAsync();

        public async Task<bool> ExisteEmailAsync(string email) =>
            await _col.Find(u => u.Email == email).AnyAsync();

        public async Task CreateAsync(Usuario usuario) =>
            await _col.InsertOneAsync(usuario);
    }
}