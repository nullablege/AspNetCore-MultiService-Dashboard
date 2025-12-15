namespace Proje7.Models
{
    public class MainViewModel
    {
        //Oteller
        public HotelSearchResponse Oteller { get; set; }

        //Havadurumu
        public string SehirAdi { get; set; }
        public string İcon { get; set; }
        public string Derece {  get; set; }
        public string HavaDurumu { get; set; }
        public int Nem {  get; set; }
        public string Ruzgar {  get; set; }


        //Doviz
        public Double USD { get; set; }
        public Double EUR { get; set; }
        public Double GBP { get; set; }

        //Kripto
        public Double BTC { get; set; } = 0;
        public Double ETH { get; set; } = 0;
        public Double BNB { get; set; } = 0;

        //Akaryakıt

        public string Benzin { get; set; }
        public string Motorin { get; set; }
        public string LPG { get; set; }

        //Yemek
        public string GununYemegiAdi { get; set; }
        public string GununYemegiTarifi { get; set; }

        //Gezi Rotalari
        public string Rota1Baslik { get; set; }
        public string Rota1Detay { get; set; }

        public string Rota2Baslik { get; set; }
        public string Rota2Detay { get; set; }

        public string Rota3Baslik { get; set; }
        public string Rota3Detay { get; set; }

        public string Rota4Baslik { get; set; }
        public string Rota4Detay { get; set; }
        
        public string Rota5Baslik { get; set; }
        public string Rota5Detay { get; set; }



    }
}
