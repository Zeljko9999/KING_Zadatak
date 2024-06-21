namespace ProductAPI.Exceptions
{

    // A type of exception that will be passed to the user
    public class UserErrorMessage : Exception
    {
        public UserErrorMessage(string? message) : base(message)
        {
        }
    }
}
