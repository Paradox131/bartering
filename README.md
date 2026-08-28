A barter-style marketplace built with ASP.NET Core MVC. Users list items and propose direct swaps — no money involved.

 Features
- User registration and login (ASP.NET Core Identity)
- Browse and search listings by category
- List items with photos
- Propose 1-for-1 swap offers
- Accept, decline, cancel, and complete swaps

The database is created automatically on first run via EF Core migrations.
Project structure
- "Models" — domain entities and view models
- "Data" — EF Core `ApplicationDbContext`
- "Services" — item and swap business logic
- "Controllers" — MVC controllers
- "Views" — Razor views
