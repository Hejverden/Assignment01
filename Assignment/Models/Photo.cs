using System.Text.Json.Serialization;

namespace Assignment.Models;

/// <summary>
/// Photo as fetched from Flickr API.
/// </summary>
public class Photo
{
    /// <summary>
    /// Gets or sets the ID of the photo.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the owner of the photo.
    /// </summary>
    [JsonPropertyName("owner")]
    public required string Owner { get; set; }

    /// <summary>
    /// Gets or sets the secret of the photo.
    /// </summary>
    [JsonPropertyName("secret")]
    public required string Secret { get; set; }

    /// <summary>
    /// Gets or sets the server of the photo.
    /// </summary>
    [JsonPropertyName("server")]
    public required string Server { get; set; }

    /// <summary>
    /// Gets or sets the farm of the photo.
    /// </summary>
    [JsonPropertyName("farm")]
    public required int Farm { get; set; }

    /// <summary>
    /// Gets or sets the title of the photo.
    /// </summary>
    [JsonPropertyName("title")]
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the photo is public.
    /// </summary>
    [JsonPropertyName("ispublic")]
    public required int IsPublic { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the photo is a friend.
    /// </summary>
    [JsonPropertyName("isfriend")]
    public required int IsFriend { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the photo is family.
    /// </summary>
    /// [JsonPropertyName("isfamily")]
    public required int IsFamily { get; set; }


    /// <summary>
    /// Construct the source URL to a photo once we know its ID, server ID, and secret.
    /// This info is obtained from the API method (here search method, but also by many API methods).
    /// Instructions on how to construct the image URL are given by: https://www.flickr.com/services/api/misc.urls.html
    /// </summary>
    public string ImageUrl
    {
        get
        {
            return $"https://live.staticflickr.com/{Server ?? "default"}/{Id ?? "default"}_{Secret ?? "default"}_m.jpg"; // m for small size
        }
    }
}


/// <summary>
/// 
/// </summary>
public class PhotosResponse
{
    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("pages")]
    public int Pages { get; set; }

    [JsonPropertyName("perpage")]
    public int PerPage { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("photo")]
    public required List<Photo> Photo { get; set; }
}

/// <summary>
/// FlickrApiResponse is a class that matches the JSON structure of the response from Flickr API.
/// </summary>
public class FlickrApiResponse
{
    [JsonPropertyName("photos")]
    public PhotosResponse Photos { get; set; }

    [JsonPropertyName("stat")]
    public string Stat { get; set; }
}