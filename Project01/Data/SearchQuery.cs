namespace Project01.Data;

public class SearchQuery
{
    public int Id { get; set; }
    public string QueryText { get; set; }
    public DateTime SearchTime { get; set; }
    
    // Optional, add user information later on when I have introuced user accounts
    // public string UserId { get; set; }
}
