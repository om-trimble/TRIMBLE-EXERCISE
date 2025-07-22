import React, { useState } from 'react';
import TicketForm from './components/TicketForm';
import TicketList from './components/TicketList';
import './App.css';

function App() {
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  const handleTicketSubmitted = () => {
    // Increment refresh trigger to cause TicketList to refetch
    setRefreshTrigger(prev => prev + 1);
  };

  return (
    <div className="App">
      {/* Trimble Navbar */}
      <nav className="navbar navbar-expand-lg" style={{ backgroundColor: 'white' }}>
  <div className="container-fluid">
    <a
      className="navbar-brand d-flex align-items-center"
      href="#"
      onClick={(e) => e.preventDefault()}
      style={{ color: '#0063a3' }}
    >
      <img
        src="https://filecache.mediaroom.com/mr5mr_trimble/178498/TrimbleR-Horiz-RGB-Blue%201000px.jpg"
        alt="Trimble Logo"
        className="trimble-logo me-3"
        style={{ height: '40px' }}
      />
      {/* <span className="fw-bold">Support Portal</span> */}
    </a>
    <div className="navbar-nav ms-auto">
      <span className="navbar-text" style={{ color: '#0063a3' }}>
        Professional Ticket Management
      </span>
    </div>
  </div>
</nav>


      {/* Hero Header */}
      <header className="header-trimble">
        <div className="container">
          <div className="row">
            <div className="col-12 text-center">
              <h1 className="display-4 fw-bold mb-3">🎫 Support Portal</h1>
              <p className="lead mb-0">
                Your comprehensive ticket management system powered by Trimble technology
              </p>
            </div>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className="main-content">
        <div className="container">
          <div className="row justify-content-center">
            <div className="col-12 col-xl-10">
              <div className="fade-in">
                <TicketForm onTicketSubmitted={handleTicketSubmitted} />
                <TicketList refreshTrigger={refreshTrigger} />
              </div>
            </div>
          </div>
        </div>
      </main>

      {/* Footer */}
      <footer className="footer-trimble">
        <div className="container">
          <div className="row">
            <div className="col-md-6">
              <h6 className="fw-bold mb-2">Support Portal</h6>
              <p className="text-muted small mb-0">
                Powered by React + TypeScript + ASP.NET Core
              </p>
            </div>
            <div className="col-md-6 text-md-end">
              <p className="small mb-2">
                <strong>Trimble Inc.</strong> • Professional Solutions
              </p>
              <p className="text-muted small mb-0">
                Built with ❤️ for better customer support experience
              </p>
            </div>
          </div>
          <hr className="my-3 opacity-25" />
          <div className="row">
            <div className="col-12 text-center">
              <p className="text-muted small mb-0">
                © 2024 Trimble Inc. All rights reserved.
              </p>
            </div>
          </div>
        </div>
      </footer>
    </div>
  );
}

export default App;
