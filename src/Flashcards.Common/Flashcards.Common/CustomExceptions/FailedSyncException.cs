namespace Flashcards.Common.CustomExceptions
{
	public class FailedSyncException : Exception
	{
        public FailedSyncException() { }

        public FailedSyncException(string message) : base(message) { }

		public FailedSyncException(string message, Exception inner)
		: base(message, inner) { }
	}
}
