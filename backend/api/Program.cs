using backend;
using backend.DAL;
using backend.Interface;
using backend.Service;
using backend.WebSockets.MessageHandlers;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString,
        dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
}
if (builder.Environment.IsProduction())
{
    builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString);
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DALs to the container
builder.Services.AddSingleton<OrderDAL>();
builder.Services.AddSingleton<TokenDAL>();
builder.Services.AddSingleton<IUserDAL,UserDAL>();
builder.Services.AddSingleton<ITokenDAL,TokenDAL>();
builder.Services.AddSingleton<IRegisterCustomerDAL,RegisterCustomerDAL>();

// Add services to the container.
builder.Services.AddSingleton<UserService>();


// Add services to the container.
builder.Services.AddSingleton<IOrderService, OrderService>();
builder.Services.AddSingleton<ITokenService,TokenService>();
builder.Services.AddSingleton<IUserService,UserService>();

builder.Services.AddSingleton<RegisterCustomerService>();

// Add message handlers to the container.
builder.Services.AddSingleton<LoginMessageHandler>();

builder.Services.AddSingleton<RegisterCustomerHandler>();

builder.Services.AddSingleton<RegisterPersonnelHandler>();

builder.Services.AddSingleton<AuthenticationHandler>();

builder.Services.AddSingleton<OrderOptionCreateHandler>();

builder.Services.AddSingleton<OrderOptionDeleteHandler>();

builder.Services.AddSingleton<OrderOptionDeleteHandler>();


builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

// Instantiate the LoginMessageHandler and store it as an variable.
IMessageHandler loginHandler = builder.Services.BuildServiceProvider().GetRequiredService<LoginMessageHandler>();

IMessageHandler registerHandler = builder.Services.BuildServiceProvider().GetRequiredService<RegisterCustomerHandler>();

IMessageHandler registerPersonnelHandler = builder.Services.BuildServiceProvider().GetRequiredService<RegisterPersonnelHandler>();

IMessageHandler orderOptionCreateHandler = builder.Services.BuildServiceProvider().GetRequiredService<OrderOptionCreateHandler>();

IMessageHandler orderOptionDeleteHandler = builder.Services.BuildServiceProvider().GetRequiredService<OrderOptionDeleteHandler>();

IMessageHandler orderOptionUpdateHandler = builder.Services.BuildServiceProvider().GetRequiredService<OrderOptionUpdateHandler>();

IMessageHandler adminAuthenticationHandler = builder.Services.BuildServiceProvider().GetRequiredService<AuthenticationHandler>();

//a dictionary mapping message types to handlers.
Dictionary<string, IMessageHandler> messageHandlers = new Dictionary<string, IMessageHandler>
{
    { "login", loginHandler },
    {"register", registerHandler},
    {"registerPersonnel", registerPersonnelHandler },
    {"authentication", adminAuthenticationHandler},
    {"orderOptionCreate", orderOptionCreateHandler},
    {"orderOptionDelete", orderOptionDeleteHandler},
    {"orderOptionUpdate", orderOptionUpdateHandler}
};

// Instantiate the WebSocketManager with the dictionary of handlers. Should now have handlers stored in the WebSocketManager
WebSocketManager webSocketManager = new WebSocketManager(messageHandlers);

// Add the WebSocketManager to the services
builder.Services.AddSingleton(webSocketManager);

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