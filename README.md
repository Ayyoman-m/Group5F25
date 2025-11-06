# Group5F25.API


#Group5F25.APP  // For UI Layout and Design

### Sprint 1 – Task 1
- Set up initial .NET MAUI project structure for Driver Analytics App  
- Implemented MVVM base setup and page routing foundation  
- Created placeholder LoginPage and ViewModel files  
- Verified navigation and dependency injection system works  
- Prepared environment for future Sprint 2 tasks (API integration)

### Sprint 1 – Task 2
- Connected Login UI to DummyJSON API (/auth/login)  
- Sent username + password via AuthService using HttpClient  
- Parsed and stored accessToken for authenticated requests  
- Verified token using /auth/me endpoint  
- Added navigation to HomePage after successful login  
- Integrated AppShell, DI setup, and working login flow end-to-end

##### Recent Update: MySQL Integration 

- Integrated MySQL `AppDbContext` for user authentication.
- Updated `Program.cs` to register both MySQL and InMemory contexts.
- Modified `AuthService` to use dual databases (`AppDbContext` for users, `DriverAnalyticsContext` for analytics).
- Verified `register` and `login` through Swagger using MySQL persistence.
- This update enables permanent user storage instead of in-memory data.