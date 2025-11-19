namespace Shared.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException(string? name) : base($"Entiry {name} already exists!") { }
    }
}
