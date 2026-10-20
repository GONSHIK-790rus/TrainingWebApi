using TrainingWebAPI.Models;
using TrainingWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<UserIdGenerator>();
builder.Services.AddSingleton<IUserService, UserService>();
var app = builder.Build();

app.MapPost("/users", (User user, IUserService userService) => {
    if (string.IsNullOrWhiteSpace(user.Name) || user.Age <= 0)
        return Results.BadRequest();

    var createdUser = userService.Create(user);

    return Results.Created($"/users/{createdUser.Id}", createdUser);
});

app.MapGet("/users", (IUserService userService) => {
    return Results.Ok(userService.GetAll());
});

app.MapGet("/users/{id}", (Guid id, IUserService userService) => {
    User? user = userService.GetById(id);

    if (user == null)
        return Results.NotFound();

    return Results.Ok(user);
});

app.MapDelete("/users/{id}", (Guid id, IUserService userService) => {
    if (!userService.DeleteById(id))
        return Results.NotFound();

    return Results.NoContent();
});

app.MapPut("/users/{id}", (Guid id, User newUser, IUserService userService) => {
    if (string.IsNullOrWhiteSpace(newUser.Name) || newUser.Age <= 0)
        return Results.BadRequest();

    var updatedUser = userService.Update(id, newUser);

    if (updatedUser == null)
        return Results.NotFound();

    return Results.Ok(updatedUser);
});

app.MapPatch("/users/{id}", (Guid id, UserPatch userPatch, IUserService userService) => {
    if (userPatch.Name != null && string.IsNullOrWhiteSpace(userPatch.Name))
        return Results.BadRequest();

    if (userPatch.Age != null && userPatch.Age <= 0)
        return Results.BadRequest();

    var updatedUser = userService.Patch(id, userPatch);

    if (updatedUser == null)
        return Results.NotFound();

    return Results.Ok(updatedUser);
});

app.Run();
