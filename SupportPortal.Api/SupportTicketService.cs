/*
 * SupportTicketService.cs
 * 
 * This file has been commented out because the application has been migrated
 * to use Entity Framework Core with SQLite instead of in-memory list storage.
 * The functionality provided by this service is now handled directly by
 * the SupportTicketDbContext and SupportTicketsController.
 */

/*
namespace SupportPortal.Api;

public class SupportTicketService
{
    private readonly List<SupportTicket> _tickets = new();
    private int _nextId = 1;

    public SupportTicketService()
    {
        // Add some initial dummy tickets for testing
        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Alice Johnson",
            Email = "alice@example.com",
            Subject = "Cannot reset password",
            Description = "I'm having trouble resetting my password for the support portal. When I click the 'Forgot Password' link and enter my email address, I receive the reset email successfully. However, when I click the reset link in the email, it redirects me to a page that says 'Invalid or expired token'. I've tried this multiple times with different browsers (Chrome, Firefox, Safari) and the same issue persists. I've also checked my spam folder and cleared my browser cache, but nothing seems to work. Could you please help me reset my password or investigate why the reset links aren't working properly?",
            Status = "Open",
            CreatedDate = DateTime.UtcNow.AddDays(-1)
        });

        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Bob Smith",
            Email = "bob@example.com",
            Subject = "App crashes on launch",
            Description = "The support portal mobile app keeps crashing whenever I try to open it on my iPhone 14 Pro (iOS 17.2). The app starts loading with the splash screen, but then immediately closes and returns to the home screen. I've tried the following troubleshooting steps:\n\n1. Force-closing the app and reopening it\n2. Restarting my phone\n3. Uninstalling and reinstalling the app from the App Store\n4. Ensuring I have the latest version (v2.1.3)\n5. Checking that I have sufficient storage space (200GB free)\n\nThe crash happens consistently every time I try to open the app. I can access the support portal through Safari on my phone without any issues, so it seems to be specifically related to the mobile app. My device details: iPhone 14 Pro, iOS 17.2.1, Support Portal App v2.1.3.",
            Status = "In Progress",
            CreatedDate = DateTime.UtcNow.AddHours(-6)
        });

        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Charlie Brown",
            Email = "charlie@example.com",
            Subject = "Feature request: File attachments",
            Description = "I would like to request a new feature for the support portal that allows users to attach files when submitting support tickets. Currently, I can only provide text descriptions of my issues, but sometimes it would be much more helpful to include screenshots, error logs, or other supporting documents.\n\nSpecific use cases where this would be valuable:\n- Attaching screenshots of error messages or unexpected behavior\n- Including log files for technical issues\n- Sharing configuration files that might be causing problems\n- Providing examples of documents that aren't processing correctly\n\nSuggested implementation:\n- Support common file types: .png, .jpg, .pdf, .txt, .log, .zip\n- File size limit of 10MB per attachment\n- Maximum of 3 attachments per ticket\n- Ability to preview images before submitting\n- Virus scanning for uploaded files\n\nThis feature would significantly improve the quality of support requests and help the support team resolve issues more efficiently. Thank you for considering this enhancement!",
            Status = "Closed",
            CreatedDate = DateTime.UtcNow.AddHours(-12)
        });

        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Diana Prince",
            Email = "diana@example.com",
            Subject = "Performance issues with large datasets",
            Description = "I'm experiencing significant performance issues when working with large datasets in the support portal analytics dashboard. When I try to generate reports with more than 10,000 records, the system becomes extremely slow and sometimes times out completely.\n\nSpecific problems I'm encountering:\n- Report generation takes more than 5 minutes for datasets over 10K records\n- Browser becomes unresponsive during report processing\n- Frequent timeout errors with message 'Request timeout after 300 seconds'\n- Memory usage spikes to over 2GB in browser tab\n\nMy setup:\n- Browser: Chrome 119.0.6045.105 (latest)\n- Operating System: Windows 11 Pro\n- RAM: 16GB\n- Internet: 100Mbps fiber connection\n\nThe issue is particularly problematic when generating monthly reports that typically include 25,000+ support ticket records. Would it be possible to implement pagination, server-side processing, or data streaming to improve performance for large datasets?",
            Status = "Open",
            CreatedDate = DateTime.UtcNow.AddHours(-3)
        });
        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Alice Johnson",
            Email = "alice@example.com",
            Subject = "Cannot reset password",
            Description = "I'm having trouble resetting my password for the support portal. When I click the 'Forgot Password' link and enter my email address, I receive the reset email successfully. However, when I click the reset link in the email, it redirects me to a page that says 'Invalid or expired token'. I've tried this multiple times with different browsers (Chrome, Firefox, Safari) and the same issue persists. I've also checked my spam folder and cleared my browser cache, but nothing seems to work. Could you please help me reset my password or investigate why the reset links aren't working properly?",
            Status = "Open",
            CreatedDate = DateTime.UtcNow.AddDays(-1)
        });

        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Bob Smith",
            Email = "bob@example.com",
            Subject = "App crashes on launch",
            Description = "The support portal mobile app keeps crashing whenever I try to open it on my iPhone 14 Pro (iOS 17.2). The app starts loading with the splash screen, but then immediately closes and returns to the home screen. I've tried the following troubleshooting steps:\n\n1. Force-closing the app and reopening it\n2. Restarting my phone\n3. Uninstalling and reinstalling the app from the App Store\n4. Ensuring I have the latest version (v2.1.3)\n5. Checking that I have sufficient storage space (200GB free)\n\nThe crash happens consistently every time I try to open the app. I can access the support portal through Safari on my phone without any issues, so it seems to be specifically related to the mobile app. My device details: iPhone 14 Pro, iOS 17.2.1, Support Portal App v2.1.3.",
            Status = "In Progress",
            CreatedDate = DateTime.UtcNow.AddHours(-6)
        });

        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Charlie Brown",
            Email = "charlie@example.com",
            Subject = "Feature request: File attachments",
            Description = "I would like to request a new feature for the support portal that allows users to attach files when submitting support tickets. Currently, I can only provide text descriptions of my issues, but sometimes it would be much more helpful to include screenshots, error logs, or other supporting documents.\n\nSpecific use cases where this would be valuable:\n- Attaching screenshots of error messages or unexpected behavior\n- Including log files for technical issues\n- Sharing configuration files that might be causing problems\n- Providing examples of documents that aren't processing correctly\n\nSuggested implementation:\n- Support common file types: .png, .jpg, .pdf, .txt, .log, .zip\n- File size limit of 10MB per attachment\n- Maximum of 3 attachments per ticket\n- Ability to preview images before submitting\n- Virus scanning for uploaded files\n\nThis feature would significantly improve the quality of support requests and help the support team resolve issues more efficiently. Thank you for considering this enhancement!",
            Status = "Closed",
            CreatedDate = DateTime.UtcNow.AddHours(-12)
        });

        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Diana Prince",
            Email = "diana@example.com",
            Subject = "Performance issues with large datasets",
            Description = "I'm experiencing significant performance issues when working with large datasets in the support portal analytics dashboard. When I try to generate reports with more than 10,000 records, the system becomes extremely slow and sometimes times out completely.\n\nSpecific problems I'm encountering:\n- Report generation takes more than 5 minutes for datasets over 10K records\n- Browser becomes unresponsive during report processing\n- Frequent timeout errors with message 'Request timeout after 300 seconds'\n- Memory usage spikes to over 2GB in browser tab\n\nMy setup:\n- Browser: Chrome 119.0.6045.105 (latest)\n- Operating System: Windows 11 Pro\n- RAM: 16GB\n- Internet: 100Mbps fiber connection\n\nThe issue is particularly problematic when generating monthly reports that typically include 25,000+ support ticket records. Would it be possible to implement pagination, server-side processing, or data streaming to improve performance for large datasets?",
            Status = "Open",
            CreatedDate = DateTime.UtcNow.AddHours(-3)
        });
        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Alice Johnson",
            Email = "alice@example.com",
            Subject = "Cannot reset password",
            Description = "I'm having trouble resetting my password for the support portal. When I click the 'Forgot Password' link and enter my email address, I receive the reset email successfully. However, when I click the reset link in the email, it redirects me to a page that says 'Invalid or expired token'. I've tried this multiple times with different browsers (Chrome, Firefox, Safari) and the same issue persists. I've also checked my spam folder and cleared my browser cache, but nothing seems to work. Could you please help me reset my password or investigate why the reset links aren't working properly?",
            Status = "Open",
            CreatedDate = DateTime.UtcNow.AddDays(-1)
        });

        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Bob Smith",
            Email = "bob@example.com",
            Subject = "App crashes on launch",
            Description = "The support portal mobile app keeps crashing whenever I try to open it on my iPhone 14 Pro (iOS 17.2). The app starts loading with the splash screen, but then immediately closes and returns to the home screen. I've tried the following troubleshooting steps:\n\n1. Force-closing the app and reopening it\n2. Restarting my phone\n3. Uninstalling and reinstalling the app from the App Store\n4. Ensuring I have the latest version (v2.1.3)\n5. Checking that I have sufficient storage space (200GB free)\n\nThe crash happens consistently every time I try to open the app. I can access the support portal through Safari on my phone without any issues, so it seems to be specifically related to the mobile app. My device details: iPhone 14 Pro, iOS 17.2.1, Support Portal App v2.1.3.",
            Status = "In Progress",
            CreatedDate = DateTime.UtcNow.AddHours(-6)
        });

        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Charlie Brown",
            Email = "charlie@example.com",
            Subject = "Feature request: File attachments",
            Description = "I would like to request a new feature for the support portal that allows users to attach files when submitting support tickets. Currently, I can only provide text descriptions of my issues, but sometimes it would be much more helpful to include screenshots, error logs, or other supporting documents.\n\nSpecific use cases where this would be valuable:\n- Attaching screenshots of error messages or unexpected behavior\n- Including log files for technical issues\n- Sharing configuration files that might be causing problems\n- Providing examples of documents that aren't processing correctly\n\nSuggested implementation:\n- Support common file types: .png, .jpg, .pdf, .txt, .log, .zip\n- File size limit of 10MB per attachment\n- Maximum of 3 attachments per ticket\n- Ability to preview images before submitting\n- Virus scanning for uploaded files\n\nThis feature would significantly improve the quality of support requests and help the support team resolve issues more efficiently. Thank you for considering this enhancement!",
            Status = "Closed",
            CreatedDate = DateTime.UtcNow.AddHours(-12)
        });

        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Diana Prince",
            Email = "diana@example.com",
            Subject = "Performance issues with large datasets",
            Description = "I'm experiencing significant performance issues when working with large datasets in the support portal analytics dashboard. When I try to generate reports with more than 10,000 records, the system becomes extremely slow and sometimes times out completely.\n\nSpecific problems I'm encountering:\n- Report generation takes more than 5 minutes for datasets over 10K records\n- Browser becomes unresponsive during report processing\n- Frequent timeout errors with message 'Request timeout after 300 seconds'\n- Memory usage spikes to over 2GB in browser tab\n\nMy setup:\n- Browser: Chrome 119.0.6045.105 (latest)\n- Operating System: Windows 11 Pro\n- RAM: 16GB\n- Internet: 100Mbps fiber connection\n\nThe issue is particularly problematic when generating monthly reports that typically include 25,000+ support ticket records. Would it be possible to implement pagination, server-side processing, or data streaming to improve performance for large datasets?",
            Status = "Open",
            CreatedDate = DateTime.UtcNow.AddHours(-3)
        });
        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Alice Johnson",
            Email = "alice@example.com",
            Subject = "Cannot reset password",
            Description = "I'm having trouble resetting my password for the support portal. When I click the 'Forgot Password' link and enter my email address, I receive the reset email successfully. However, when I click the reset link in the email, it redirects me to a page that says 'Invalid or expired token'. I've tried this multiple times with different browsers (Chrome, Firefox, Safari) and the same issue persists. I've also checked my spam folder and cleared my browser cache, but nothing seems to work. Could you please help me reset my password or investigate why the reset links aren't working properly?",
            Status = "Open",
            CreatedDate = DateTime.UtcNow.AddDays(-1)
        });

        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Bob Smith",
            Email = "bob@example.com",
            Subject = "App crashes on launch",
            Description = "The support portal mobile app keeps crashing whenever I try to open it on my iPhone 14 Pro (iOS 17.2). The app starts loading with the splash screen, but then immediately closes and returns to the home screen. I've tried the following troubleshooting steps:\n\n1. Force-closing the app and reopening it\n2. Restarting my phone\n3. Uninstalling and reinstalling the app from the App Store\n4. Ensuring I have the latest version (v2.1.3)\n5. Checking that I have sufficient storage space (200GB free)\n\nThe crash happens consistently every time I try to open the app. I can access the support portal through Safari on my phone without any issues, so it seems to be specifically related to the mobile app. My device details: iPhone 14 Pro, iOS 17.2.1, Support Portal App v2.1.3.",
            Status = "In Progress",
            CreatedDate = DateTime.UtcNow.AddHours(-6)
        });

        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Charlie Brown",
            Email = "charlie@example.com",
            Subject = "Feature request: File attachments",
            Description = "I would like to request a new feature for the support portal that allows users to attach files when submitting support tickets. Currently, I can only provide text descriptions of my issues, but sometimes it would be much more helpful to include screenshots, error logs, or other supporting documents.\n\nSpecific use cases where this would be valuable:\n- Attaching screenshots of error messages or unexpected behavior\n- Including log files for technical issues\n- Sharing configuration files that might be causing problems\n- Providing examples of documents that aren't processing correctly\n\nSuggested implementation:\n- Support common file types: .png, .jpg, .pdf, .txt, .log, .zip\n- File size limit of 10MB per attachment\n- Maximum of 3 attachments per ticket\n- Ability to preview images before submitting\n- Virus scanning for uploaded files\n\nThis feature would significantly improve the quality of support requests and help the support team resolve issues more efficiently. Thank you for considering this enhancement!",
            Status = "Closed",
            CreatedDate = DateTime.UtcNow.AddHours(-12)
        });

        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Diana Prince",
            Email = "diana@example.com",
            Subject = "Performance issues with large datasets",
            Description = "I'm experiencing significant performance issues when working with large datasets in the support portal analytics dashboard. When I try to generate reports with more than 10,000 records, the system becomes extremely slow and sometimes times out completely.\n\nSpecific problems I'm encountering:\n- Report generation takes more than 5 minutes for datasets over 10K records\n- Browser becomes unresponsive during report processing\n- Frequent timeout errors with message 'Request timeout after 300 seconds'\n- Memory usage spikes to over 2GB in browser tab\n\nMy setup:\n- Browser: Chrome 119.0.6045.105 (latest)\n- Operating System: Windows 11 Pro\n- RAM: 16GB\n- Internet: 100Mbps fiber connection\n\nThe issue is particularly problematic when generating monthly reports that typically include 25,000+ support ticket records. Would it be possible to implement pagination, server-side processing, or data streaming to improve performance for large datasets?",
            Status = "Open",
            CreatedDate = DateTime.UtcNow.AddHours(-3)
        });
        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Alice Johnson",
            Email = "alice@example.com",
            Subject = "Cannot reset password",
            Description = "I'm having trouble resetting my password for the support portal. When I click the 'Forgot Password' link and enter my email address, I receive the reset email successfully. However, when I click the reset link in the email, it redirects me to a page that says 'Invalid or expired token'. I've tried this multiple times with different browsers (Chrome, Firefox, Safari) and the same issue persists. I've also checked my spam folder and cleared my browser cache, but nothing seems to work. Could you please help me reset my password or investigate why the reset links aren't working properly?",
            Status = "Open",
            CreatedDate = DateTime.UtcNow.AddDays(-1)
        });

        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Bob Smith",
            Email = "bob@example.com",
            Subject = "App crashes on launch",
            Description = "The support portal mobile app keeps crashing whenever I try to open it on my iPhone 14 Pro (iOS 17.2). The app starts loading with the splash screen, but then immediately closes and returns to the home screen. I've tried the following troubleshooting steps:\n\n1. Force-closing the app and reopening it\n2. Restarting my phone\n3. Uninstalling and reinstalling the app from the App Store\n4. Ensuring I have the latest version (v2.1.3)\n5. Checking that I have sufficient storage space (200GB free)\n\nThe crash happens consistently every time I try to open the app. I can access the support portal through Safari on my phone without any issues, so it seems to be specifically related to the mobile app. My device details: iPhone 14 Pro, iOS 17.2.1, Support Portal App v2.1.3.",
            Status = "In Progress",
            CreatedDate = DateTime.UtcNow.AddHours(-6)
        });

        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Charlie Brown",
            Email = "charlie@example.com",
            Subject = "Feature request: File attachments",
            Description = "I would like to request a new feature for the support portal that allows users to attach files when submitting support tickets. Currently, I can only provide text descriptions of my issues, but sometimes it would be much more helpful to include screenshots, error logs, or other supporting documents.\n\nSpecific use cases where this would be valuable:\n- Attaching screenshots of error messages or unexpected behavior\n- Including log files for technical issues\n- Sharing configuration files that might be causing problems\n- Providing examples of documents that aren't processing correctly\n\nSuggested implementation:\n- Support common file types: .png, .jpg, .pdf, .txt, .log, .zip\n- File size limit of 10MB per attachment\n- Maximum of 3 attachments per ticket\n- Ability to preview images before submitting\n- Virus scanning for uploaded files\n\nThis feature would significantly improve the quality of support requests and help the support team resolve issues more efficiently. Thank you for considering this enhancement!",
            Status = "Closed",
            CreatedDate = DateTime.UtcNow.AddHours(-12)
        });

        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Diana Prince",
            Email = "diana@example.com",
            Subject = "Performance issues with large datasets",
            Description = "I'm experiencing significant performance issues when working with large datasets in the support portal analytics dashboard. When I try to generate reports with more than 10,000 records, the system becomes extremely slow and sometimes times out completely.\n\nSpecific problems I'm encountering:\n- Report generation takes more than 5 minutes for datasets over 10K records\n- Browser becomes unresponsive during report processing\n- Frequent timeout errors with message 'Request timeout after 300 seconds'\n- Memory usage spikes to over 2GB in browser tab\n\nMy setup:\n- Browser: Chrome 119.0.6045.105 (latest)\n- Operating System: Windows 11 Pro\n- RAM: 16GB\n- Internet: 100Mbps fiber connection\n\nThe issue is particularly problematic when generating monthly reports that typically include 25,000+ support ticket records. Would it be possible to implement pagination, server-side processing, or data streaming to improve performance for large datasets?",
            Status = "Open",
            CreatedDate = DateTime.UtcNow.AddHours(-3)
        });
        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Alice Johnson",
            Email = "alice@example.com",
            Subject = "Cannot reset password",
            Description = "I'm having trouble resetting my password for the support portal. When I click the 'Forgot Password' link and enter my email address, I receive the reset email successfully. However, when I click the reset link in the email, it redirects me to a page that says 'Invalid or expired token'. I've tried this multiple times with different browsers (Chrome, Firefox, Safari) and the same issue persists. I've also checked my spam folder and cleared my browser cache, but nothing seems to work. Could you please help me reset my password or investigate why the reset links aren't working properly?",
            Status = "Open",
            CreatedDate = DateTime.UtcNow.AddDays(-1)
        });

        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Bob Smith",
            Email = "bob@example.com",
            Subject = "App crashes on launch",
            Description = "The support portal mobile app keeps crashing whenever I try to open it on my iPhone 14 Pro (iOS 17.2). The app starts loading with the splash screen, but then immediately closes and returns to the home screen. I've tried the following troubleshooting steps:\n\n1. Force-closing the app and reopening it\n2. Restarting my phone\n3. Uninstalling and reinstalling the app from the App Store\n4. Ensuring I have the latest version (v2.1.3)\n5. Checking that I have sufficient storage space (200GB free)\n\nThe crash happens consistently every time I try to open the app. I can access the support portal through Safari on my phone without any issues, so it seems to be specifically related to the mobile app. My device details: iPhone 14 Pro, iOS 17.2.1, Support Portal App v2.1.3.",
            Status = "In Progress",
            CreatedDate = DateTime.UtcNow.AddHours(-6)
        });

        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Charlie Brown",
            Email = "charlie@example.com",
            Subject = "Feature request: File attachments",
            Description = "I would like to request a new feature for the support portal that allows users to attach files when submitting support tickets. Currently, I can only provide text descriptions of my issues, but sometimes it would be much more helpful to include screenshots, error logs, or other supporting documents.\n\nSpecific use cases where this would be valuable:\n- Attaching screenshots of error messages or unexpected behavior\n- Including log files for technical issues\n- Sharing configuration files that might be causing problems\n- Providing examples of documents that aren't processing correctly\n\nSuggested implementation:\n- Support common file types: .png, .jpg, .pdf, .txt, .log, .zip\n- File size limit of 10MB per attachment\n- Maximum of 3 attachments per ticket\n- Ability to preview images before submitting\n- Virus scanning for uploaded files\n\nThis feature would significantly improve the quality of support requests and help the support team resolve issues more efficiently. Thank you for considering this enhancement!",
            Status = "Closed",
            CreatedDate = DateTime.UtcNow.AddHours(-12)
        });

        _tickets.Add(new SupportTicket
        {
            Id = _nextId++,
            Name = "Diana Prince",
            Email = "diana@example.com",
            Subject = "Performance issues with large datasets",
            Description = "I'm experiencing significant performance issues when working with large datasets in the support portal analytics dashboard. When I try to generate reports with more than 10,000 records, the system becomes extremely slow and sometimes times out completely.\n\nSpecific problems I'm encountering:\n- Report generation takes more than 5 minutes for datasets over 10K records\n- Browser becomes unresponsive during report processing\n- Frequent timeout errors with message 'Request timeout after 300 seconds'\n- Memory usage spikes to over 2GB in browser tab\n\nMy setup:\n- Browser: Chrome 119.0.6045.105 (latest)\n- Operating System: Windows 11 Pro\n- RAM: 16GB\n- Internet: 100Mbps fiber connection\n\nThe issue is particularly problematic when generating monthly reports that typically include 25,000+ support ticket records. Would it be possible to implement pagination, server-side processing, or data streaming to improve performance for large datasets?",
            Status = "Open",
            CreatedDate = DateTime.UtcNow.AddHours(-3)
        });
    }

    public List<SupportTicket> GetAllTickets()
    {
        return _tickets;
    }

    public SupportTicket? GetTicketById(int id)
    {
        return _tickets.FirstOrDefault(t => t.Id == id);
    }

    public void AddTicket(SupportTicket ticket)
    {
        ticket.Id = _nextId++;
        ticket.CreatedDate = DateTime.UtcNow;
        _tickets.Add(ticket);
    }

    public bool CloseTicket(int id)
    {
        var ticket = _tickets.FirstOrDefault(t => t.Id == id);
        if (ticket == null)
        {
            return false;
        }

        ticket.Status = "Closed";
        return true;
    }
}
*/
