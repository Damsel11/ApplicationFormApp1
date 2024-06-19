using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSingleton(s =>
{
    var cosmosClientOptions = new CosmosClientOptions
    {
        SerializerOptions = new CosmosSerializationOptions
        {
            PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
        }
    };
    return new CosmosClient(builder.Configuration["CosmosDb:AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="], cosmosClientOptions);
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();