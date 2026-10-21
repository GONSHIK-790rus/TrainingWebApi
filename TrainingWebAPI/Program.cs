using Microsoft.EntityFrameworkCore;

using TrainingWebAPI.Models;
using TrainingWebAPI.Services;
using TrainingWebAPI.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<UserIdGenerator>();
builder.Services.AddScoped<IUserService, UserService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseNpgsql(connectionString);
});
    
var app = builder.Build();

app.MapGet("/users", async (IUserService userService) => {
    var users = await userService.GetAllAsync();

    return Results.Ok(users);
});

app.MapGet("/db-check", async (AppDbContext appDbContext) => {
    if (!await appDbContext.Database.CanConnectAsync())
        return Results.Problem(
            "Database unavailable",
            statusCode: 503
        );

    return Results.Ok();
});

app.MapPost("/users", async (User user, IUserService userService) => {
    if (string.IsNullOrWhiteSpace(user.Name) || user.Age <= 0)
        return Results.BadRequest();

    var createdUser = await userService.CreateAsync(user);

    return Results.Created($"/users/{createdUser.Id}", createdUser);
});

app.MapGet("/users/{id}", async (Guid id, IUserService userService) => {
    User? user = await userService.GetByIdAsync(id);

    if (user == null)
        return Results.NotFound();

    return Results.Ok(user);
});

app.MapDelete("/users/{id}", async (Guid id, IUserService userService) => {
    if (!await userService.DeleteByIdAsync(id))
        return Results.NotFound();

    return Results.NoContent();
});

app.MapPut("/users/{id}", async (Guid id, User newUser, IUserService userService) => {
    if (string.IsNullOrWhiteSpace(newUser.Name) || newUser.Age <= 0)
        return Results.BadRequest();

    var updatedUser = await userService.UpdateAsync(id, newUser);

    if (updatedUser == null)
        return Results.NotFound();

    return Results.Ok(updatedUser);
});

app.MapPatch("/users/{id}", async (Guid id, UserPatch userPatch, IUserService userService) => {
    if (userPatch.Name != null && string.IsNullOrWhiteSpace(userPatch.Name))
        return Results.BadRequest();

    if (userPatch.Age != null && userPatch.Age <= 0)
        return Results.BadRequest();

    var updatedUser = await userService.PatchAsync(id, userPatch);

    if (updatedUser == null)
        return Results.NotFound();

    return Results.Ok(updatedUser);
});

app.Run();
