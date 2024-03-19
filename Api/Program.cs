var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAzureClients(cb => {
    cb.AddTableServiceClient(builder.Configuration.GetConnectionString("AzureTableStorage"));
});

builder.Services.AddMassTransit(x => {
    x.UsingRabbitMq((context, cfg) => {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], h => {
            h.Password(builder.Configuration["RabbitMQ:Password"]);
            h.Username(builder.Configuration["RabbitMQ:UserName"]);
        });

        cfg.Publish<ViewSongMessage>(pCfg => {
            pCfg.BindQueue("view-exchange", "view-queue", c => { c.ExchangeType = "topic"; });
            pCfg.Durable = true;
            pCfg.AutoDelete = false;
        });
        cfg.Send<ViewSongMessage>(sendTopology => { sendTopology.UseRoutingKeyFormatter(c => c.Message.RoutingKey); });
    });
});

builder.Services.AddScoped<IRepository<Song>, SongRepository>();
builder.Services.AddScoped<IRepository<SongView>, SongViewRepository>();
builder.Services.AddScoped<IRepository<TopSong>, TopSongRepository>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
    if (type.GetInterface(nameof(IMinimalApiEndpoint)) != null)
        ((IMinimalApiEndpoint)Activator.CreateInstance(type)!).RegisterRoutes(app);


app.Run();