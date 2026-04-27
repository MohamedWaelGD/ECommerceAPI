E-Commerce API Plan
1. Goal

Build a simple API where:

User signs up
User logs in
User views products
User adds products to cart
User checks out using Stripe
Stripe confirms payment using webhook
Order becomes Paid
Cart is cleared
Product stock is reduced
2. Tech Stack
.NET 10 Web API
SQL Server
EF Core
ASP.NET Core Identity
JWT Authentication
Refresh Tokens
CQRS + MediatR
FluentValidation
Scalar OpenAPI
Stripe Checkout
Serilog
Clean Architecture
3. Solution Structure
ECommerceApi
│
├── src
│   ├── ECommerce.Api
│   ├── ECommerce.Application
│   ├── ECommerce.Domain
│   └── ECommerce.Infrastructure
│
└── tests
    └── ECommerce.Tests
4. Project Responsibilities
ECommerce.Domain

Contains core business entities.

Entities
Enums
Common base entities
Domain rules

Example:

User
Product
Cart
CartItem
Order
OrderItem
RefreshToken
ECommerce.Application

Contains use cases.

CQRS commands
CQRS queries
DTOs
Validators
Interfaces
Result pattern

Example:

CreateProductCommand
LoginCommand
AddCartItemCommand
CreateCheckoutSessionCommand
ECommerce.Infrastructure

Contains technical implementation.

DbContext
EF Core configurations
Identity setup
JWT service
Stripe service
Repositories
Migrations
ECommerce.Api

Contains API entry point.

Controllers
Program.cs
Scalar setup
Auth setup
Middlewares
appsettings.json
5. Domain Entities
User

User entity should be inside Domain.

using Microsoft.AspNetCore.Identity;

namespace ECommerce.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Cart? Cart { get; set; }
    public ICollection<Order> Orders { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}
Product
public class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public decimal Price { get; set; }
    public int StockQuantity { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
Cart
public class Cart
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public ICollection<CartItem> Items { get; set; } = [];
}
CartItem
public class CartItem
{
    public Guid Id { get; set; }

    public Guid CartId { get; set; }
    public Cart Cart { get; set; } = default!;

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;

    public int Quantity { get; set; }

    public decimal UnitPriceSnapshot { get; set; }
}
Order
public class Order
{
    public Guid Id { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public OrderStatus Status { get; set; } = OrderStatus.PendingPayment;

    public decimal TotalAmount { get; set; }

    public string? StripeCheckoutSessionId { get; set; }
    public string? StripePaymentIntentId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PaidAt { get; set; }

    public ICollection<OrderItem> Items { get; set; } = [];
}
OrderItem
public class OrderItem
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }
    public Order Order { get; set; } = default!;

    public Guid ProductId { get; set; }

    public string ProductNameSnapshot { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice => UnitPrice * Quantity;
}
RefreshToken
public class RefreshToken
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
6. Enums
public enum OrderStatus
{
    PendingPayment = 1,
    Paid = 2,
    PaymentFailed = 3,
    Cancelled = 4
}
7. Database Tables
AspNetUsers
AspNetRoles
AspNetUserRoles
AspNetUserClaims
AspNetRoleClaims
AspNetUserLogins
AspNetUserTokens

RefreshTokens

Products

Carts
CartItems

Orders
OrderItems
8. Main Relationships
User 1 --- 1 Cart
User 1 --- many Orders
User 1 --- many RefreshTokens

Cart 1 --- many CartItems
Product 1 --- many CartItems

Order 1 --- many OrderItems
Product 1 --- many OrderItems
9. Packages
Domain
dotnet add ECommerce.Domain package Microsoft.Extensions.Identity.Stores
Application
dotnet add ECommerce.Application package MediatR
dotnet add ECommerce.Application package FluentValidation
dotnet add ECommerce.Application package FluentValidation.DependencyInjectionExtensions
Infrastructure
dotnet add ECommerce.Infrastructure package Microsoft.EntityFrameworkCore.SqlServer
dotnet add ECommerce.Infrastructure package Microsoft.EntityFrameworkCore.Design
dotnet add ECommerce.Infrastructure package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add ECommerce.Infrastructure package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add ECommerce.Infrastructure package Stripe.net
API
dotnet add ECommerce.Api package Scalar.AspNetCore
dotnet add ECommerce.Api package Microsoft.AspNetCore.OpenApi
dotnet add ECommerce.Api package Serilog.AspNetCore
10. appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ECommerceDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Issuer": "ECommerceApi",
    "Audience": "ECommerceApiUsers",
    "Secret": "THIS_IS_DEMO_SECRET_KEY_CHANGE_IT_LATER_123456789",
    "AccessTokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  },
  "Stripe": {
    "SecretKey": "sk_test_xxx",
    "WebhookSecret": "whsec_xxx",
    "SuccessUrl": "https://localhost:4200/payment-success",
    "CancelUrl": "https://localhost:4200/payment-cancel"
  },
  "AllowedHosts": "*"
}

For SQL Server Express:

"DefaultConnection": "Server=.\\SQLEXPRESS;Database=ECommerceDb;Trusted_Connection=True;TrustServerCertificate=True"
11. DbContext
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence;

public class ApplicationDbContext 
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>()
            .Property(x => x.Price)
            .HasColumnType("decimal(18,2)");

        builder.Entity<CartItem>()
            .Property(x => x.UnitPriceSnapshot)
            .HasColumnType("decimal(18,2)");

        builder.Entity<Order>()
            .Property(x => x.TotalAmount)
            .HasColumnType("decimal(18,2)");

        builder.Entity<OrderItem>()
            .Property(x => x.UnitPrice)
            .HasColumnType("decimal(18,2)");

        builder.Entity<User>()
            .HasOne(x => x.Cart)
            .WithOne(x => x.User)
            .HasForeignKey<Cart>(x => x.UserId);

        builder.Entity<CartItem>()
            .HasIndex(x => new { x.CartId, x.ProductId })
            .IsUnique();
    }
}
12. Auth APIs
Endpoints
POST /api/auth/register
POST /api/auth/login
POST /api/auth/refresh-token
POST /api/auth/logout
GET  /api/auth/me
Register Request
{
  "fullName": "Mohamed Wael",
  "email": "test@example.com",
  "password": "123456"
}
Login Request
{
  "email": "test@example.com",
  "password": "123456"
}
Login Response
{
  "accessToken": "jwt_token",
  "refreshToken": "refresh_token",
  "expiresAt": "2026-04-27T15:30:00Z",
  "user": {
    "id": "guid",
    "fullName": "Mohamed Wael",
    "email": "test@example.com",
    "roles": ["Customer"]
  }
}
13. Product APIs
Endpoints
POST   /api/products
PUT    /api/products/{id}
GET    /api/products/{id}
GET    /api/products
DELETE /api/products/{id}
Permissions
Admin:
- Create product
- Update product
- Delete product

Customer:
- View products
Create Product Request
{
  "name": "T-Shirt",
  "description": "Black cotton t-shirt",
  "price": 450,
  "stockQuantity": 20,
  "imageUrl": "https://example.com/tshirt.png"
}
14. Cart APIs
Endpoints
GET    /api/cart
POST   /api/cart/items
PUT    /api/cart/items/{itemId}
DELETE /api/cart/items/{itemId}
DELETE /api/cart/clear
Add Cart Item Request
{
  "productId": "guid",
  "quantity": 2
}
Rules
User must be authenticated
User has one cart only
Cannot add inactive product
Cannot add quantity greater than stock
If product already exists in cart, increase quantity
Save product price snapshot
15. Order APIs
Endpoints
GET /api/orders
GET /api/orders/{id}
Rules
User can see only his own orders
Admin can see all orders later, but skip it in MVP
Order is created from checkout, not manually
16. Stripe Checkout APIs
Endpoints
POST /api/checkout
POST /api/payments/stripe/webhook
Checkout Flow
1. User adds items to cart
2. User calls POST /api/checkout
3. Backend validates stock
4. Backend creates Order with PendingPayment
5. Backend creates OrderItems from CartItems
6. Backend creates Stripe Checkout Session
7. Backend saves StripeCheckoutSessionId in Order
8. Backend returns checkoutUrl
9. Frontend redirects user to Stripe Checkout
10. User pays
11. Stripe sends webhook event
12. Backend validates webhook signature
13. Backend marks order as Paid
14. Backend reduces stock
15. Backend clears cart

Stripe recommends creating a new Checkout Session each time the customer attempts to pay, and webhooks are the correct way to react to payment events outside the direct payment flow.

Checkout Response
{
  "orderId": "guid",
  "checkoutUrl": "https://checkout.stripe.com/..."
}
17. Important Payment Rule

Never trust this:

/payment-success

The success URL only means the user was redirected.

Only trust:

Stripe webhook
18. CQRS Folder Structure
ECommerce.Application
│
├── Common
│   ├── Interfaces
│   ├── Models
│   └── Results
│
├── Features
│   ├── Auth
│   │   ├── Commands
│   │   │   ├── Register
│   │   │   ├── Login
│   │   │   ├── RefreshToken
│   │   │   └── Logout
│   │   └── Queries
│   │       └── GetCurrentUser
│   │
│   ├── Products
│   │   ├── Commands
│   │   │   ├── CreateProduct
│   │   │   ├── UpdateProduct
│   │   │   └── DeleteProduct
│   │   └── Queries
│   │       ├── GetProductById
│   │       └── GetProducts
│   │
│   ├── Cart
│   │   ├── Commands
│   │   │   ├── AddCartItem
│   │   │   ├── UpdateCartItem
│   │   │   ├── RemoveCartItem
│   │   │   └── ClearCart
│   │   └── Queries
│   │       └── GetMyCart
│   │
│   ├── Orders
│   │   └── Queries
│   │       ├── GetMyOrders
│   │       └── GetOrderById
│   │
│   └── Payments
│       └── Commands
│           ├── CreateCheckoutSession
│           └── HandleStripeWebhook
19. API Controller Structure
ECommerce.Api
│
├── Controllers
│   ├── AuthController.cs
│   ├── ProductsController.cs
│   ├── CartController.cs
│   ├── OrdersController.cs
│   └── PaymentsController.cs
20. Authorization Rules
Public APIs
POST /api/auth/register
POST /api/auth/login
GET  /api/products
GET  /api/products/{id}
POST /api/payments/stripe/webhook
Authenticated APIs
GET    /api/auth/me
POST   /api/auth/logout
POST   /api/auth/refresh-token

GET    /api/cart
POST   /api/cart/items
PUT    /api/cart/items/{id}
DELETE /api/cart/items/{id}

POST   /api/checkout

GET    /api/orders
GET    /api/orders/{id}
Admin APIs
POST   /api/products
PUT    /api/products/{id}
DELETE /api/products/{id}
21. Scalar Setup

Scalar should read auth from OpenAPI security schemes. Scalar documentation says authentication schemes must already exist in the OpenAPI document before Scalar can use them.

In Program.cs:

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new();

        document.Components.SecuritySchemes["Bearer"] = new()
        {
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Enter your JWT token"
        };

        document.SecurityRequirements.Add(new()
        {
            [new()
            {
                Reference = new()
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }] = []
        });

        return Task.CompletedTask;
    });
});

Then:

app.MapOpenApi();

app.MapScalarApiReference(options =>
{
    options.Title = "E-Commerce API";
});
22. JWT Setup

In Program.cs:

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt["Issuer"],

            ValidateAudience = true,
            ValidAudience = jwt["Audience"],

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Secret"]!)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

Pipeline:

app.UseAuthentication();
app.UseAuthorization();
23. Infrastructure Dependency Injection
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentityCore<User>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
        })
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddSignInManager();

        return services;
    }
}
24. Seed Default Roles

Seed:

Admin
Customer

Optional seed admin:

admin@demo.com
Admin123456
25. Development Phases
Phase 1 — Setup Solution

Deliverables:

Create projects
Add references
Install packages
Setup appsettings
Setup SQL Server connection
Setup DbContext
Setup migrations
Run database update
Setup Scalar

Done when:

API runs
Scalar opens
SQL Server database is created
Phase 2 — Auth

Deliverables:

Register
Login
Refresh token
Logout
Get current user
JWT generation
Role seeding

Done when:

User can register
User can login
JWT appears in response
JWT can be used in Scalar
Protected endpoint works
Phase 3 — Products

Deliverables:

Create product
Update product
Delete product
Get product by id
Get products list
Admin authorization
Validation

Done when:

Admin can manage products
Customer can only read products
Invalid prices are rejected
Inactive products are hidden from public list
Phase 4 — Cart

Deliverables:

Get my cart
Add item
Update quantity
Remove item
Clear cart
Stock validation

Done when:

Customer can add product to cart
Duplicate product increases quantity
Quantity cannot exceed stock
Cart total is calculated
Phase 5 — Orders

Deliverables:

Create order internally during checkout
Save order items
Get my orders
Get order details

Done when:

Checkout creates PendingPayment order
User can view his orders
User cannot view another user's order
Phase 6 — Stripe Checkout

Deliverables:

Create Checkout Session
Return checkout URL
Store Stripe session id
Handle webhook
Mark order as paid
Reduce stock
Clear cart

Done when:

User can pay with Stripe test card
Webhook updates order status
Cart is empty after payment
Stock is reduced
Phase 7 — Polish

Deliverables:

Global exception middleware
Result pattern
Validation pipeline behavior
Pagination for products
Pagination for orders
Serilog logging
Better response models
26. Suggested Endpoint List
/api/auth/register
/api/auth/login
/api/auth/refresh-token
/api/auth/logout
/api/auth/me

/api/products
/api/products/{id}

/api/cart
/api/cart/items
/api/cart/items/{itemId}
api/cart/clear

/api/checkout

/api/orders
/api/orders/{id}

/api/payments/stripe/webhook
27. What to Build First

Follow this order exactly:

1. Create solution
2. Setup SQL Server + DbContext
3. Setup Identity User in Domain
4. Setup JWT
5. Setup Scalar auth
6. Build Register/Login
7. Build Products
8. Build Cart
9. Build Order creation from cart
10. Build Stripe Checkout
11. Build Stripe Webhook
12. Add validations and cleanup
28. MVP Scope

Include:

Auth
Roles
Products
Cart
Orders
Stripe Checkout
Webhook
SQL Server
Scalar
JWT

Do not include now:

Coupons
Shipping
Reviews
Wishlist
Product variants
Categories
Refunds
Email sending
Admin dashboard
Inventory history
29. Final Learning Milestone

Your MVP is complete when this works:

Register as customer
Login
Use JWT in Scalar
Get products
Add product to cart
Create checkout session
Pay using Stripe test card
Receive webhook
Order status becomes Paid
Cart becomes empty
Stock decreases

Updated Additions to the Plan
Default Connection String
In ECommerce.Api/appsettings.json:
{  "ConnectionStrings": {    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ECommerceAPIDb;Trusted_Connection=True;TrustServerCertificate=True;"  }}

Serilog Logging
Install in API
dotnet add ECommerce.Api package Serilog.AspNetCoredotnet add ECommerce.Api package Serilog.Sinks.Consoledotnet add ECommerce.Api package Serilog.Sinks.File
appsettings.json
{  "Serilog": {    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.File"],    "MinimumLevel": {      "Default": "Information",      "Override": {        "Microsoft": "Warning",        "Microsoft.EntityFrameworkCore.Database.Command": "Warning"      }    },    "WriteTo": [      { "Name": "Console" },      {        "Name": "File",        "Args": {          "path": "Logs/ecommerce-api-.log",          "rollingInterval": "Day"        }      }    ],    "Enrich": ["FromLogContext"]  }}
Program.cs
using Serilog;builder.Host.UseSerilog((context, configuration) =>{    configuration.ReadFrom.Configuration(context.Configuration);});
Use logs in handlers/services:
_logger.LogInformation("Product created with id {ProductId}", product.Id);_logger.LogError(ex, "Failed to create checkout session for user {UserId}", userId);

Result Pattern
Location
ECommerce.Application└── Common    └── Results        ├── Result.cs        └── ResultT.cs
Result.cs
namespace ECommerce.Application.Common.Results;public class Result{    public bool IsSuccess { get; }    public bool IsFailure => !IsSuccess;    public string? Error { get; }    protected Result(bool isSuccess, string? error)    {        IsSuccess = isSuccess;        Error = error;    }    public static Result Success() => new(true, null);    public static Result Failure(string error) => new(false, error);}
ResultT.cs
namespace ECommerce.Application.Common.Results;public class Result<T> : Result{    public T? Value { get; }    private Result(bool isSuccess, T? value, string? error)        : base(isSuccess, error)    {        Value = value;    }    public static Result<T> Success(T value) => new(true, value, null);    public static new Result<T> Failure(string error) => new(false, default, error);}
Example usage:
public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken){    var product = await _context.Products.FindAsync([request.Id], cancellationToken);    if (product is null)        return Result<ProductDto>.Failure("Product not found.");    return Result<ProductDto>.Success(new ProductDto(product.Id, product.Name, product.Price));}

Configuration Pattern
Use strongly typed options instead of reading strings everywhere.
Folder
ECommerce.Infrastructure└── Options    ├── JwtOptions.cs    └── StripeOptions.cs
JwtOptions.cs
namespace ECommerce.Infrastructure.Options;public class JwtOptions{    public const string SectionName = "Jwt";    public string Issuer { get; set; } = string.Empty;    public string Audience { get; set; } = string.Empty;    public string Secret { get; set; } = string.Empty;    public int AccessTokenExpirationMinutes { get; set; }    public int RefreshTokenExpirationDays { get; set; }}
StripeOptions.cs
namespace ECommerce.Infrastructure.Options;public class StripeOptions{    public const string SectionName = "Stripe";    public string SecretKey { get; set; } = string.Empty;    public string WebhookSecret { get; set; } = string.Empty;    public string SuccessUrl { get; set; } = string.Empty;    public string CancelUrl { get; set; } = string.Empty;}
appsettings.json
{  "Jwt": {    "Issuer": "ECommerceApi",    "Audience": "ECommerceApiUsers",    "Secret": "THIS_IS_DEMO_SECRET_KEY_CHANGE_IT_LATER_123456789",    "AccessTokenExpirationMinutes": 60,    "RefreshTokenExpirationDays": 7  },  "Stripe": {    "SecretKey": "sk_test_xxx",    "WebhookSecret": "whsec_xxx",    "SuccessUrl": "https://localhost:4200/payment-success",    "CancelUrl": "https://localhost:4200/payment-cancel"  }}
Register options:
services.Configure<JwtOptions>(    configuration.GetSection(JwtOptions.SectionName));services.Configure<StripeOptions>(    configuration.GetSection(StripeOptions.SectionName));
Use in service:
private readonly JwtOptions _jwtOptions;public JwtTokenService(IOptions<JwtOptions> options){    _jwtOptions = options.Value;}

Dependency Injection for Each Layer
Domain
Usually no dependency injection.
Domain should not depend on Application, Infrastructure, or API.Domain contains entities, enums, value objects, and domain rules.

Application Dependency Injection
Create:
ECommerce.Application└── DependencyInjection.cs
using FluentValidation;using Microsoft.Extensions.DependencyInjection;using System.Reflection;namespace ECommerce.Application;public static class DependencyInjection{    public static IServiceCollection AddApplication(this IServiceCollection services)    {        var assembly = Assembly.GetExecutingAssembly();        services.AddMediatR(config =>            config.RegisterServicesFromAssembly(assembly));        services.AddValidatorsFromAssembly(assembly);        return services;    }}

Infrastructure Dependency Injection
Create:
ECommerce.Infrastructure└── DependencyInjection.cs
using ECommerce.Domain.Entities;using ECommerce.Infrastructure.Options;using ECommerce.Infrastructure.Persistence;using Microsoft.AspNetCore.Identity;using Microsoft.EntityFrameworkCore;using Microsoft.Extensions.Configuration;using Microsoft.Extensions.DependencyInjection;namespace ECommerce.Infrastructure;public static class DependencyInjection{    public static IServiceCollection AddInfrastructure(        this IServiceCollection services,        IConfiguration configuration)    {        services.Configure<JwtOptions>(            configuration.GetSection(JwtOptions.SectionName));        services.Configure<StripeOptions>(            configuration.GetSection(StripeOptions.SectionName));        services.AddDbContext<ApplicationDbContext>(options =>            options.UseSqlServer(                configuration.GetConnectionString("DefaultConnection")));        services.AddIdentityCore<User>(options =>        {            options.Password.RequiredLength = 6;            options.Password.RequireDigit = true;            options.Password.RequireUppercase = false;            options.Password.RequireLowercase = false;            options.Password.RequireNonAlphanumeric = false;        })        .AddRoles<IdentityRole<Guid>>()        .AddEntityFrameworkStores<ApplicationDbContext>()        .AddSignInManager();        // services.AddScoped<IJwtTokenService, JwtTokenService>();        // services.AddScoped<IStripeCheckoutService, StripeCheckoutService>();        // services.AddScoped<ICurrentUserService, CurrentUserService>();        return services;    }}

API Dependency Injection
In Program.cs:
using ECommerce.Application;using ECommerce.Infrastructure;using Serilog;var builder = WebApplication.CreateBuilder(args);builder.Host.UseSerilog((context, configuration) =>{    configuration.ReadFrom.Configuration(context.Configuration);});builder.Services.AddControllers();builder.Services.AddApplication();builder.Services.AddInfrastructure(builder.Configuration);builder.Services.AddAuthentication();builder.Services.AddAuthorization();builder.Services.AddOpenApi();var app = builder.Build();app.MapOpenApi();app.MapScalarApiReference();app.UseSerilogRequestLogging();app.UseHttpsRedirection();app.UseAuthentication();app.UseAuthorization();app.MapControllers();app.Run();

Updated Implementation Order
1. Create solution and layers2. Add connection string using LocalDB3. Add Domain entities4. Add Application dependency injection5. Add Infrastructure dependency injection6. Add DbContext and Identity7. Add configuration pattern for Jwt and Stripe8. Add Serilog9. Add Result pattern10. Add JWT authentication11. Add Scalar with Bearer auth12. Add Auth APIs13. Add Products APIs14. Add Cart APIs15. Add Orders APIs16. Add Stripe checkout17. Add Stripe webhook