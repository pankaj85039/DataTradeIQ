# Signalara - Full Stack Application

A modern full-stack application with React frontend and .NET Core Web API backend.

## Project Structure

```
Signalara/
├── frontend/          # React + Vite application
│   ├── src/          # React source code
│   ├── public/       # Static assets
│   └── package.json  # Frontend dependencies
├── backend/          # .NET Core Web API
│   └── SignalaraAPI/ # Main API project
└── README.md         # Project documentation
```

## Prerequisites

- Node.js v20.16.0+ (preferably 20.19.0 or 22.12+)
- npm v10.8.0+
- .NET 9.0+
- Git
- Visual Studio Code with PowerShell

## Setup Instructions

### 1. Clone the Repository

```bash
git clone <repository-url>
cd Signalara
```

### 2. Frontend Setup

```bash
cd frontend
npm install
```

### 3. Backend Setup

```bash
cd backend/SignalaraAPI
dotnet restore
```

## Running the Application

### Option 1: Using VS Code (Recommended)

With two PowerShell terminals in VS Code:

**Terminal 1 - Frontend:**

```powershell
cd frontend
npm run dev
```

**Terminal 2 - Backend:**

```powershell
cd backend/SignalaraAPI
dotnet run
```

### Option 2: Running Separately

**Frontend (runs on http://localhost:5173):**

```bash
cd frontend
npm run dev
```

**Backend (runs on http://localhost:5000):**

```bash
cd backend/SignalaraAPI
dotnet run
```

## Development

### Frontend

- React 19 with Vite
- All development features enabled
- Hot Module Replacement (HMR)
- Scripts:
  - `npm run dev` - Start development server
  - `npm run build` - Build for production
  - `npm run preview` - Preview production build
  - `npm run lint` - Run ESLint

### Backend

- .NET 9.0 Web API
- ASP.NET Core 9.0
- RESTful API structure
- Default port: 5000 (HTTP) / 5001 (HTTPS)

## Project Configuration

### Frontend Configuration

- Framework: React 19
- Build Tool: Vite 7.3.1
- Port: 5173
- Config file: `frontend/vite.config.js`

### Backend Configuration

- Framework: ASP.NET Core 9.0
- Runtime: .NET 9.0
- Port: 5000 (HTTP) / 5001 (HTTPS)
- Config file: `backend/SignalaraAPI/appsettings.json`

## Git Workflow

The project is already initialized as a Git repository. The `.git` folder is present in the root directory.

### First Time Push to GitHub

```bash
# Add all files
git add .

# Create initial commit
git commit -m "Initial commit: React frontend + .NET backend"

# Add remote repository
git remote add origin <your-github-repo-url>

# Push to GitHub
git push -u origin main
```

## API Communication

- Frontend origin: `http://localhost:5173`
- Backend origin: `http://localhost:5000`
- CORS is configured in the backend API

## Environment Variables

### Frontend (.env)

```
VITE_API_URL=http://localhost:5000
```

### Backend (appsettings.json)

```json
{
  "AllowedHosts": "*",
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

## Troubleshooting

### Node.js Version Warning

If you see warnings about Node.js version, consider upgrading to 20.19.0 or 22.12+:

```bash
nvm install 20.19.0
nvm use 20.19.0
```

### Port Already in Use

- Frontend port 5173 already in use: Check other instances
- Backend port 5000 already in use: Modify `launchSettings.json`

### CORS Issues

Ensure CORS is enabled in `backend/SignalaraAPI/Program.cs`

## Contributing

1. Create a new branch for features
2. Commit your changes
3. Push to your branch
4. Create a Pull Request

## License

This project is open source and available under the MIT License.

## Support

For issues and questions, please create an issue on GitHub or contact the development team.
