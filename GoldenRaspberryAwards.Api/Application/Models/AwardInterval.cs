namespace GoldenRaspberryAwards.Api.Application.Models
{
    public class AwardInterval
    {
        public string Producer { get; set; } // Nome do produtor
        public int Interval { get; set; }   // Intervalo de tempo entre prêmios
        public int PreviousWin { get; set; } // Ano da vitória anterior
        public int FollowingWin { get; set; } // Ano da vitória seguinte
    }
}
