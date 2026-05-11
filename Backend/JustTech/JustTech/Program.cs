using Business.Logic.Services;
using JustTech.Business.MappingProfiles;
using JustTech.Business.Services;
using JustTech.Core.Interfaces;
using JustTech.Infrastructure.Data;
using JustTech.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add after AddDbContext and before AddControllers


// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ClockSkew = TimeSpan.Zero
    };
});

// Add Authorization After Authentication
builder.Services.AddAuthorization();


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

// Register Repositories
builder.Services.AddScoped(typeof(JustTech.Core.Interfaces.IRepository<>), typeof(JustTech.Infrastructure.Repositories.Repository<>));
builder.Services.AddScoped<ICourseRepository,CourseRepository>();
builder.Services.AddScoped<IRoundRepository, RoundRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
builder.Services.AddScoped<ILectureRepository, LectureRepository>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
// Register UnitOfWork
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

// Register Services
builder.Services.AddScoped<ICourseService,CourseService>();
builder.Services.AddScoped<IAuthService, AuthService>(); // Add IAuthService registration before var app = builder.Build()
builder.Services.AddScoped<IRoundService, RoundService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<ILectureService, LectureService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();


// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();



app.UseSwagger();
app.UseSwaggerUI();

// UseAuthentication before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
