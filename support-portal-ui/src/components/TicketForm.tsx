import React, { useState } from 'react';

interface TicketFormProps {
  onTicketSubmitted: () => void;
}

interface FormData {
  name: string;
  email: string;
  subject: string;
  description: string;
}

const TicketForm: React.FC<TicketFormProps> = ({ onTicketSubmitted }) => {
  const [formData, setFormData] = useState<FormData>({
    name: '',
    email: '',
    subject: '',
    description: ''
  });

  const [isSubmitting, setIsSubmitting] = useState(false);
  const [submitError, setSubmitError] = useState<string | null>(null);
  const [submitSuccess, setSubmitSuccess] = useState(false);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
    
    // Clear error and success messages when user starts typing
    if (submitError) setSubmitError(null);
    if (submitSuccess) setSubmitSuccess(false);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    setSubmitError(null);
    setSubmitSuccess(false);

    try {
      const response = await fetch('http://localhost:5000/api/tickets', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(formData)
      });

      if (response.ok) {
        // Clear the form
        setFormData({
          name: '',
          email: '',
          subject: '',
          description: ''
        });
        
        // Show success state
        setSubmitSuccess(true);
        
        // Notify parent to refresh the list
        onTicketSubmitted();
        
        // Auto-hide success message after 3 seconds
        setTimeout(() => setSubmitSuccess(false), 3000);
      } else {
        setSubmitError('Failed to submit ticket. Please try again.');
      }
    } catch (error) {
      console.error('Error:', error);
      setSubmitError('Network error. Please check your connection and try again.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="card card-trimble">
      <div className="card-header">
        <h2 className="card-title h4 mb-2">
          <i className="bi bi-envelope-plus me-2"></i>
          Create Support Ticket
        </h2>
        <p className="card-text text-muted mb-0">
          Fill out the form below to submit a new support request. We'll get back to you as soon as possible.
        </p>
      </div>

      <div className="card-body">
        {/* Success Alert */}
        {submitSuccess && (
          <div className="alert alert-trimble-success d-flex align-items-center mb-4" role="alert">
            <i className="bi bi-check-circle-fill me-2"></i>
            <div>
              <strong>Success!</strong> Your support ticket has been submitted successfully.
            </div>
          </div>
        )}

        {/* Error Alert */}
        {submitError && (
          <div className="alert alert-trimble-danger d-flex align-items-center mb-4" role="alert">
            <i className="bi bi-exclamation-triangle-fill me-2"></i>
            <div>
              <strong>Error:</strong> {submitError}
            </div>
          </div>
        )}
        
        <form onSubmit={handleSubmit}>
          {/* Name and Email Row */}
          <div className="row">
            <div className="col-md-6">
              <div className="mb-3">
                <label htmlFor="name" className="form-label fw-semibold">
                  <i className="bi bi-person me-1"></i>
                  Your Name <span className="text-danger">*</span>
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="name"
                  name="name"
                  value={formData.name}
                  onChange={handleInputChange}
                  required
                  disabled={isSubmitting}
                  placeholder="Enter your full name"
                />
              </div>
            </div>

            <div className="col-md-6">
              <div className="mb-3">
                <label htmlFor="email" className="form-label fw-semibold">
                  <i className="bi bi-envelope me-1"></i>
                  Email Address <span className="text-danger">*</span>
                </label>
                <input
                  type="email"
                  className="form-control"
                  id="email"
                  name="email"
                  value={formData.email}
                  onChange={handleInputChange}
                  required
                  disabled={isSubmitting}
                  placeholder="your.email@example.com"
                />
              </div>
            </div>
          </div>

          {/* Subject */}
          <div className="mb-3">
            <label htmlFor="subject" className="form-label fw-semibold">
              <i className="bi bi-tag me-1"></i>
              Subject <span className="text-danger">*</span>
            </label>
            <input
              type="text"
              className="form-control"
              id="subject"
              name="subject"
              value={formData.subject}
              onChange={handleInputChange}
              required
              disabled={isSubmitting}
              placeholder="Brief description of your issue"
            />
          </div>

          {/* Description */}
          <div className="mb-4">
            <label htmlFor="description" className="form-label fw-semibold">
              <i className="bi bi-chat-text me-1"></i>
              Description <span className="text-danger">*</span>
            </label>
            <textarea
              className="form-control"
              id="description"
              name="description"
              value={formData.description}
              onChange={handleInputChange}
              required
              disabled={isSubmitting}
              rows={5}
              placeholder="Please provide detailed information about your issue, including any error messages, steps to reproduce, and what you expected to happen."
            />
          </div>

          {/* Submit Section */}
          <div className="d-flex flex-column flex-sm-row justify-content-between align-items-start align-items-sm-center gap-3">
            <div className="small text-muted">
              <i className="bi bi-info-circle me-1"></i>
              <span className="text-danger">*</span> Required fields
            </div>
            
            <button
              type="submit"
              disabled={isSubmitting}
              className={`btn ${isSubmitting ? 'btn-secondary' : 'btn-trimble-primary'} d-flex align-items-center`}
              style={{ minWidth: '180px' }}
            >
              {isSubmitting ? (
                <>
                  <div className="spinner-trimble me-2"></div>
                  Submitting...
                </>
              ) : (
                <>
                  <i className="bi bi-send me-2"></i>
                  Submit Ticket
                </>
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default TicketForm; 