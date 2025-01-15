namespace WebApi.Models;

public class Device
{
    public Guid Id { get; set; }
    public List<Session> Sessions { get; set; } = new();
}