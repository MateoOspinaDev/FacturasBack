using BackMonoLegal.Models;
using BackMonoLegal.MonoLegalDBSettings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BackMonoLegal.Repository
{
    public class ClienteRepository
    {

        private readonly IMongoCollection<Cliente> _clienteCollection;

        public ClienteRepository(IMonoLegalSettings settings)
        {
            var client = new MongoClient(settings.Server);
            var database = client.GetDatabase(settings.Database);

            _clienteCollection = database.GetCollection<Cliente>(settings.Collection);
        }
        public async Task<IEnumerable<Cliente>> GetAll()
        {
            var clientes = await _clienteCollection.FindAsync(new BsonDocument());
            return await clientes.ToListAsync();
        }

        public async Task<Cliente> GetById(string id)
        {
            var filter = Builders<Cliente>.Filter.Eq("_id", ObjectId.Parse(id));
            return await _clienteCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task Create(Cliente cliente)
        {
            await _clienteCollection.InsertOneAsync(cliente);
        }
    }
}
