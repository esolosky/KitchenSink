using RiderDevTest.Extensions;

var builder = WebApplication.CreateBuilder(args);

var app = await builder.ConfigureServices().ConfigurePipeline();

app.Run();