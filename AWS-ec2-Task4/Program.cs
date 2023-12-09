using Amazon.Util;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/aws-region", () =>
{
    var region = EC2InstanceMetadata.Region;

    if (region is null)
    {
        app.Logger.LogError("Failed to retrieve region.");
    }

    var availabilityZone = EC2InstanceMetadata.AvailabilityZone;

    if (string.IsNullOrEmpty(availabilityZone))
    {
        app.Logger.LogError("Failed to retrieve availability zone.");
    }

    return new { region, availabilityZone };
})
.WithName("GetAWSRegion");

app.Run();