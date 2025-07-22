# Support Portal UI

A React + TypeScript frontend for the Support Ticket Portal application.

## Features

- **Create Support Tickets**: Submit new support tickets with name, email, subject, and description
- **View Ticket List**: Display all submitted tickets in a clean table format
- **Real-time Updates**: Automatically refreshes the ticket list after creating new tickets
- **Responsive Design**: Works on desktop and mobile devices

## Components

### TicketForm
- Controlled form inputs for creating new tickets
- Form validation and submission handling
- Automatically clears form after successful submission
- Shows loading state during submission

### TicketList
- Fetches and displays tickets from the API
- Shows ticket ID, Subject, Name, Email, Status, and Created Date
- Loading and error states
- Manual refresh button
- Auto-refreshes when new tickets are submitted

### Shared Types
- `Ticket` interface in `src/types/Ticket.ts`

## Prerequisites

Make sure your backend API is running on `http://localhost:5000`

## Getting Started

1. Install dependencies:
```bash
npm install
```

2. Start the development server:
```bash
npm start
```

3. Open [http://localhost:3000](http://localhost:3000) to view it in the browser

## API Integration

The frontend connects to these API endpoints:
- `GET /api/tickets` - Fetch all tickets
- `POST /api/tickets` - Create new ticket

## Scripts

- `npm start` - Runs the app in development mode
- `npm run build` - Builds the app for production
- `npm test` - Launches the test runner
- `npm run eject` - Removes the build dependency

## Technologies Used

- React 18
- TypeScript
- Inline CSS styling (responsive and modern)
- Fetch API for HTTP requests
