using backend;
using backend.DAL;
using backend.Interface;
using backend.Service;
using backend.WebSockets.MessageHandlers;
using backend.WebSockets.MessageHandlers.OrderHandlers;

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
builder.Services.AddSingleton<OrderOptionDAL>();
builder.Services.AddSingleton<TokenDAL>();
builder.Services.AddSingleton<IUserDAL,UserDAL>();
builder.Services.AddSingleton<ITokenDAL,TokenDAL>();
builder.Services.AddSingleton<IRegisterCustomerDAL,RegisterCustomerDAL>();

// Add services to the container.
builder.Services.AddSingleton<IOrderOptionService, OrderOptionService>();
builder.Services.AddSingleton<ITokenService,TokenService>();
builder.Services.AddSingleton<IUserService,UserService>();
builder.Services.AddSingleton<RegisterCustomerService>();

// Add message handlers to the container.
builder.Services.AddSingleton<LoginMessageHandler>();
builder.Services.AddSingleton<RegisterCustomerHandler>();
builder.Services.AddSingleton<RegisterPersonnelHandler>();

//Authentication handler
builder.Services.AddSingleton<AuthenticationHandler>();
//OrderOption handlers, create, read, update and delete
builder.Services.AddSingleton<OrderOptionCreateHandler>();
builder.Services.AddSingleton<OrderOptionReadHandler>();
builder.Services.AddSingleton<OrderOptionUpdateHandler>();
builder.Services.AddSingleton<OrderOptionDeleteHandler>();

//OrderFromCustomerHandler
builder.Services.AddSingleton<OrderFromCustomerHandler>();


builder.Services.AddSingleton<MqttClientDAL>();
builder.Services.AddSingleton<MqttClientService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

// Instantiate the LoginMessageHandler and store it as an variable.
IMessageHandler loginHandler = builder.Services.BuildServiceProvider().GetRequiredService<LoginMessageHandler>();

// Register handlers for customer and personnel
IMessageHandler registerHandler = builder.Services.BuildServiceProvider().GetRequiredService<RegisterCustomerHandler>();
IMessageHandler registerPersonnelHandler = builder.Services.BuildServiceProvider().GetRequiredService<RegisterPersonnelHandler>();

//OrderOption handlers, create, read, update and delete
IMessageHandler orderOptionCreateHandler = builder.Services.BuildServiceProvider().GetRequiredService<OrderOptionCreateHandler>();
IMessageHandler orderOptionReadHandler = builder.Services.BuildServiceProvider().GetRequiredService<OrderOptionReadHandler>();
IMessageHandler orderOptionDeleteHandler = builder.Services.BuildServiceProvider().GetRequiredService<OrderOptionDeleteHandler>();
IMessageHandler orderOptionUpdateHandler = builder.Services.BuildServiceProvider().GetRequiredService<OrderOptionUpdateHandler>();

IMessageHandler orderFromCustomerHandler = builder.Services.BuildServiceProvider().GetRequiredService<OrderFromCustomerHandler>();

//Authentication handler for all roles, admin, personnel and customer
IMessageHandler authenticationHandler = builder.Services.BuildServiceProvider().GetRequiredService<AuthenticationHandler>();

//a dictionary mapping message types to handlers.
Dictionary<string, IMessageHandler> messageHandlers = new Dictionary<string, IMessageHandler>
{
    { "login", loginHandler },
    {"register", registerHandler},
    {"registerPersonnel", registerPersonnelHandler },
    {"authentication", authenticationHandler},
    {"orderOptionCreate", orderOptionCreateHandler},
    {"orderOptionRead", orderOptionReadHandler},
    {"orderOptionUpdate", orderOptionUpdateHandler},
    {"orderOptionDelete", orderOptionDeleteHandler},
    {"orderFromCustomer", orderFromCustomerHandler}
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

app.Services.GetService<MqttClientService>().CommunicateWithBroker();

app.Run();