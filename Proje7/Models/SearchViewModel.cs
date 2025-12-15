namespace Proje7.Models
{
    public class SearchViewModel
    {
        public string? Sehir { get; set; }

        public DateTime GirisTarih { get; set; }
        public DateTime CikisTarih { get; set; }

        public DateOnly GirisGun => DateOnly.FromDateTime(GirisTarih);
        public DateOnly CikisGun => DateOnly.FromDateTime(CikisTarih);
    }
}
