using MalirosoBlog.API.DataSeeder;
using MalirosoBlog.API.Extensions;
using MalirosoBlog.Data.Context;
using MalirosoBlog.Models.Entities;
using MalirosoBlog.Services.Extensions;
using MalirosoBlog.Services.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

string root = Directory.GetCurrentDirectory();
string dotenv = Path.Combine(root, ".env");
Dotenv.Load(dotenv);

var builder = WebApplication.CreateBuilder(args);

/*builder.Services.AddDbContext<MailRosoBlogDbContext>(
    options => options.UseInMemoryDatabase("AppDb"));*/

string partialDbConnString = builder.Configuration.GetConnectionString("DefaultConnection")!;
string connectionString = builder.Configuration.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>()!.BuildConnectionString(partialDbConnString!);

builder.Services.AddDbContext<MailRosoBlogDbContext>(options =>
{
    options.UseSqlServer(connectionString, s =>
    {
        s.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(20),
            errorNumbersToAdd: null);
    });

});

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(config =>
{
    config.User.RequireUniqueEmail = true;
    config.Password.RequiredLength = 8;
    config.Password.RequireDigit = true;
    config.Password.RequiredUniqueChars = 0;
    config.Password.RequireNonAlphanumeric = true;
    config.Password.RequireUppercase = true;
    config.Password.RequireLowercase = true;
}).AddEntityFrameworkStores<MailRosoBlogDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
        .AddJwtBearer(jwt =>
        {
            JWTConfiguration jwtConfig = builder.Configuration.GetSection(nameof(JWTConfiguration)).Get<JWTConfiguration>()!;
            var key = Encoding.ASCII.GetBytes(jwtConfig.Secret);

            jwt.SaveToken = true;
            jwt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidAudience = jwtConfig.Audience,
                ClockSkew = TimeSpan.Zero
            };
        });


builder.Services.AddAuthorization(cfg =>
{
    cfg.AddPolicy("Authorization", policy => policy.Requirements.Add(new AuthorizationRequirement()));
    cfg.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    cfg.AddPolicy("AuthorsOnly", policy => policy.RequireRole("Author"));
});

builder.Services.BindConfigurations(builder.Configuration);

// Configure SeedData
builder.Services.BindSeedDataConfiguration(builder.Configuration);

//Add automapper
builder.Services.AddAutoMapper(Assembly.Load("MalirosoBlog.Models"));

builder.Services.RegisterServices();

builder.Services.AddHttpContextAccessor();

//builder.Services.AddControllers(setupAction => { setupAction.Filters.Add<ValidateModelAttribute>(); });

builder.Services.AddControllers(setupAction => { setupAction.ReturnHttpNotAcceptable = true; }).AddJsonOptions(
    options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());


    }).AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

    });

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyHeader()
           .AllowAnyMethod()
           .WithOrigins(
            "http://localhost:44356"
           )
           .AllowCredentials();
}));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MalirosoBlogAPI", Version = "v1" });


    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\""
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            },
    });
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.ContentType = "application/json";

        IExceptionHandlerFeature? exceptionHandleFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (exceptionHandleFeature != null)
        {
            var status = ResponseStatus.FATAL_ERROR;
            switch (exceptionHandleFeature.Error)
            {
                case InvalidOperationException:
                case ArgumentNullException:
                case ArgumentException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    status = ResponseStatus.APP_ERROR;
                    break;
                case UnauthorizedAccessException:
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    status = ResponseStatus.UNAUTHORIZED;
                    break;
                case DbUpdateException:
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    status = ResponseStatus.APP_ERROR;
                    break;
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            ErrorResponse err = new() { Success = false, Status = status };

            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            string msg = JsonConvert.SerializeObject(err, serializerSettings);

            await context.Response.WriteAsync(msg);
        }
    });
});

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MalirosoBlog v1");
    c.InjectStylesheet("/css/swagger-dark-theme.css");
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await SeedAppData.EnsurePopulated(app);

app.Run();
