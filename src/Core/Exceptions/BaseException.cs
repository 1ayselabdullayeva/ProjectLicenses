namespace Core.Exceptions
{
	public class BaseException<T> : Exception where T : class, new()
	{
		public BaseException() { }

		public BaseException(string message) : base(message) { }

		public BaseException(string message, Exception innerException) : base(message, innerException) { }
		public static T Create(string errorMessage)
		{
			return (T)Activator.CreateInstance(typeof(T), errorMessage);
		}
		public static T Create(string message, Exception innerException)
		{
			return (T)Activator.CreateInstance(typeof(T), message, innerException);
		}
		public Dictionary<string, IEnumerable<string>> Errors
		{
			get; set;
		}
	}
}
