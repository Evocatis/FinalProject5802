public class DataObfuscationStrategyFactory
{
    public static IDataObfuscation CreateObfuscationStrategy(string baseUser, UserData userData)
    {
        return baseUser switch
        {
            "SuperAdmin" => new SuperAdminObfuscationStrategy(userData),
            "Admin" => CreateAdminObfuscationFactory(baseUser, userData),
            _ => CreateNonPrivilegedObfuscationFactory(baseUser, userData),
        };
    }

    public static IDataObfuscation CreateAdminObfuscationFactory(string baseUser, UserData userData)
    {
        return userData.Role switch
        {
            "SuperAdmin" => new NonPrivilegedStrategy(userData),
            "Admin" => new NonPrivilegedStrategy(userData),
            "User" => new AdminObfuscationStrategy(userData),
            "American" => new AdminToAmerican(userData),
            "Driver" => new AdminObfuscationStrategy(userData),
            _ => CreateNonPrivilegedObfuscationFactory(baseUser, userData),
        };
    }

    public static IDataObfuscation CreateNonPrivilegedObfuscationFactory(string baseUser, UserData userData)
    {
        if (baseUser == userData.Role)
        {
            return new AdminObfuscationStrategy(userData);
        }
        else
        {
            return new NonPrivilegedStrategy(userData);
        }
    }
}