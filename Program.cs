using AzNotesSample.Configuration;
using AzNotesSample.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddSingleton<IStorage, MemoryStorage>();
builder.Services.AddScoped<IStorage, BlobStorage>();

builder.Services.Configure<StorageOptions>(builder.Configuration);
builder.Services.AddSingleton<StorageConnectionString>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();
