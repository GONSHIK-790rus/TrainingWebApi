using TrainingWebAPI.Models;

namespace TrainingWebAPI.Services;

interface IUserService {
    public User Create(User user);
    public List<User> GetAll();
    public User? GetById(Guid id);
    public bool DeleteById(Guid id);
    public User? Update(Guid id, User newUser);
    public User? Patch(Guid id, UserPatch userPatch);
}
