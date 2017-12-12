namespace MappingStack.General.Exceptions
{
    public class NotSupportedByMappingStackException 
        : System.Exception
        , IMappingStackException
    {
        public NotSupportedByMappingStackException(string message)
            : base(message)
        {
        }
    }
}
