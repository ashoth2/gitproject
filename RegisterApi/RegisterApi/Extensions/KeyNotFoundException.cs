namespace RegisterApi.Extensions
{
    public class KeyNotFoundException :Exception
    {
        public KeyNotFoundException(string message) : base(message)
        { }
    }
}
