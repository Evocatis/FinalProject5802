﻿// Description: This C# program demonstrates a simple console application that takes command-line arguments
// and uses a factory pattern to create different data obfuscation strategies based on user roles.
// It includes a UserData class to represent user information and a DataObfuscationStrategyFactory to create strategies.
class Program
{
    static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            List<string> decorators = new List<string>();
            string firstArgument = args[0];
            Console.WriteLine($"Base User: {firstArgument}");
            if(args.Length > 1)
            {
                for (int i = 1; i < args.Length; i++)
                {
                    decorators.Add(args[i]);
                }
                Console.WriteLine($"Decorators: {string.Join(", ", decorators)}");       
            
            }
            Start(firstArgument, decorators);
        }
    }

    static void Start(string baseUser, List<string> decorators)
    {

        var users = new List<UserData>
        {
            new() { Id = 1, FirstName = "Alice", LastName = "Johnson", Address = "123 Maple St", State = "California", Email = "alice.johnson@example.com", Country = "USA", Phone = "555-123-4567", Role = "SuperAdmin" },
            new() { Id = 2, FirstName = "Brian", LastName = "Smith", Address = "456 Oak Ave", State = "Texas", Email = "brian.smith@example.com", Country = "USA", Phone = "555-234-5678", Role = "Admin" },
            new() { Id = 3, FirstName = "Carla", LastName = "Martinez", Address = "789 Pine Ln", State = "Florida", Email = "carla.martinez@example.com", Country = "USA", Phone = "555-345-6789", Role = "User" },
            new() { Id = 4, FirstName = "David", LastName = "Nguyen", Address = "321 Birch Rd", State = "New York", Email = "david.nguyen@example.com", Country = "USA", Phone = "555-456-7890", Role = "User" },
            new() { Id = 5, FirstName = "Elena", LastName = "Petrov", Address = "654 Cedar Blvd", State = "Illinois", Email = "elena.petrov@example.com", Country = "USA", Phone = "555-567-8901", Role = "SuperAdmin" },
            new() { Id = 6, FirstName = "Frank", LastName = "Wright", Address = "987 Walnut St", State = "Ohio", Email = "frank.wright@example.com", Country = "USA", Phone = "555-678-9012", Role = "Admin" },
            new() { Id = 7, FirstName = "Grace", LastName = "Lee", Address = "159 Elm Dr", State = "Georgia", Email = "grace.lee@example.com", Country = "USA", Phone = "555-789-0123", Role = "User" },
            new() { Id = 8, FirstName = "Henry", LastName = "Clark", Address = "753 Spruce Ct", State = "North Carolina", Email = "henry.clark@example.com", Country = "USA", Phone = "555-890-1234", Role = "User" },
            new() { Id = 9, FirstName = "Isla", LastName = "Brown", Address = "852 Ash Ter", State = "Michigan", Email = "isla.brown@example.com", Country = "USA", Phone = "555-901-2345", Role = "SuperAdmin" },
            new() { Id = 10, FirstName = "Jack", LastName = "Davis", Address = "951 Poplar Way", State = "Pennsylvania", Email = "jack.davis@example.com", Country = "USA", Phone = "555-012-3456", Role = "Admin" },
            new() { Id = 11, FirstName = "Karen", LastName = "White", Address = "135 Chestnut Pl", State = "Indiana", Email = "karen.white@example.com", Country = "USA", Phone = "555-111-2222", Role = "User" },
            new() { Id = 12, FirstName = "Leo", LastName = "Kim", Address = "246 Redwood Pkwy", State = "Washington", Email = "leo.kim@example.com", Country = "USA", Phone = "555-222-3333", Role = "American" },
            new() { Id = 13, FirstName = "Maya", LastName = "Singh", Address = "357 Beech Cir", State = "Virginia", Email = "maya.singh@example.com", Country = "USA", Phone = "555-333-4444", Role = "Driver" },
            new() { Id = 14, FirstName = "Nathan", LastName = "Lopez", Address = "468 Dogwood Ave", State = "Tennessee", Email = "nathan.lopez@example.com", Country = "USA", Phone = "555-444-5555", Role = "DogTrainer" },
            new() { Id = 15, FirstName = "Olivia", LastName = "Baker", Address = "579 Hickory St", State = "Colorado", Email = "olivia.baker@example.com", Country = "USA", Phone = "555-555-6666", Role = "CatTrainer" },
            new() { Id = 16, FirstName = "Paul", LastName = "Reed", Address = "680 Sycamore Rd", State = "Wisconsin", Email = "paul.reed@example.com", Country = "USA", Phone = "555-666-7777", Role = "CatTrainer" },
            new() { Id = 17, FirstName = "Quinn", LastName = "Jenkins", Address = "781 Magnolia Blvd", State = "Minnesota", Email = "quinn.jenkins@example.com", Country = "USA", Phone = "555-777-8888", Role = "DogTrainer" },
            new() { Id = 18, FirstName = "Rachel", LastName = "Torres", Address = "892 Cypress Ln", State = "Oregon", Email = "rachel.torres@example.com", Country = "USA", Phone = "555-888-9999", Role = "User" },
            new() { Id = 19, FirstName = "Samuel", LastName = "Price", Address = "903 Maple Ave", State = "Arizona", Email = "samuel.price@example.com", Country = "USA", Phone = "555-999-0000", Role = "American" },
        };

        var strategies = new List<IDataObfuscation>();

        foreach (var user in users)
        {
            var strategy = DataObfuscationStrategyFactory.CreateObfuscationStrategy(baseUser, user);
            strategy = DataDecoratorsFactory.ApplyDecorators(strategy, decorators);
            strategies.Add(strategy);
        }
        foreach(var strat in strategies)
        {
            Console.WriteLine(strat.Obfuscate());
        }
    }
}

public class UserData
{
    public required int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Address { get; set; }
    public required string State { get; set; }
    public required string Email { get; set; }
    public required string Country { get; set; }
    public required string Phone { get; set; }
    public required string Role { get; set;}
}



