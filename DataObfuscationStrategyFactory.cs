public class DataObfuscationStrategyFactory
{
    public static IDataObfuscationStrategy CreateObfuscationStrategy(string strategyType, UserData userData)
    {
        return strategyType switch
        {
            "SuperAdmin" => new SuperAdminObfuscationStrategy(userData),
            "Admin" => new AdminObfuscationStrategy(userData),
            _ => new NonPrivilegedStrategy(userData),
        };
    }
}