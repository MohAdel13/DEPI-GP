using Business.Logic.Services;
using JustTech.Business.MappingProfiles;
using JustTech.Business.Services;
using JustTech.Core.Interfaces;
using JustTech.Infrastructure.Data;
using JustTech.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

// Register Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICourseRepository,CourseRepository>();

// Register UnitOfWork
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

// Register Services
builder.Services.AddScoped<ICourseService,CourseService>();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();



app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
