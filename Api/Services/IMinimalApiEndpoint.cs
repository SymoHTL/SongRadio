namespace Api.Services;

public interface IMinimalApiEndpoint {
    public string Route { get; }
    
    void RegisterRoutes(WebApplication app);
}