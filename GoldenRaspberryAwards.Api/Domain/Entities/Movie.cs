namespace GoldenRaspberryAwards.Api.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Title { get; set; } = string.Empty; // Obrigatório
        public string Studios { get; set; } = string.Empty; // Obrigatório
        public string Producers { get; set; } = string.Empty;
        public bool IsWinner { get; set; }
    }


}
