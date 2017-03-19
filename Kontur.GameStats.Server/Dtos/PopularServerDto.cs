namespace Kontur.GameStats.Server.Dtos
{
    public class PopularServerDto
    {
        public string Endpoint { get; set; }
        public string Name { get; set; }
        public double AverageMatchesPerDay { get; set; }
    }
}