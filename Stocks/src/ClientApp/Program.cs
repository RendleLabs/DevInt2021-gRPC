using Market.Protos;
using Portfolios.Protos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var portfoliosUri = builder.Configuration.GetValue<Uri>("Services:Portfolios");
builder.Services.AddGrpcClient<PortfolioService.PortfolioServiceClient>(options => options.Address = portfoliosUri);

var marketUri = builder.Configuration.GetValue<Uri>("Services:Market");
builder.Services.AddGrpcClient<MarketService.MarketServiceClient>(options => options.Address = marketUri);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
