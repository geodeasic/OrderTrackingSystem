# Order Management System API

## Overview
This project is a .NET 8 Web API that simulates an order management system. It demonstrates clean architecture, extensible design, and best practices for API development, testing, and performance optimization.

## Features
- **Discounting System:** Applies different promotion rules based on customer segments and order history.
- **Order Status Tracking:** Supports appropriate state transitions for order statuses.
- **Order Analytics Endpoint:** Returns analytics such as average order value, fulfillment time, and more.
- **Performance Optimizations:** Includes caching for analytics data and batch loading to avoid N+1 query issues.

## Getting Started
1. **Clone the repository:**
   ```bash
   git clone <your-repo-url>
   ```
2. **Navigate to the solution directory:**
   ```bash
   cd OrderTrackingSystem
   ```
3. **Run the API:**
   ```bash
   dotnet run --project Orders.ApiService/Orders.ApiService.csproj
   ```
4. **Swagger UI:**
   Visit `https://localhost:<port>/swagger` for interactive API documentation.

## Testing
- **Unit Tests:**
  - Located in `Orders.Domain.Tests` and `Orders.Application.Tests` projects.
- **Integration Tests:**
  - Located in `Orders.ApiService.Tests`.
- Run all tests:
  ```bash
  dotnet test
  ```

## Performance Optimizations
- **Batch Loading:** Customer profiles are batch-loaded to avoid N+1 query patterns.
- **Caching:** Analytics data is cached in memory for 1 minute to reduce computation overhead.

## API Documentation
- Annotated with Swagger/OpenAPI for easy exploration and testing.

## Assumptions
- In-memory repositories are used for demonstration and testing purposes.
- Promotion rules and order status transitions are extensible.

## Contact
For questions or contributions, please open an issue or submit a pull request.
