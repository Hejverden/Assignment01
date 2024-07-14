namespace Assignment.Models;

// Notes to myself:

// Do not use positional record, because:
// It generates a primary constructor that takes parameters for all properties.
// It doesn't have a parameterless constructor by default, which is required for configuration binding with IOptions<T>.
// FX: This generates a constructor: public record FlickrApiSettings(string apiKey, string apiSecret) but no parameterless constructor.
// Regular classes can have both parameterless and parameterized constructors.
// The parameterless constructor is required for the configuration binding to work with IOptions<T>.

/// <summary>
/// The API key and secret values are saved securely inside the .env file.
/// The ApiKey and ApiSecret properties are defined inside the appsettings.json (no need to secure them!).
/// </summary>        
public class FlickrApiSettings
{
    public required string BaseUrl { get; set; }
    public required string EndpointPath { get; set; }
    public required string Method { get; set; }
    public required string Format { get; set; }
    public required string ApiKey { get; set; }
    public required string ApiSecret { get; set; }
}
