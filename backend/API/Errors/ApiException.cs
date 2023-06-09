namespace API.Errors
{
  public class ApiException
  {
    public ApiException(int statusCode, string message, string deatils)
    {
      StatusCode = statusCode;
      Message = message;
      Deatils = deatils;
    }

    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string Deatils { get; set; }
  }
}