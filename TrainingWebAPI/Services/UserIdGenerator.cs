namespace TrainingWebAPI.Services;

public class UserIdGenerator {
    public Guid Create() {
        return Guid.NewGuid();
    }
}
