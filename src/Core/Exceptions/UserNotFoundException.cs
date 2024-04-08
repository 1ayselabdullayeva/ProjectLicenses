namespace Core.Exceptions
{
    public class UserNotFoundException : BaseException<UserNotFoundException>
    {
        public UserNotFoundException()
        {
        }
        public UserNotFoundException(string errorMessage) : base(errorMessage)
        {

        }
        public UserNotFoundException(string errorMessage, Dictionary<string, IEnumerable<string>> errors) : base(errorMessage)
        {
            Errors = errors;
        }
        public UserNotFoundException(string errorMessage, Exception innerException) : base(errorMessage, innerException)
        {

        }

        public Dictionary<string, IEnumerable<string>> Errors { get; set; }
    }
}
