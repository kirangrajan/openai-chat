
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


// Configure the HTTP request pipeline.





using System.Text.Json.Serialization;

public class Message
{
    public int Id { get; set; }
    [JsonPropertyName("username")]
    public string Username { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
