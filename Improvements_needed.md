Can we do caching for get request with id?

Improve the cache logic... it should not invalidate full cache memory... it should only invalidate affected cache memory 

Replace In-memory cache to distributed caching (Redis or somthing)

InvalidateAllTicketCache() is a manual and approximate cleanup — won’t catch every combination unless loops are large - improve its logic

