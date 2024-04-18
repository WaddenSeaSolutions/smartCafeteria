using backend;
using backend.DAL;
using backend.Service;
using backend.WebSockets.MessageHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddSingleton<OrderService>();
builder.Services.AddSingleton<TokenService>();

// Add DALs to the container
builder.Services.AddSingleton<OrderDAL>();
builder.Services.AddSingleton<TokenDAL>();
builder.Services.AddSingleton<UserDAL>(); // Add this line

// Add services to the container.
builder.Services.AddSingleton<OrderService>();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddSingleton<UserService>(); // Add this line

// Instantiate the LoginMessageHandler and store it as an variable.
// UserService userService = new UserService(); // Remove this line
IMessageHandler loginHandler = builder.Services.BuildServiceProvider().GetRequiredService<LoginMessageHandler>();

// Create a dictionary mapping message types to handlers.
Dictionary<string, IMessageHandler> messageHandlers = new Dictionary<string, IMessageHandler>
{
    { "login", loginHandler }
};

// Instantiate the WebSocketManager with the dictionary of handlers. Should now have handlers stored in the WebSocketManager
WebSocketManager webSocketManager = new WebSocketManager(messageHandlers);

// Add the WebSocketManager to the services
builder.Services.AddSingleton(webSocketManager);


if (builder.Environment.IsDevelopment())
{
    builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString,
        dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
}
if (builder.Environment.IsProduction())
{
    builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString);
}

var app = builder.Build();


app.UseCors(options =>
{
    options.SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

app.MapControllers();

app.Run();