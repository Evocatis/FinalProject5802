# Data Obfuscation with Strategy & Factory Patterns

This project implements data obfuscation using the **Strategy** design pattern in combination with a simple **Factory** pattern to dynamically assign obfuscation behavior based on user roles. The goal is to ensure that sensitive user information is selectively hidden or shown depending on the permission level of the requestor, while keeping the architecture clean, extensible, and easy to maintain.

## Why Strategy?

The **Strategy** pattern allows us to encapsulate different obfuscation behaviors into isolated classes. Each role (e.g., `SuperAdmin`, `Admin`, `NonPrivileged`) has its own `Obfuscate()` implementation, defining exactly what user fields should be exposed. This approach avoids cluttering a single class with conditional logic (if/else or switch statements), and instead favors composition over inheritance, making it easier to test, debug, and extend.

Rather than scattering permission-based formatting throughout the codebase, each strategy class clearly represents a single, well-defined responsibility. For example, the `AdminObfuscationStrategy` includes most user information but deliberately omits contact details, while the `NonPrivilegedStrategy` restricts output to just a user's name.

## Why a Factory?

To avoid placing the burden of choosing the right strategy on calling code, we use a **Factory** that selects the correct `IDataObfuscationStrategy` implementation based on the current user's role. This encapsulates decision-making logic and ensures consistent strategy assignment. The Factory design also makes future role-based behavior changes easy—just add a new role-strategy pair without touching existing logic.

## Extensibility

By using an abstract base class (`BaseObfuscationStrategy`) to handle shared logic like constructor setup, we reduce redundancy across strategy classes. If new roles are added in the future (like `Manager`, `Auditor`, or `Guest`), we can support them simply by creating a new class that inherits from the base and overrides the `Obfuscate()` method. There's no risk of breaking existing behavior, and no need to modify core logic elsewhere in the application.

## Adding a New Obfuscation Strategy

One of the key benefits of using the **Strategy Pattern** is how easy it is to extend the system with new behavior. This section will walk you through adding a new obfuscation rule for a user role, step-by-step. We'll use a hypothetical `Manager` role that displays only the user's name and state.

### Step 1: Create the Strategy Class

Add a new class that inherits from `BaseObfuscationStrategy` and implements the `Obfuscate()` method. This class encapsulates exactly what information the `Manager` role is allowed to see.

Example:

```csharp
public class ManagerObfuscationStrategy : BaseObfuscationStrategy
{
    public ManagerObfuscationStrategy(UserData user) : base(user) { }

    public override string Obfuscate()
    {
        return $"{User.FirstName} {User.LastName}, {User.State}";
    }
}

```

### Step 2: Register the Strategy in the Factory

Update your `ObfuscationStrategyFactory` to return your new strategy when the "Manager" role is encountered.

Example:

```csharp
public static class ObfuscationStrategyFactory
{
    public static IDataObfuscationStrategy Create(UserData user, string role)
    {
        return role switch
        {
            "SuperAdmin" => new SuperAdminObfuscationStrategy(user),
            "Admin" => new AdminObfuscationStrategy(user),
            "Manager" => new ManagerObfuscationStrategy(user),
            "User" => new NonPrivilegedStrategy(user),
            _ => throw new ArgumentException("Invalid role")
        };
    }
}
