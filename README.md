﻿# Order Management System API

## Overview
This project is a .NET 8 Web API that simulates an order management system. I
t demonstrates clean architecture, extensible design, and best practices for API development, 
testing, and performance optimization.

## Approach
- Implemented a modular .NET 8 Web API using clean architecture principles.
- Added a discounting system with rules based on customer segments and order history.
- Integrated order status tracking with valid state transitions.
- Provided an analytics endpoint for average order value, fulfillment time, etc.
- Used dependency injection and service abstractions for testability and maintainability.
- Documented API endpoints with Swagger/OpenAPI.
- Wrote unit tests for discount logic and integration tests for API endpoints.
- Applied performance optimizations (e.g., in-memory caching for analytics and query optimization for retrieving customer profile).

## Assumptions
- Customer segments and promotion rules are predefined and static for this implementation.
- Order status transitions follow a linear workflow (i.e, Pending → Confirmed → Shipped → Delivered → Returned → Cancelled → Closed).
- Analytics calculations are based on available in-memory data.
- No external database or persistent storage is used; all data is in-memory for demonstration.
- Authentication and authorization are out of scope for this assignment.

## Features
- **Discounting System:** Applies different promotion rules based on customer segments and order history.
- **Order Status Tracking:** Supports appropriate state transitions for order statuses.
- **Order Analytics Endpoint:** Returns analytics such as number of orders in last 7 days, average fulfillment time, and more.
- **Performance Optimizations:** Includes caching for analytics data and batch loading to avoid N+1 query issues.

## Getting Started
1. **Clone the repository:**
   ```bash
   git clone https://github.com/geodeasic/OrderTrackingSystem.git
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
   Visit `https://localhost:7071/swagger` for interactive API documentation.

## Testing Approach
### Testing Frameworks
- Unit tests written based on xUnit and Moq for mocking dependencies.
- Integration tests written based on xUnit and Moq for mocking dependencies.

### Test Coverage
- Unit tests focus on the discount calculation logic and order status transitions.
- Integration tests focus the Orders API endpoint.

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
