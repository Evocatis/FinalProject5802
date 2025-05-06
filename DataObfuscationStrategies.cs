public interface IDataObfuscationStrategy
{
    string Obfuscate();
}

// Abstract base class for shared UserData handling
public abstract class BaseObfuscationStrategy(UserData user) : IDataObfuscationStrategy
{
    protected readonly UserData User = user;

    public abstract string Obfuscate();
}

// SuperAdmin sees everything
public class SuperAdminObfuscationStrategy(UserData user) : BaseObfuscationStrategy(user)
{
    public override string Obfuscate()
    {
        return $"{User.FirstName} {User.LastName}, {User.Address}, {User.State}, {User.Country}, {User.Email}, {User.Phone}";
    }
}

// Admin sees most, but no contact info
public class AdminObfuscationStrategy(UserData user) : BaseObfuscationStrategy(user)
{
    public override string Obfuscate()
    {
        return $"{User.FirstName} {User.LastName}, {User.Address}, {User.State}, {User.Country}";
    }
}

// Non-privileged users see only names
public class NonPrivilegedStrategy(UserData user) : BaseObfuscationStrategy(user)
{
    public override string Obfuscate()
    {
        return $"{User.FirstName} {User.LastName}";
    }
}