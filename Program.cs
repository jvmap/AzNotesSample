using AzNotesSample.Configuration;
using AzNotesSample.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton(StorageFactory.CreateStorage);
builder.Services.Configure<StorageOptions>(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();
