namespace BackMonoLegal.PersistenceAdapter.MonoLegalDBSettings
{
    public interface IMonoLegalSettings
    {
        string? Server { get; set; }
        string? Database { get; set; }
        string? Collection { get; set; }
    }
}
