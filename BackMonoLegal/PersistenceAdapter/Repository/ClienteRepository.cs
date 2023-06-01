using BackMonoLegal.Domain.Models;
using BackMonoLegal.PersistenceAdapter.MonoLegalDBSettings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BackMonoLegal.PersistenceAdapter.Repository
{
    public class ClienteRepository : IClienteRepository
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


        public async Task ActualizarEstadoFacturaCliente(string clienteId, Factura factura)
        {
            var filter = Builders<Cliente>.Filter.And(
                Builders<Cliente>.Filter.Eq("_id", ObjectId.Parse(clienteId)),
                Builders<Cliente>.Filter.ElemMatch(f => f.Facturas, Builders<Factura>.Filter.Eq("_id", ObjectId.Parse(factura.Id)))
            );

            var update = Builders<Cliente>.Update.Set("Facturas.$.Estado", factura.Estado);

            await _clienteCollection.UpdateOneAsync(filter, update);
        }


    }
}
