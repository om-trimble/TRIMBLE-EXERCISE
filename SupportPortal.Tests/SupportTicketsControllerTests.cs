using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SupportPortal.Api;
using SupportPortal.Api.Data;

namespace SupportPortal.Tests;

public class SupportTicketsControllerTests : IDisposable
{
    private readonly SupportTicketDbContext _context;
    private readonly IMemoryCache _cache;
    private readonly SupportTicketsController _controller;

    public SupportTicketsControllerTests()
    {
        var options = new DbContextOptionsBuilder<SupportTicketDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new SupportTicketDbContext(options);
        _cache = new MemoryCache(new MemoryCacheOptions());
        _controller = new SupportTicketsController(_context, _cache);
    }

    public void Dispose()
    {
        _context.Dispose();
        _cache.Dispose();
    }

    [Fact]
    public void GetAllTickets_WhenNoTickets_ReturnsEmptyList()
    {
        // Act
        var result = _controller.GetAllTickets();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var tickets = Assert.IsType<List<SupportTicket>>(okResult.Value);
        Assert.Empty(tickets);
    }

    [Fact]
    public void GetAllTickets_WhenTicketsExist_ReturnsAllTickets()
    {
        // Arrange
        var ticket1 = new SupportTicket
        {
            Name = "John Doe",
            Email = "john@example.com",
            Subject = "Test Subject 1",
            Description = "Test Description 1"
        };
        var ticket2 = new SupportTicket
        {
            Name = "Jane Smith",
            Email = "jane@example.com",
            Subject = "Test Subject 2",
            Description = "Test Description 2"
        };

        _context.Tickets.AddRange(ticket1, ticket2);
        _context.SaveChanges();

        // Act
        var result = _controller.GetAllTickets();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var tickets = Assert.IsType<List<SupportTicket>>(okResult.Value);
        Assert.Equal(2, tickets.Count);
    }

    [Fact]
    public void GetTicketById_WhenTicketExists_ReturnsTicket()
    {
        // Arrange
        var ticket = new SupportTicket
        {
            Name = "John Doe",
            Email = "john@example.com",
            Subject = "Test Subject",
            Description = "Test Description"
        };

        _context.Tickets.Add(ticket);
        _context.SaveChanges();

        // Act
        var result = _controller.GetTicketById(ticket.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTicket = Assert.IsType<SupportTicket>(okResult.Value);
        Assert.Equal(ticket.Id, returnedTicket.Id);
        Assert.Equal(ticket.Name, returnedTicket.Name);
        Assert.Equal(ticket.Email, returnedTicket.Email);
    }

    [Fact]
    public void GetTicketById_WhenTicketDoesNotExist_ReturnsNotFound()
    {
        // Act
        var result = _controller.GetTicketById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void CreateTicket_WithValidData_CreatesTicketWithDefaults()
    {
        // Arrange
        var ticket = new SupportTicket
        {
            Name = "John Doe",
            Email = "john@example.com",
            Subject = "Test Subject",
            Description = "Test Description"
        };

        // Act
        var result = _controller.CreateTicket(ticket);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdTicket = Assert.IsType<SupportTicket>(createdResult.Value);
        
        Assert.Equal("Open", createdTicket.Status);
        Assert.True(createdTicket.CreatedDate > DateTime.UtcNow.AddMinutes(-1));
        Assert.True(createdTicket.CreatedDate <= DateTime.UtcNow);
        Assert.Equal("John Doe", createdTicket.Name);
        Assert.Equal("john@example.com", createdTicket.Email);
        Assert.Equal("Test Subject", createdTicket.Subject);
        Assert.Equal("Test Description", createdTicket.Description);

        // Verify it was saved to database
        var savedTicket = _context.Tickets.Find(createdTicket.Id);
        Assert.NotNull(savedTicket);
        Assert.Equal(createdTicket.Name, savedTicket.Name);
    }

    [Fact]
    public void CreateTicket_WithMinimalData_CreatesTicketSuccessfully()
    {
        // Arrange
        var ticket = new SupportTicket
        {
            Name = "A",
            Email = "a@b.co",
            Subject = "S",
            Description = "D"
        };

        // Act
        var result = _controller.CreateTicket(ticket);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdTicket = Assert.IsType<SupportTicket>(createdResult.Value);
        
        Assert.Equal("Open", createdTicket.Status);
        Assert.Equal("A", createdTicket.Name);
        Assert.Equal("a@b.co", createdTicket.Email);
    }

    [Fact]
    public void CreateTicket_OverridesStatusAndCreatedDate()
    {
        // Arrange
        var ticket = new SupportTicket
        {
            Name = "John Doe",
            Email = "john@example.com",
            Subject = "Test Subject",
            Description = "Test Description",
            Status = "Closed", // This should be overridden
            CreatedDate = DateTime.UtcNow.AddDays(-1) // This should be overridden
        };

        // Act
        var result = _controller.CreateTicket(ticket);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdTicket = Assert.IsType<SupportTicket>(createdResult.Value);
        
        Assert.Equal("Open", createdTicket.Status); // Should be overridden to "Open"
        Assert.True(createdTicket.CreatedDate > DateTime.UtcNow.AddMinutes(-1)); // Should be overridden to now
    }

    [Fact]
    public void CloseTicket_WhenTicketExists_ClosesTicketSuccessfully()
    {
        // Arrange
        var ticket = new SupportTicket
        {
            Name = "John Doe",
            Email = "john@example.com",
            Subject = "Test Subject",
            Description = "Test Description",
            Status = "Open"
        };

        _context.Tickets.Add(ticket);
        _context.SaveChanges();

        // Act
        var result = _controller.CloseTicket(ticket.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        
        // Verify the ticket status was updated in the database
        var updatedTicket = _context.Tickets.Find(ticket.Id);
        Assert.NotNull(updatedTicket);
        Assert.Equal("Closed", updatedTicket.Status);
    }

    [Fact]
    public void CloseTicket_WhenTicketDoesNotExist_ReturnsNotFound()
    {
        // Act
        var result = _controller.CloseTicket(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void CloseTicket_WhenTicketAlreadyClosed_CanCloseAgain()
    {
        // Arrange
        var ticket = new SupportTicket
        {
            Name = "John Doe",
            Email = "john@example.com",
            Subject = "Test Subject",
            Description = "Test Description",
            Status = "Closed"
        };

        _context.Tickets.Add(ticket);
        _context.SaveChanges();

        // Act
        var result = _controller.CloseTicket(ticket.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        
        // Verify the ticket status remains closed
        var updatedTicket = _context.Tickets.Find(ticket.Id);
        Assert.NotNull(updatedTicket);
        Assert.Equal("Closed", updatedTicket.Status);
    }

    [Fact]
    public void CreateTicket_ReturnsCorrectActionName()
    {
        // Arrange
        var ticket = new SupportTicket
        {
            Name = "John Doe",
            Email = "john@example.com",
            Subject = "Test Subject",
            Description = "Test Description"
        };

        // Act
        var result = _controller.CreateTicket(ticket);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(SupportTicketsController.GetTicketById), createdResult.ActionName);
    }

    [Fact]
    public void Controller_PersistsDataAcrossOperations()
    {
        // Arrange & Act - Create a ticket
        var ticket = new SupportTicket
        {
            Name = "John Doe",
            Email = "john@example.com",
            Subject = "Test Subject",
            Description = "Test Description"
        };

        var createResult = _controller.CreateTicket(ticket);
        var createdTicket = Assert.IsType<SupportTicket>(((CreatedAtActionResult)createResult.Result!).Value);

        // Act - Retrieve the ticket
        var getResult = _controller.GetTicketById(createdTicket.Id);
        var retrievedTicket = Assert.IsType<SupportTicket>(((OkObjectResult)getResult.Result!).Value);

        // Assert - Data should match
        Assert.Equal(createdTicket.Id, retrievedTicket.Id);
        Assert.Equal(createdTicket.Name, retrievedTicket.Name);
        Assert.Equal(createdTicket.Email, retrievedTicket.Email);
        Assert.Equal(createdTicket.Status, retrievedTicket.Status);
    }

    [Fact]
    public void CloseTicket_ReturnsCorrectMessage()
    {
        // Arrange
        var ticket = new SupportTicket
        {
            Name = "John Doe",
            Email = "john@example.com",
            Subject = "Test Subject",
            Description = "Test Description"
        };

        _context.Tickets.Add(ticket);
        _context.SaveChanges();

        // Act
        var result = _controller.CloseTicket(ticket.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value;
        
        // Check that the response contains the expected message structure
        Assert.NotNull(response);
        var messageProperty = response.GetType().GetProperty("message");
        Assert.NotNull(messageProperty);
        Assert.Equal("Ticket closed successfully", messageProperty.GetValue(response));
    }
} 