namespace BackMonoLegal.PersistenceAdapter.MonoLegalDBSettings
{
    public class MonoLegalSettings : IMonoLegalSettings
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
    }
}
