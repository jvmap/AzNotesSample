namespace AzNotesSample.Storage
{
    public interface IStorage
    {
        public Task SaveAsync(string text);

        public Task<string> LoadAsync();

        public string DescriptiveText { get; }
    }
}
