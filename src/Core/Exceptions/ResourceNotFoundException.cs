
namespace Core.Exceptions
{
    public class ResourceNotFoundException : BaseException<ResourceNotFoundException>
    {
        public ResourceNotFoundException()
        {
        }

        public ResourceNotFoundException(string errorMessage) : base(errorMessage)
        {

        }
        public ResourceNotFoundException(string errorMessage, Dictionary<string, IEnumerable<string>> errors) : base(errorMessage)
        {
            Errors = errors;
        }
        public ResourceNotFoundException(string errorMessage, Exception innerException) : base(errorMessage, innerException)
        {

        }

        public Dictionary<string, IEnumerable<string>> Errors { get; set; }
    }
}
