# Data Obfuscation with Strategy and Decorator Patterns

This project demonstrates a combined use of the **Strategy** and **Decorator** design patterns to support flexible and extensible data obfuscation logic. It is a console application that processes a set of users and, depending on the user's role and options, selectively hides or formats sensitive fields.

The goal is to show how Strategy can control *what data is shown* and Decorator can control *how it's displayed or enhanced*, all while keeping the architecture modular and open to future changes.

---

## How It Works

- The **Strategy Pattern** is used to define different obfuscation rules for each user role (e.g., `Admin`, `SuperAdmin`, `User`).
- The **Decorator Pattern** is layered on top to apply additional formatting or behavior (e.g., wrapping in HTML, adding disclaimers).
- A **Factory method** selects the strategy based on the role and dynamically applies one or more decorators based on command-line input.

---

## Why Strategy?

Rather than writing role logic using `if/else` blocks, we encapsulate role-based behavior in separate classes:

- `AdminObfuscationStrategy` shows limited info.
- `SuperAdminObfuscationStrategy` shows everything.
- `NonPrivilegedStrategy` restricts output to a basic set.

This follows the **Open/Closed Principle**: we can add new roles by creating new classes, without touching existing logic.

---

## Why Decorator?

Decorator allows us to enhance or modify the result of a strategy *without changing the strategy itself*. It’s ideal for layering cross-cutting concerns such as:

- Wrapping output in HTML tags
- Adding disclaimers
- Logging or timestamping (extensible)

This keeps our role logic clean and allows behavior stacking.

## Strategy Implementation Overview

At the heart of the design is an interface (`IDataObfuscation`) with a single method:

```csharp
public interface IDataObfuscationStrategy
{
    string Obfuscate();
}
```

Each strategy class implements this interface and receives a UserData object, then determines which fields to reveal based on that role’s permissions.

A base class (BaseObfuscationStrategy) provides shared setup logic like holding a reference to the UserData so individual strategy classes can stay focused on their unique obfuscation behavior.

Here's what a simple strategy might look like:

```csharp
public class NonPrivilegedStrategy : BaseObfuscationStrategy
{
    public NonPrivilegedStrategy(UserData user) : base(user) { }

    public override string Obfuscate()
    {
        return $"{User.FirstName} {User.LastName}";
    }
}
```

In the calling code, we assign a strategy based on the user's role using a simple switch statement (or another mapping structure), then delegate to the chosen strategy:

```csharp
IDataObfuscationStrategy strategy = role switch
{
    "SuperAdmin" => new SuperAdminObfuscationStrategy(user),
    "Admin" => new AdminObfuscationStrategy(user),
    "User" => new NonPrivilegedStrategy(user),
    _ => throw new ArgumentException("Invalid role")
};

var obfuscated = strategy.Obfuscate();

```

Even though we use a switch here, the actual logic is offloaded into the separate strategy classes, keeping our control flow light and our business logic decoupled.

### Decorator Implementation Overview
Once a strategy has determined what data to show, decorators allow us to enhance or modify how that data is presented, without touching the underlying logic.

At the heart of the design is a common interface (IDataObfuscation) which is implemented by both concrete strategies and decorators. This means decorators can wrap strategies, or even other decorators, in a flexible chain.

Here's what a simple decoratro might look like:

```csharp
public class HtmlWrapperDecorator : ObfuscationDecorator
{
    public HtmlWrapperDecorator(IDataObfuscation inner) : base(inner) { }

    public override string Obfuscate()
    {
        return $"<div class='user'>{Inner.Obfuscate()}</div>";
    }
}
```

### Applying Decorators with a Factory
Decorators are dynamically selected using DataDecoratorsFactory, which takes a list of keys and applies them in order:

```csharp
baseStrategy = decorator switch
{
    "html" => new HtmlWrapperDecorator(baseStrategy),
    "disclaimer" => new DisclaimerDecorator(baseStrategy),
    _ => baseStrategy
};
```

### Putting it together
After choosing a strategy, decorators are added as needed based on user input:

```csharp
var strategy = new AdminObfuscationStrategy(user);
var decorated = DataDecoratorsFactory.ApplyDecorators(strategy, new List<string> { "html", "disclaimer" });

Console.WriteLine(decorated.Obfuscate());
```

## Extensibility

Adding support for a new role only requires creating a new class that inherits from BaseObfuscationStrategy and implements the Obfuscate() method. There's no need to touch existing strategies, which reduces the risk of regression.

This makes the system very maintainable. If a new role like Manager, Auditor, or Guest is introduced, it can be integrated in a few lines without touching the rest of the codebase.

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
```

### Step 3: Use the Decorator

Now you can include "Manager" in your command line arguments to apply the new strategy:

```bash
dotnet run manager
```

## Adding a new Obfuscation Decorator

One of the main benefits of the Decorator Pattern is its support for layering behavior without modifying existing code. This section walks through adding a new output modifier, let’s say a TimestampDecorator that appends the current UTC time to the obfuscation result.

### Create the Decorator Class

Create a new class that inherits from ObfuscationDecorator and overrides the Obfuscate() method to add timestamp functionality.

```csharp
public class TimestampDecorator : ObfuscationDecorator
{
    public TimestampDecorator(IDataObfuscation inner) : base(inner) { }

    public override string Obfuscate()
    {
        return $"{Inner.Obfuscate()}\nGenerated at: {DateTime.UtcNow:u}";
    }
}
```

This class simply appends a UTC timestamp to whatever output the wrapped strategy or decorator provides.

### Step 2: Register the Decorator in the Factory

Add the new decorator keyword to the DataDecoratorsFactory so it can be invoked dynamically at runtime:

```csharp
public static class DataDecoratorsFactory
{
    public static IDataObfuscation ApplyDecorators(IDataObfuscation baseStrategy, List<string> decorators)
    {
        foreach (var decorator in decorators)
        {
            baseStrategy = decorator switch
            {
                "html" => new HtmlWrapperDecorator(baseStrategy),
                "disclaimer" => new DisclaimerDecorator(baseStrategy),
                "timestamp" => new TimestampDecorator(baseStrategy),
                _ => baseStrategy
            };
        }

        return baseStrategy;
    }
}
```

### Step 3: Use the Decorator

Now you can include "timestamp" in your command-line args or configuration to apply the new behavior:

```bash
dotnet run admin timestamp
```