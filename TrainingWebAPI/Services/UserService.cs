using TrainingWebAPI.Models;

namespace TrainingWebAPI.Services;

public class UserService : IUserService {
    private readonly UserIdGenerator _idGenerator;

    private List<User> Users { get; } = [];

    public UserService(UserIdGenerator idGenerator) {
        _idGenerator = idGenerator;
    }

    public User Create(User user) {
        user.Id = _idGenerator.Create();

        Users.Add(user);

        return user;
    }

    public List<User> GetAll() {
        return Users;
    }

    public User? GetById(Guid id) {
        return Users.FirstOrDefault(user => user.Id == id);
    }

    public bool DeleteById(Guid id) { 
        var user = GetById(id);

        if (user == null)
            return false;
        
        return Users.Remove(user);
    }

    public User? Update(Guid id, User newUser) {
        var user = GetById(id);

        if (user == null)
            return null;

        user.Name = newUser.Name;
        user.Age = newUser.Age;
        user.IsActive = newUser.IsActive;

        return user;
    }

    public User? Patch(Guid id, UserPatch userPatch) {
        var user = GetById(id);

        if (user == null)
            return null;

        if (userPatch.Name != null)
            user.Name = userPatch.Name;

        if (userPatch.Age != null)
            user.Age = userPatch.Age.Value;

        if (userPatch.IsActive != null)
            user.IsActive = userPatch.IsActive.Value;

        return user;
    }
}