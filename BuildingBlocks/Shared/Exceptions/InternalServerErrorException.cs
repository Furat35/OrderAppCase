namespace Shared.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        public InternalServerErrorException(string message = "Unexpected error occured! Please try again.") : base(message)
        {

        }
    }
}
