# E-Commerce API Implementation Plan

## Phase 1 - Solution Setup

- Use the existing .NET 10 projects: `ECommerceAPI.WebAPI`, `ECommerceAPI.Application`, `ECommerceAPI.Domain`, and `ECommerceAPI.Infrastructure`.
- Add project references following Clean Architecture dependencies.
- Install packages for EF Core SQL Server, Identity, JWT, MediatR, FluentValidation, Scalar, Stripe, and Serilog.
- Configure LocalDB connection string, JWT options, Stripe options, and Serilog.
- Configure OpenAPI and Scalar with Bearer authentication.

## Phase 2 - Domain and Infrastructure

- Add domain entities: `User`, `Product`, `Cart`, `CartItem`, `Order`, `OrderItem`, and `RefreshToken`.
- Add `OrderStatus` enum.
- Add EF Core `ApplicationDbContext` with Identity and relationship configuration.
- Add strongly typed `JwtOptions` and `StripeOptions`.
- Add infrastructure services for JWT tokens, Stripe Checkout, current user access, role seeding, and database seeding.

## Phase 3 - Application Layer

- Add CQRS commands and queries for auth, products, cart, orders, and payments.
- Add DTOs, interfaces, result pattern, and validation behavior.
- Add FluentValidation validators for request commands.
- Keep use cases in the Application layer and depend on abstractions only.

## Phase 4 - API Layer

- Add controllers for auth, products, cart, orders, checkout, and Stripe webhook.
- Configure authentication and authorization policies.
- Add global exception middleware and consistent HTTP result mapping.
- Remove starter WeatherForecast API files.

## Phase 5 - MVP Behavior

- Register and login users with Customer role.
- Generate access and refresh tokens.
- Allow Admin users to manage products and authenticated customers to manage carts.
- Create pending orders from cart checkout and return a Stripe Checkout URL.
- Handle Stripe webhook confirmation to mark orders paid, reduce stock, and clear the cart.

## Phase 6 - Verification

- Run package restore.
- Build the full solution.
- Fix compile errors found during verification.
- Leave database migration/update for the developer environment because SQL Server availability and credentials are machine-specific.
