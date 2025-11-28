using DWM.BusinessLogic;
using DWM.Persistence;
using Microsoft.AspNetCore.Cors;
using TGH.Common.Persistence.Interfaces;
using TGH.Common.Repository.Implementations;
using TGH.Common.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IDatabaseContext, DWMBestiarySqliteDbContext>
(
	builder => new DWMBestiarySqliteDbContext(PersistenceConstants.SQLITE_DB_TARGET_FILEPATH)
);

builder.Services.AddScoped<IGenericRepository, GenericRepository>();

//TODO: Add concrete service implementations here:
builder.Services.AddScoped<MonsterFamilyTreeService>();
builder.Services.AddScoped<MonsterDetailsService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors
(
	options =>
	{
		options.AddPolicy
		(
			"AllowAllPolicy",
			builder =>
				builder
					.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader()
		);
	}
);

builder.WebHost.UseUrls("http://localhost:5000");

var app = builder.Build();

app.UseCors("AllowAllPolicy");
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
