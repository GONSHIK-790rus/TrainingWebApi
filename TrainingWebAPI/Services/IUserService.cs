using TrainingWebAPI.Models;

namespace TrainingWebAPI.Services;

public interface IUserService {
    public Task<User> CreateAsync(User user);
    public Task<List<User>> GetAllAsync();
    public Task<User?> GetByIdAsync(Guid id);
    public Task<bool> DeleteByIdAsync(Guid id);
    public Task<User?> UpdateAsync(Guid id, User newUser);
    public Task<User?> PatchAsync(Guid id, UserPatch userPatch);
}
