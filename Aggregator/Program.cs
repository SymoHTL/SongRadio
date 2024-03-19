var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IRepository<Song>, SongRepository>();
builder.Services.AddScoped<IRepository<SongView>, SongViewRepository>();
builder.Services.AddScoped<IRepository<TopSong>, TopSongRepository>();

builder.Services.AddAzureClients(cb => {
    cb.AddTableServiceClient(builder.Configuration.GetConnectionString("AzureTableStorage"));
});

builder.Services.AddMassTransit(x => {
    x.AddConsumer<ViewSongMessageConsumer>();

    x.UsingRabbitMq((context, cfg) => {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], h => {
            h.Password(builder.Configuration["RabbitMQ:Password"]);
            h.Username(builder.Configuration["RabbitMQ:UserName"]);
        });

        cfg.ReceiveEndpoint(e => {
            e.Bind("view-exchange", pCfg => {
                pCfg.Durable = true;
                pCfg.AutoDelete = false;
                pCfg.ExchangeType = "topic";
            });
            e.ConfigureConsumer<ViewSongMessageConsumer>(context);
        });
    });
});

builder.Services.AddScoped<ViewSongMessageConsumer>();

var app = builder.Build();

app.Run();