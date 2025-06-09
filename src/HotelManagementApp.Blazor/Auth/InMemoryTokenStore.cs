namespace HotelManagementApp.Blazor.Auth
{
    public interface ITokenStore
    {
        string? AccessToken { get; set; }
        string? RefreshToken { get; set; }
        void Clear();
    }

    public class InMemoryTokenStore : ITokenStore
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }

        public void Clear()
        {
            AccessToken = null;
            RefreshToken = null;
        }
    }
}