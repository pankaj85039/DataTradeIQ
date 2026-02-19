# Development Guide

## Running Both Frontend and Backend in VS Code

This guide explains how to run both the React frontend and .NET backend simultaneously in VS Code using PowerShell terminals.

### Method 1: Using Two Separate PowerShell Terminals (Recommended)

1. **Open VS Code** and navigate to the Signalara project root

   ```bash
   code p:\projects\Signalara
   ```

2. **Open First PowerShell Terminal**
   - Press `Ctrl + Shift + Back Tick` to open integrated terminal
   - Or go to Terminal → New Terminal

3. **Start Frontend in Terminal 1:**

   ```powershell
   cd frontend
   npm run dev
   ```

   - Frontend will be available at `http://localhost:5173`
   - Leave this terminal running

4. **Open Second PowerShell Terminal**
   - Press `Ctrl + Shift + Back Tick` again
   - Select "New Terminal" or split terminal

5. **Start Backend in Terminal 2:**
   ```powershell
   cd backend\SignalaraAPI
   dotnet run
   ```

   - Backend will be available at `http://localhost:5000`
   - Leave this terminal running

### Method 2: Using VS Code Tasks

1. **Open Command Palette**: `Ctrl + Shift + P`
2. **Run Tasks**:
   - Search for "Tasks: Run Task"
   - Select "Start Frontend" or "Start Backend"
   - Or "Start All Services" to run both simultaneously

### Method 3: Using npm/dotnet Watch Mode

**Frontend with Watch Mode:**

```powershell
cd frontend
npm run dev
```

**Backend with Watch Mode:**

```powershell
cd backend\SignalaraAPI
dotnet watch run
```

This enables hot reload for both applications.

## File Structure for Development

```
frontend/
├── src/
│   ├── App.jsx          # Main App component
│   ├── App.css          # App styles
│   ├── main.jsx         # Entry point
│   ├── index.css        # Global styles
│   └── assets/          # Static assets
├── public/              # Public static files
├── vite.config.js       # Vite configuration
├── package.json         # Dependencies and scripts
└── eslint.config.js     # ESLint configuration

backend/
└── SignalaraAPI/
    ├── Controllers/     # API controllers
    ├── Models/          # Data models
    ├── Services/        # Business logic
    ├── Program.cs       # Application entry point
    ├── appsettings.json # Configuration
    └── SignalaraAPI.csproj
```

## Environment Setup

### Frontend Environment Variables

Create `.env` file in frontend directory:

```
VITE_API_URL=http://localhost:5000
```

Access in code:

```javascript
const apiUrl = import.meta.env.VITE_API_URL;
```

### Backend Environment Variables

Configure in `backend/SignalaraAPI/appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  },
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://localhost:5000"
      }
    }
  },
  "AllowedHosts": "*"
}
```

## Common Development Tasks

### Installing New Frontend Dependencies

```powershell
cd frontend
npm install package-name
```

### Installing New Backend Dependencies

In Visual Studio:

- Right-click project → Manage NuGet Packages

Or via CLI:

```powershell
cd backend\SignalaraAPI
dotnet add package PackageName
```

### Code Formatting

**Frontend (ESLint):**

```powershell
cd frontend
npm run lint
```

**Backend (.NET):**

```powershell
cd backend\SignalaraAPI
dotnet format
```

### Building for Production

**Frontend:**

```powershell
cd frontend
npm run build
```

Output: `frontend/dist/`

**Backend:**

```powershell
cd backend\SignalaraAPI
dotnet publish -c Release
```

Output: `backend/SignalaraAPI/bin/Release/net9.0/publish/`

## Debugging

### Frontend Debugging

In VS Code, install "Debugger for Chrome" extension, then:

1. Press `F5` or go to Run → Start Debugging
2. Set breakpoints in your React components
3. Frontend debug port: 9222

### Backend Debugging

1. Open backend folder in VS Code (separate window recommended)
2. Install C# Dev Kit extension
3. Press `F5` to start debugging
4. Set breakpoints in C# code

## Git Workflow

### Committing Changes

```powershell
# Check status
git status

# Add all changes
git add .

# Commit with message
git commit -m "feat: describe your changes"

# Push to remote
git push origin main
```

### Branching Strategy

```powershell
# Create feature branch
git checkout -b feature/feature-name

# Work on feature
# ... make changes ...

# Commit and push
git add .
git commit -m "feat: add feature"
git push origin feature/feature-name

# Create Pull Request on GitHub
# After review, merge to main
git checkout main
git merge feature/feature-name
git push origin main
```

## Troubleshooting

### Port Already in Use

**Frontend port 5173:**

```powershell
# Find process using port 5173
netstat -ano | findstr :5173
# Kill process (replace PID)
taskkill /PID <PID> /F
```

**Backend port 5000:**
Modify `backend/SignalaraAPI/Properties/launchSettings.json`:

```json
"applicationUrl": "http://localhost:5001"
```

### Clearing Cache

**Frontend:**

```powershell
cd frontend
rm node_modules -r
rm package-lock.json
npm install
```

**Backend:**

```powershell
cd backend/SignalaraAPI
rm bin -r
rm obj -r
dotnet restore
```

### Hot Reload Not Working

**Frontend:**

- Check if Vite dev server is running
- Hard refresh browser: `Ctrl + Shift + R` (Chrome/Firefox)

**Backend:**

- Use `dotnet watch run` instead of `dotnet run`
- Ensure file changes are being detected

## VS Code Extensions Recommended

- **ES7+ React/Redux/React-Native snippets** - dsznajder.es7-react-js-snippets
- **C# Dev Kit** - ms-dotnettools.csharp
- **Thunder Client** - rangav.vscode-thunder-client (for API testing)
- **REST Client** - humao.rest-client (for API testing)
- **GitLens** - eamodio.gitlens (for Git integration)

## Performance Tips

1. **Frontend:**
   - Use React.memo for expensive components
   - Code split with React.lazy
   - Monitor bundle size with `npm run build`

2. **Backend:**
   - Use async/await for I/O operations
   - Enable response compression
   - Use proper indexing for databases

3. **General:**
   - Monitor network requests in DevTools
   - Use proper error handling
   - Implement proper logging

## Next Steps

1. Set up database (SQL Server/PostgreSQL)
2. Implement authentication (JWT tokens)
3. Set up CI/CD pipeline (GitHub Actions)
4. Configure Docker for containerization
5. Set up testing framework (Jest for React, xUnit for .NET)
