namespace AzNotesSample.Storage
{
    public class MemoryStorage : IStorage
    {
        private static string _value = string.Empty;
        
        public Task<string> LoadAsync()
        {
            return Task.FromResult(_value);
        }

        public Task SaveAsync(string text)
        {
            _value = text;
            return Task.CompletedTask;
        }
    }
}
