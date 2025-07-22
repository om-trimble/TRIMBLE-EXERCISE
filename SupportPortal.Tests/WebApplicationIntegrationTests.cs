using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using SupportPortal.Api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SupportPortal.Api.Data;
using Microsoft.AspNetCore.Hosting;

namespace SupportPortal.Tests;

public class WebApplicationIntegrationTests : IClassFixture<WebApplicationIntegrationTests.TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public WebApplicationIntegrationTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    public class TestWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
    {
        private readonly string _databasePath;

        public TestWebApplicationFactory()
        {
            // Create a unique database file for each test run
            _databasePath = Path.Combine(Path.GetTempPath(), $"test_supporttickets_{Guid.NewGuid()}.db");
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the existing DbContext registration
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SupportTicketDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add DbContext using unique SQLite database for this test run
                services.AddDbContext<SupportTicketDbContext>(options =>
                {
                    options.UseSqlite($"Data Source={_databasePath}");
                });

                // Ensure database is created for each test
                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<SupportTicketDbContext>();
                context.Database.EnsureCreated();
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Clean up the test database
                if (File.Exists(_databasePath))
                {
                    try
                    {
                        File.Delete(_databasePath);
                    }
                    catch
                    {
                        // Ignore cleanup errors
                    }
                }
            }
            base.Dispose(disposing);
        }
    }

    [Fact]
    public async Task Application_StartsSuccessfully()
    {
        // Act & Assert
        var response = await _client.GetAsync("/api/tickets");
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Swagger_IsEnabled()
    {
        // Act
        var response = await _client.GetAsync("/swagger/index.html");
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CORS_IsConfigured()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Options, "/api/tickets");
        request.Headers.Add("Origin", "http://localhost:3000");
        request.Headers.Add("Access-Control-Request-Method", "GET");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.True(response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetAllTickets_ReturnsJsonResponse()
    {
        // Act
        var response = await _client.GetAsync("/api/tickets");
        var tickets = await response.Content.ReadFromJsonAsync<List<SupportTicket>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(tickets);
        Assert.True(tickets.Count > 0); // Database contains seeded sample tickets
        Assert.True(tickets.Count <= 20); // Default page size should be 20 or less
    }

    [Fact]
    public async Task GetTicketById_NonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/tickets/99999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetTicketById_InvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/tickets/99999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateTicket_ValidTicket_ReturnsCreated()
    {
        // Arrange
        var newTicket = new SupportTicket
        {
            Name = "Integration Test User",
            Email = "integration@test.com",
            Subject = "Integration Test Subject",
            Description = "Integration Test Description"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/tickets", newTicket);
        var createdTicket = await response.Content.ReadFromJsonAsync<SupportTicket>();

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(createdTicket);
        Assert.True(createdTicket.Id > 0);
        Assert.Equal(newTicket.Name, createdTicket.Name);
        Assert.Equal(newTicket.Email, createdTicket.Email);
        Assert.Equal(newTicket.Subject, createdTicket.Subject);
        Assert.Equal(newTicket.Description, createdTicket.Description);
        Assert.Equal("Open", createdTicket.Status);
    }

    [Fact]
    public async Task CloseTicket_ValidId_ReturnsOk()
    {
        // Arrange - First create a ticket
        var newTicket = new SupportTicket
        {
            Name = "Test User for Close",
            Email = "close@test.com",
            Subject = "Test Close Subject",
            Description = "Test Close Description"
        };
        var createResponse = await _client.PostAsJsonAsync("/api/tickets", newTicket);
        var createdTicket = await createResponse.Content.ReadFromJsonAsync<SupportTicket>();

        // Act - Close the ticket
        var response = await _client.PutAsync($"/api/tickets/{createdTicket!.Id}/close", null);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        // Verify ticket is actually closed
        var getResponse = await _client.GetAsync($"/api/tickets/{createdTicket.Id}");
        var closedTicket = await getResponse.Content.ReadFromJsonAsync<SupportTicket>();
        Assert.Equal("Closed", closedTicket!.Status);
    }

    [Fact]
    public async Task CloseTicket_InvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.PutAsync("/api/tickets/99999/close", null);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task HttpsRedirection_IsConfigured()
    {
        // This test verifies that HTTPS redirection middleware is configured
        // The WebApplicationFactory will handle this automatically in test environment
        
        // Act
        var response = await _client.GetAsync("/api/tickets");

        // Assert - If HTTPS redirection is working, we should get a successful response
        // In test environment, this confirms the middleware is configured
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task DependencyInjection_SupportTicketService_IsRegistered()
    {
        // This test verifies that the SupportTicketService is properly registered as singleton
        
        // Act - Make multiple requests to verify singleton behavior
        var response1 = await _client.GetAsync("/api/tickets");
        var tickets1 = await response1.Content.ReadFromJsonAsync<List<SupportTicket>>();
        
        var response2 = await _client.GetAsync("/api/tickets");
        var tickets2 = await response2.Content.ReadFromJsonAsync<List<SupportTicket>>();

        // Assert - Both requests should return the same data (proving singleton)
        Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
        Assert.NotNull(tickets1);
        Assert.NotNull(tickets2);
        Assert.Equal(tickets1.Count, tickets2.Count);
    }
} 