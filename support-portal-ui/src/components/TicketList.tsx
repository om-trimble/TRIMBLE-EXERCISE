import React, { useState, useEffect } from 'react';
import { Ticket } from '../types/Ticket';

interface TicketListProps {
  refreshTrigger: number;
}

const TicketList: React.FC<TicketListProps> = ({ refreshTrigger }) => {
  const [tickets, setTickets] = useState<Ticket[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  
  // New state for ticket search
  const [searchId, setSearchId] = useState('');
  const [searchedTicket, setSearchedTicket] = useState<Ticket | null>(null);
  const [searchLoading, setSearchLoading] = useState(false);
  const [searchError, setSearchError] = useState<string | null>(null);

  // New state for expandable tickets
  const [expandedTicketId, setExpandedTicketId] = useState<number | null>(null);

  // Pagination state
  const [pageNumber, setPageNumber] = useState(1);
  const pageSize = 20;

  const fetchTickets = async () => {
    try {
      setLoading(true);
      setError(null);
      
      const response = await fetch(`http://localhost:5000/api/tickets?pageNumber=${pageNumber}&pageSize=${pageSize}`);
      
      if (!response.ok) {
        throw new Error('Failed to fetch tickets');
      }
      
      const data = await response.json();
      setTickets(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'An error occurred');
      console.error('Error fetching tickets:', err);
    } finally {
      setLoading(false);
    }
  };

  const searchTicketById = async () => {
    if (!searchId.trim()) return;
    
    try {
      setSearchLoading(true);
      setSearchError(null);
      setSearchedTicket(null);
      
      const response = await fetch(`http://localhost:5000/api/tickets/${searchId}`);
      
      if (response.status === 404) {
        setSearchError(`Ticket with ID ${searchId} not found`);
        return;
      }
      
      if (!response.ok) {
        throw new Error('Failed to fetch ticket');
      }
      
      const data = await response.json();
      setSearchedTicket(data);
    } catch (err) {
      setSearchError(err instanceof Error ? err.message : 'An error occurred');
      console.error('Error searching ticket:', err);
    } finally {
      setSearchLoading(false);
    }
  };

  const withdrawTicket = async (ticketId: number) => {
    try {
      const response = await fetch(`http://localhost:5000/api/tickets/${ticketId}/close`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        }
      });

      if (!response.ok) {
        throw new Error('Failed to close ticket');
      }

      // Refresh the tickets list
      await fetchTickets();
      
      // Also update the searched ticket if it was the one being withdrawn
      if (searchedTicket && searchedTicket.id === ticketId) {
        await searchTicketById();
      }
      
      alert('Ticket closed successfully!');
    } catch (err) {
      console.error('Error closing ticket:', err);
      alert('Error closing ticket. Please try again.');
    }
  };

  const toggleTicketExpansion = (ticketId: number) => {
    if (expandedTicketId === ticketId) {
      // Collapse if same ticket is clicked
      setExpandedTicketId(null);
    } else {
      // Expand clicked ticket (and collapse any other)
      setExpandedTicketId(ticketId);
    }
  };

  const handlePreviousPage = () => {
    if (pageNumber > 1) {
      setPageNumber(pageNumber - 1);
      setExpandedTicketId(null); // Collapse any expanded tickets when changing pages
    }
  };

  const handleNextPage = () => {
    setPageNumber(pageNumber + 1);
    setExpandedTicketId(null); // Collapse any expanded tickets when changing pages
  };

  useEffect(() => {
    fetchTickets();
  }, [refreshTrigger, pageNumber]);

  const formatDate = (dateString: string) => {
    try {
      return new Date(dateString).toLocaleString();
    } catch {
      return dateString;
    }
  };

  const getStatusBadgeClass = (status: string) => {
    const statusLower = status.toLowerCase();
    if (statusLower === 'open') return 'badge badge-trimble-open';
    if (statusLower === 'in progress') return 'badge badge-trimble-progress';
    if (statusLower === 'closed') return 'badge badge-trimble-closed';
    return 'badge badge-trimble-open';
  };

  const renderTicketRow = (ticket: Ticket, index: number, isSearchResult = false) => {
    const isClosed = ticket.status.toLowerCase() === 'closed';
    const isExpanded = expandedTicketId === ticket.id;
    
    return (
      <React.Fragment key={`${isSearchResult ? 'search' : 'list'}-${ticket.id}`}>
        <tr 
          onClick={() => toggleTicketExpansion(ticket.id)}
          className={`${isClosed ? 'table-danger' : ''}`}
          style={{ 
            cursor: 'pointer',
            borderBottom: isExpanded ? 'none' : undefined,
          }}
        >
          <td className="align-middle">
            <div className="d-flex align-items-center">
              <i 
                className={`bi bi-chevron-right me-2 text-muted transition-all ${isExpanded ? 'rotate-90' : ''}`}
                style={{ 
                  fontSize: '0.8rem',
                  transition: 'transform 0.2s ease',
                  transform: isExpanded ? 'rotate(90deg)' : 'rotate(0deg)'
                }}
              ></i>
              <span className="fw-bold text-trimble-blue">{ticket.id}</span>
            </div>
          </td>
          <td className="align-middle fw-semibold">{ticket.subject}</td>
          <td className="align-middle">{ticket.name}</td>
          <td className="align-middle text-muted">{ticket.email}</td>
          <td className="align-middle">
            <span className={getStatusBadgeClass(ticket.status)}>
              {ticket.status}
            </span>
          </td>
          <td className="align-middle text-muted small">
            {formatDate(ticket.createdDate)}
          </td>
          <td className="align-middle">
            {!isClosed && (
              <button
                onClick={(e) => {
                  e.stopPropagation();
                  withdrawTicket(ticket.id);
                }}
                className="btn btn-trimble-danger btn-sm d-flex align-items-center"
                type="button"
              >
                <i className="bi bi-trash me-1"></i>
                Withdraw
              </button>
            )}
          </td>
        </tr>
        
        {/* Expandable description row */}
        {isExpanded && (
          <tr className={isClosed ? 'table-danger' : ''}>
            <td colSpan={7} className="p-0">
              <div className="expandable-content p-4">
                <div className="mb-2">
                  <strong className="text-trimble-blue">
                    <i className="bi bi-file-text me-1"></i>
                    Description:
                  </strong>
                </div>
                <div className="bg-white p-3 rounded border">
                  <p className="mb-0 text-muted" style={{ whiteSpace: 'pre-wrap', lineHeight: 1.6 }}>
                    {ticket.description || 'No description provided.'}
                  </p>
                </div>
                <div className="mt-3 small text-muted fst-italic">
                  <i className="bi bi-info-circle me-1"></i>
                  Click anywhere on the ticket row to collapse this view
                </div>
              </div>
            </td>
          </tr>
        )}
      </React.Fragment>
    );
  };

  const renderTicketListContent = () => {
    if (loading) {
      return (
        <div className="text-center py-5">
          <div className="spinner-trimble mx-auto mb-3" style={{ width: '3rem', height: '3rem' }}></div>
          <p className="text-muted">Loading tickets...</p>
        </div>
      );
    }

    if (error) {
      return (
        <div className="alert alert-trimble-danger text-center py-4" role="alert">
          <i className="bi bi-exclamation-triangle display-6 text-danger mb-3"></i>
          <h5 className="alert-heading">Error Loading Tickets</h5>
          <p className="mb-3">{error}</p>
          <button 
            onClick={fetchTickets}
            className="btn btn-trimble-danger"
          >
            <i className="bi bi-arrow-clockwise me-1"></i>
            Try Again
          </button>
        </div>
      );
    }

    if (tickets.length === 0) {
      return (
        <div className="alert alert-trimble-warning text-center py-4" role="alert">
          <i className="bi bi-inbox display-6 text-warning mb-3"></i>
          <h5 className="alert-heading">No Tickets Found</h5>
          <p className="mb-0">
            There are currently no support tickets. Create your first ticket using the form above!
          </p>
        </div>
      );
    }

    return (
      <>
        <div className="mb-3 small text-muted fst-italic">
          <i className="bi bi-info-circle me-1"></i>
          Click on any ticket row to view its description
        </div>
        <div className="table-responsive">
          <table className="table table-trimble table-hover mb-0">
            <thead>
              <tr>
                <th scope="col">ID</th>
                <th scope="col">Subject</th>
                <th scope="col">Name</th>
                <th scope="col">Email</th>
                <th scope="col">Status</th>
                <th scope="col">Date</th>
                <th scope="col">Actions</th>
              </tr>
            </thead>
            <tbody>
              {tickets.map((ticket, index) => renderTicketRow(ticket, index))}
            </tbody>
          </table>
        </div>
      </>
    );
  };

  return (
    <div className="card card-trimble">
      {/* Ticket Search Section */}
      <div className="card-header">
        <h3 className="card-title h5 mb-3">
          <i className="bi bi-search me-2"></i>
          Search Ticket by ID
        </h3>
        <div className="row g-2 align-items-end">
          <div className="col-md-8 col-lg-6">
            <label htmlFor="searchId" className="form-label small fw-semibold">Ticket ID</label>
            <input
              type="number"
              className="form-control"
              id="searchId"
              value={searchId}
              onChange={(e) => setSearchId(e.target.value)}
              placeholder="Enter ticket ID"
            />
          </div>
          <div className="col-md-4 col-lg-3">
            <button
              onClick={searchTicketById}
              disabled={searchLoading || !searchId.trim()}
              className={`btn w-100 ${searchLoading ? 'btn-secondary' : 'btn-trimble-primary'}`}
            >
              {searchLoading ? (
                <>
                  <div className="spinner-trimble me-2" style={{ width: '1rem', height: '1rem' }}></div>
                  Searching...
                </>
              ) : (
                <>
                  <i className="bi bi-search me-1"></i>
                  Search
                </>
              )}
            </button>
          </div>
        </div>

        {searchError && (
          <div className="alert alert-trimble-danger mt-3 mb-0" role="alert">
            <i className="bi bi-exclamation-triangle me-2"></i>
            {searchError}
          </div>
        )}

        {searchedTicket && (
          <div className="mt-4">
            <h6 className="fw-semibold text-trimble-blue mb-3">
              <i className="bi bi-check-circle me-1"></i>
              Search Result:
            </h6>
            <div className="table-responsive">
              <table className="table table-trimble table-hover mb-0">
                <thead>
                  <tr>
                    <th scope="col">ID</th>
                    <th scope="col">Subject</th>
                    <th scope="col">Name</th>
                    <th scope="col">Email</th>
                    <th scope="col">Status</th>
                    <th scope="col">Date</th>
                    <th scope="col">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {renderTicketRow(searchedTicket, 0, true)}
                </tbody>
              </table>
            </div>
          </div>
        )}
      </div>

      {/* Main Ticket List */}
      <div className="card-body">
        <div className="d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center mb-4 gap-3">
          <h2 className="card-title h4 mb-0">
            <i className="bi bi-list-ul me-2"></i>
            All Support Tickets 
            <span className="badge bg-secondary ms-2">
              {loading ? '...' : `Page ${pageNumber}`}
            </span>
          </h2>
          <button 
            onClick={() => {
              setExpandedTicketId(null);
              fetchTickets();
            }}
            disabled={loading}
            className={`btn ${loading ? 'btn-secondary' : 'btn-trimble-success'}`}
          >
            {loading ? (
              <>
                <div className="spinner-trimble me-2" style={{ width: '1rem', height: '1rem' }}></div>
                Loading...
              </>
            ) : (
              <>
                <i className="bi bi-arrow-clockwise me-1"></i>
                Refresh
              </>
            )}
          </button>
        </div>

        {renderTicketListContent()}

        {/* Pagination Controls */}
        {!loading && !error && tickets.length > 0 && (
          <div className="d-flex flex-column flex-sm-row justify-content-between align-items-center mt-4 pt-3 border-top">
            <div className="text-muted small mb-2 mb-sm-0">
              <i className="bi bi-info-circle me-1"></i>
              Page {pageNumber} • Showing up to {pageSize} tickets per page
            </div>
            <div className="d-flex gap-2">
              <button
                onClick={handlePreviousPage}
                disabled={pageNumber === 1}
                className={`btn ${pageNumber === 1 ? 'btn-outline-secondary' : 'btn-trimble-primary'}`}
                title="Previous page"
              >
                <i className="bi bi-chevron-left me-1"></i>
                Previous
              </button>
              <button
                onClick={handleNextPage}
                disabled={tickets.length < pageSize}
                className={`btn ${tickets.length < pageSize ? 'btn-outline-secondary' : 'btn-trimble-primary'}`}
                title="Next page"
              >
                Next
                <i className="bi bi-chevron-right ms-1"></i>
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default TicketList; 