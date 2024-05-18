public interface IPasswordHasher {
    string HashPassword(string password);
    bool VerifyPassword(string hashedPassword, string plainPassword);
}

public class BcryptPasswordHasher : IPasswordHasher {
    public string HashPassword(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string hashedPassword, string plainPassword) {
        return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }
}