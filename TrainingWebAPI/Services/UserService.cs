using Microsoft.EntityFrameworkCore;
using TrainingWebAPI.Data;
using TrainingWebAPI.Models;

namespace TrainingWebAPI.Services;

public class UserService : IUserService {
    private readonly UserIdGenerator _idGenerator;
    private readonly AppDbContext _appDbContext;

    public UserService(UserIdGenerator idGenerator, AppDbContext appDbContext) {
        _idGenerator = idGenerator;
        _appDbContext = appDbContext;
    }

    public async Task<User> CreateAsync(User user) {
        user.Id = _idGenerator.Create();

        _appDbContext.Users.Add(user);
        await _appDbContext.SaveChangesAsync();

        return user;
    }

    public Task<List<User>> GetAllAsync() {
        return _appDbContext.Users.AsNoTracking().ToListAsync();
    }

    public Task<User?> GetByIdAsync(Guid id) {
        return _appDbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<bool> DeleteByIdAsync(Guid id) { 
        var user = await GetByIdAsync(id);

        if (user == null)
            return false;
        
        _appDbContext.Users.Remove(user);
        await _appDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<User?> UpdateAsync(Guid id, User newUser) {
        var user = await GetByIdAsync(id);

        if (user == null)
            return null;

        user.Name = newUser.Name;
        user.Age = newUser.Age;
        user.IsActive = newUser.IsActive;

        await _appDbContext.SaveChangesAsync();

        return user;
    }

    public async Task<User?> PatchAsync(Guid id, UserPatch userPatch) {
        var user = await GetByIdAsync(id);

        if (user == null)
            return null;

        if (userPatch.Name != null)
            user.Name = userPatch.Name;

        if (userPatch.Age != null)
            user.Age = userPatch.Age.Value;

        if (userPatch.IsActive != null)
            user.IsActive = userPatch.IsActive.Value;

        await _appDbContext.SaveChangesAsync();

        return user;
    }
}