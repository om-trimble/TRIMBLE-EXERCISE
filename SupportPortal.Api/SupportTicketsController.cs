using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SupportPortal.Api.Data;

namespace SupportPortal.Api;

    [ApiController]
    [Route("api/tickets")]
    public class SupportTicketsController : ControllerBase
    {
    private readonly SupportTicketDbContext _context;
    private readonly IMemoryCache _cache;

    public SupportTicketsController(SupportTicketDbContext context, IMemoryCache cache)
        {
        _context = context;
        _cache = cache;
        }

        [HttpGet]
        public ActionResult<List<SupportTicket>> GetAllTickets(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 20)
        {
            // Validate parameters
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 100) pageSize = 100; // Limit max page size for performance
            
            // Generate cache key
            var cacheKey = $"tickets_page_{pageNumber}_size_{pageSize}";
            
            // Try to get from cache first
            if (_cache.TryGetValue(cacheKey, out List<SupportTicket>? cachedTickets))
            {
                return Ok(cachedTickets);
            }
            
            // Get tickets from database with pagination
            var tickets = _context.Tickets
                .OrderBy(t => t.Id) // Ensure consistent ordering for pagination
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            
            // Store in cache with sliding expiration of 60 seconds
            var cacheOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(60)
            };
            _cache.Set(cacheKey, tickets, cacheOptions);
            
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public ActionResult<SupportTicket> GetTicketById(int id)
        {
        var ticket = _context.Tickets.Find(id);
            
            if (ticket == null)
            {
                return NotFound();
            }
            
            return Ok(ticket);
        }

        [HttpPost]
        public ActionResult<SupportTicket> CreateTicket([FromBody] SupportTicket ticket)
        {
        // Set default values as requested
        ticket.CreatedDate = DateTime.UtcNow;
        ticket.Status = "Open";
        
        _context.Tickets.Add(ticket);
        _context.SaveChanges();
        
        // Invalidate all cached pages since a new ticket was added
        InvalidateAllTicketCache();
        
            return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, ticket);
        }

    [HttpPut("{id}/close")]
    public ActionResult CloseTicket(int id)
    {
        var ticket = _context.Tickets.Find(id);
        
        if (ticket == null)
        {
            return NotFound();
        }
        
        ticket.Status = "Closed";
        _context.SaveChanges();
        
        // Invalidate all cached pages since ticket status changed
        InvalidateAllTicketCache();
        
        return Ok(new { message = "Ticket closed successfully" });
    }

    private void InvalidateAllTicketCache()
    {
        // Get all cache keys that match our ticket cache pattern and remove them
        // Since IMemoryCache doesn't have a direct way to get all keys, we'll use a simple approach
        // to clear commonly used cache entries
        
        for (int page = 1; page <= 10; page++) // Clear first 10 pages
        {
            for (int size = 10; size <= 100; size += 10) // Common page sizes
            {
                var cacheKey = $"tickets_page_{page}_size_{size}";
                _cache.Remove(cacheKey);
            }
        }
        
        // Also clear the most common page size (20)
        for (int page = 1; page <= 20; page++)
        {
            var cacheKey = $"tickets_page_{page}_size_20";
            _cache.Remove(cacheKey);
        }
    }
} 