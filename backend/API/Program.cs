using API.Extensions;
using API.Middleware;

internal class Program
{
  private static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();

    builder.Services.AddApplicationServices(builder.Configuration);
    builder.Services.AddIdentiyServices(builder.Configuration);

    var app = builder.Build();

    app.UseMiddleware<ExceptionMiddleware>();

    app.UseCors(builder => builder.AllowAnyHeader()
    .AllowAnyMethod().WithOrigins("https://localhost:4200"));

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
  }
}