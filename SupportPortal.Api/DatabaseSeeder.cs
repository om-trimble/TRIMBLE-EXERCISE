using System.Text.Json;
using SupportPortal.Api.Data;

namespace SupportPortal.Api;

public static class DatabaseSeeder
{
    public static async Task SeedDatabase(SupportTicketDbContext context)
    {
        // Check if database already has tickets
        if (context.Tickets.Any())
        {
            Console.WriteLine("Database already contains tickets. Skipping seeding.");
            return;
        }

        try
        {
            // Read the sample tickets JSON file
            var jsonFilePath = Path.Combine(AppContext.BaseDirectory, "SampleTickets.json");
            
            if (!File.Exists(jsonFilePath))
            {
                Console.WriteLine($"Sample tickets file not found at: {jsonFilePath}");
                return;
            }

            var jsonContent = await File.ReadAllTextAsync(jsonFilePath);
            var sampleTickets = JsonSerializer.Deserialize<List<SampleTicketData>>(jsonContent);

            if (sampleTickets == null || !sampleTickets.Any())
            {
                Console.WriteLine("No sample tickets found in JSON file.");
                return;
            }

            // Convert sample data to SupportTicket entities
            var tickets = sampleTickets.Select(sample => new SupportTicket
            {
                Name = sample.Name,
                Email = sample.Email,
                Subject = sample.Subject,
                Description = sample.Description,
                Status = sample.Status,
                CreatedDate = sample.CreatedDate
            }).ToList();

            // Add tickets to database
            await context.Tickets.AddRangeAsync(tickets);
            await context.SaveChangesAsync();

            Console.WriteLine($"Successfully seeded database with {tickets.Count} sample tickets.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding database: {ex.Message}");
        }
    }
}

// Helper class for JSON deserialization
public class SampleTicketData
{
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Subject { get; set; } = "";
    public string Description { get; set; } = "";
    public string Status { get; set; } = "Open";
    public DateTime CreatedDate { get; set; }
} 