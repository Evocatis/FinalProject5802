public class WhyWeUseStrategies
{
    public static string CreateObfuscationOutput(string baseUser, UserData userData)
    {
        if (baseUser == "SuperAdmin")
        {
            // SuperAdmin sees everything
            return $"{userData.FirstName} {userData.LastName}, {userData.Address}, {userData.State}, {userData.Country}, {userData.Email}, {userData.Phone}";
        }
        else if (baseUser == "Admin")
        {
            // Admin's view depends on the role of the user they are viewing
            if (userData.Role == "SuperAdmin" || userData.Role == "Admin")
            {
                // Admin viewing SuperAdmin or Admin => NonPrivileged view (just names)
                return $"{userData.FirstName} {userData.LastName}";
            }
            else if (userData.Role == "User" || userData.Role == "Driver")
            {
                // Admin viewing User or Driver => no contact info
                return $"{userData.FirstName} {userData.LastName}, {userData.Address}, {userData.State}, {userData.Country}";
            }
            else if (userData.Role == "American")
            {
                // Admin viewing American => country only
                return $"{userData.Country}";
            }
            else
            {
                // Fallback to non-privileged view
                return baseUser == userData.Role
                    ? $"{userData.FirstName} {userData.LastName}, {userData.Address}, {userData.State}, {userData.Country}"
                    : $"{userData.FirstName} {userData.LastName}";
            }
        }
        else
        {
            // Non-admins get a more limited view
            if (baseUser == userData.Role)
            {
                return $"{userData.FirstName} {userData.LastName}, {userData.Address}, {userData.State}, {userData.Country}";
            }
            else
            {
                return $"{userData.FirstName} {userData.LastName}";
            }
        }
    }
}
