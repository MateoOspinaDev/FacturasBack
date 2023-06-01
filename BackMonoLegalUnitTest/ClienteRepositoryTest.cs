using BackMonoLegal.Domain.Models;
using BackMonoLegal.PersistenceAdapter.MonoLegalDBSettings;
using BackMonoLegal.PersistenceAdapter.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackMonoLegalUnitTest
{
    public class ClienteRepositoryTests
    {
        private readonly Mock<IMongoCollection<Cliente>> _clienteCollectionMock;
        private readonly ClienteRepository _clienteRepository;

        public ClienteRepositoryTests()
        {
            _clienteCollectionMock = new Mock<IMongoCollection<Cliente>>();
            var settings = new MonoLegalSettings
            {
                Server = "mongodb://localhost:27017",
                Database = "test-db",
                Collection = "clientes"
            };
            _clienteRepository = new ClienteRepository(settings);
        }

        
    }

}
