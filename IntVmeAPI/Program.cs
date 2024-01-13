using AppAny.HotChocolate.FluentValidation;
using FirebaseAdmin;
using FirebaseAdminAuthentication.DependencyInjection.Extensions;
using FirebaseAdminAuthentication.DependencyInjection.Models;
using Google.Apis.Auth.OAuth2;
using IntVmeAPI.DataLoaders;
using IntVmeAPI.Domain.Courses;
using IntVmeAPI.Schema.Mutations;
using IntVmeAPI.Schema.Queries;
using IntVmeAPI.Services;
using IntVmeAPI.Services.Courses;
using IntVmeAPI.Services.Instructors;
using IntVmeAPI.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string? connectionString = builder.Configuration.GetConnectionString("default");
builder.Services.AddPooledDbContextFactory<SchoolDBContext>(o => o.UseSqlServer(connectionString).LogTo(Console.WriteLine));

var firebaseConfig = builder.Configuration.GetValue<string>("FIREBASE_CONFIG");
var googleCredentials = GoogleCredential.FromJson(firebaseConfig);
var firebase = FirebaseApp.Create(new AppOptions()
{
    Credential = googleCredentials
});

builder.Services.AddScoped<CourseUseCases>();
builder.Services.AddScoped<CoursesRepository>();
builder.Services.AddScoped<InstructorsRepository>();
builder.Services.AddScoped<InstructorDataLoader>();
builder.Services.AddScoped<UserDataLoader>();
builder.Services.AddSingleton(firebase);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                  /*.AllowCredentials()*/
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var services = builder.Services;

services.AddTransient<CourseInputValidator>();
services.AddTransient<InstructorInputValidator>();

services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .RegisterDbContext<SchoolDBContext>(DbContextKind.Pooled)
    .RegisterService<CoursesRepository>(ServiceKind.Resolver)
    .RegisterService<CourseUseCases>(ServiceKind.Resolver)
    .AddSubscriptionType<Subscription>()
    .AddType<Instructor>()
    .AddType<Course>()
    .AddTypeExtension<CourseQuery>()
    .AddTypeExtension<InstructorQuery>()
    .AddTypeExtension<CourseMutation>()
    .AddTypeExtension<InstructorMutation>()
    .AddAuthorization(
                o => o.AddPolicy("IsAdmin",
                p => p.RequireClaim(FirebaseUserClaimType.EMAIL, "hyurii3@gmail.com")))
    .AddInMemorySubscriptions()
    .AddFluentValidation(o =>
    {
        o.UseDefaultErrorMapper();
    });

services.AddFirebaseAuthentication();







var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IDbContextFactory<SchoolDBContext> contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<SchoolDBContext>>();
    using (SchoolDBContext context = contextFactory.CreateDbContext())
    {
        context.Database.Migrate();
    }
}
// TODO: only specific domains
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseWebSockets();

app.MapGraphQL();
app.Run();
