public interface IEventBusService
{
    event Action? ProfileUpdated;
    void NotifyProfileUpdated();
}

public class EventBusService : IEventBusService
{
    public event Action? ProfileUpdated;
    
    public void NotifyProfileUpdated()
    {
        ProfileUpdated?.Invoke();
    }
}