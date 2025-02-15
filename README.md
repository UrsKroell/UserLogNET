# UserLogNET 
### a UserLog SDK for dotnet

-----

Website: https://getuserlog.com/

Docs: https://docs.getuserlog.com/

## Installation

```sh
dotnet add package UserLogNET
```

## Usage

### Add client to dependency injection

```csharp
using UserLogNET.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddUserLog("api-key", "project-name");
```

The project name will be auto-injected in all requests.

### Log

```csharp
using UserLogNET;
using UserLogNET.Client;

// Inject UserLogClient into service
public GenericController(IUserLogClient userLogClient)
{
    _userLogClient = userLogClient;
}

public async Task<bool> Get()
{
    // Create a new UserLogEvent
    var newLogEvent = new UserLogEvent()
    {
        Channel = "payments",                           // Required
        Event = "New Subscription",                     // Required
        Notify = true,                                  // Required
        
        Description = "A new subscription was created", // optional
        UserId = "user@example.local",                  // optional
        Icon = "💰"                                     // optional (needs to be an emoji)
    };
    
    // Add Tags (optional)
    newLogEvent.AddTags(
        ("plan", "hello"),       // supports string
        ("cycle", "monthly"),    
        ("mrr", 19.95),          // numbers
        ("trial", true)          // and bool
    );
    
    // push the LogEvent to be tracked
    return await _userLogClient.Track(newLogEvent);
}
```
