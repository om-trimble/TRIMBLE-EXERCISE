using SupportPortal.Api;

namespace SupportPortal.Tests;

public class SupportTicketTests
{
    [Fact]
    public void SupportTicket_DefaultConstructor_SetsDefaultValues()
    {
        // Act
        var ticket = new SupportTicket();

        // Assert
        Assert.Equal(0, ticket.Id);
        Assert.Equal("", ticket.Name);
        Assert.Equal("", ticket.Email);
        Assert.Equal("", ticket.Subject);
        Assert.Equal("", ticket.Description);
        Assert.Equal("Open", ticket.Status);
        Assert.True(ticket.CreatedDate <= DateTime.UtcNow);
        Assert.True(ticket.CreatedDate >= DateTime.UtcNow.AddSeconds(-1)); // Should be within last second
    }

    [Fact]
    public void SupportTicket_Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var ticket = new SupportTicket();
        var testId = 123;
        var testName = "John Doe";
        var testEmail = "john@example.com";
        var testSubject = "Test Subject";
        var testDescription = "Test Description";
        var testStatus = "In Progress";
        var testCreatedDate = DateTime.UtcNow.AddDays(-1);

        // Act
        ticket.Id = testId;
        ticket.Name = testName;
        ticket.Email = testEmail;
        ticket.Subject = testSubject;
        ticket.Description = testDescription;
        ticket.Status = testStatus;
        ticket.CreatedDate = testCreatedDate;

        // Assert
        Assert.Equal(testId, ticket.Id);
        Assert.Equal(testName, ticket.Name);
        Assert.Equal(testEmail, ticket.Email);
        Assert.Equal(testSubject, ticket.Subject);
        Assert.Equal(testDescription, ticket.Description);
        Assert.Equal(testStatus, ticket.Status);
        Assert.Equal(testCreatedDate, ticket.CreatedDate);
    }

    [Fact]
    public void SupportTicket_Properties_CanBeSetToNull()
    {
        // Arrange
        var ticket = new SupportTicket();

        // Act & Assert - These should not throw exceptions
        ticket.Name = null!;
        ticket.Email = null!;
        ticket.Subject = null!;
        ticket.Description = null!;
        ticket.Status = null!;

        Assert.Null(ticket.Name);
        Assert.Null(ticket.Email);
        Assert.Null(ticket.Subject);
        Assert.Null(ticket.Description);
        Assert.Null(ticket.Status);
    }

    [Fact]
    public void SupportTicket_Properties_CanBeSetToEmptyStrings()
    {
        // Arrange
        var ticket = new SupportTicket();

        // Act
        ticket.Name = "";
        ticket.Email = "";
        ticket.Subject = "";
        ticket.Description = "";
        ticket.Status = "";

        // Assert
        Assert.Equal("", ticket.Name);
        Assert.Equal("", ticket.Email);
        Assert.Equal("", ticket.Subject);
        Assert.Equal("", ticket.Description);
        Assert.Equal("", ticket.Status);
    }

    [Fact]
    public void SupportTicket_Properties_CanHandleLongStrings()
    {
        // Arrange
        var ticket = new SupportTicket();
        var longString = new string('A', 1000);

        // Act
        ticket.Name = longString;
        ticket.Email = longString;
        ticket.Subject = longString;
        ticket.Description = longString;
        ticket.Status = longString;

        // Assert
        Assert.Equal(longString, ticket.Name);
        Assert.Equal(longString, ticket.Email);
        Assert.Equal(longString, ticket.Subject);
        Assert.Equal(longString, ticket.Description);
        Assert.Equal(longString, ticket.Status);
    }

    [Fact]
    public void SupportTicket_CreatedDate_CanBeSetToSpecificDateTime()
    {
        // Arrange
        var ticket = new SupportTicket();
        var specificDate = new DateTime(2023, 12, 25, 10, 30, 45, DateTimeKind.Utc);

        // Act
        ticket.CreatedDate = specificDate;

        // Assert
        Assert.Equal(specificDate, ticket.CreatedDate);
    }

    [Fact]
    public void SupportTicket_Id_CanBeSetToNegativeValue()
    {
        // Arrange
        var ticket = new SupportTicket();

        // Act
        ticket.Id = -1;

        // Assert
        Assert.Equal(-1, ticket.Id);
    }

    [Fact]
    public void SupportTicket_Id_CanBeSetToMaxValue()
    {
        // Arrange
        var ticket = new SupportTicket();

        // Act
        ticket.Id = int.MaxValue;

        // Assert
        Assert.Equal(int.MaxValue, ticket.Id);
    }

    [Fact]
    public void SupportTicket_AllProperties_WorkIndependently()
    {
        // Arrange
        var ticket1 = new SupportTicket();
        var ticket2 = new SupportTicket();

        // Act
        ticket1.Id = 1;
        ticket1.Name = "User1";
        ticket2.Id = 2;
        ticket2.Name = "User2";

        // Assert - Changes to one ticket don't affect the other
        Assert.Equal(1, ticket1.Id);
        Assert.Equal("User1", ticket1.Name);
        Assert.Equal(2, ticket2.Id);
        Assert.Equal("User2", ticket2.Name);
    }
} 