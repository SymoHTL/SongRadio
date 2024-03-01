namespace Api.Services;

public interface IMinimalApiEndpoint {
    public string Route { get; set; }
    
    void RegisterRoutes(WebApplication app);
}