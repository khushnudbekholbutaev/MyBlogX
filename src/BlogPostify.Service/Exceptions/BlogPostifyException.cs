namespace BlogPostify.Service.Exceptions;

public class BlogPostifyException : Exception
{
    public int StatusCode { get; set; }
    public BlogPostifyException(int code,string message) : base(message)
    {
        this.StatusCode = code;
    }
}
