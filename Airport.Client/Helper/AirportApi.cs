namespace Airport.Client.Helper
{
    public class AirportApi
    {
        public HttpClient Initial()
        {
            var Client = new HttpClient();
            Client.BaseAddress = new Uri("https://localhost:7235/");
            return Client;
        }
    }
}
