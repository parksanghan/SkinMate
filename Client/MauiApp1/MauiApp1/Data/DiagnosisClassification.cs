using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiApp1.Data
{
    // 
    public class DiagnosisClassification
    {
        [JsonPropertyName("forehead_wrinkle")]
        public int ForeheadWrinkle { get; set; }
        //이마 주름 

        [JsonPropertyName("frown_wrinkle")]
        public int FrownWrinkle { get; set; }
        //미간 주름

        [JsonPropertyName("eyes_wrinkle")]
        public int EyesWrinkle { get; set; }
        //눈가 주름

        [JsonPropertyName("cheek_pore")]
        public int CheekPore { get; set; }
        // 볼 모공

        [JsonPropertyName("lips_dryness")]
        public int LipsDryness { get; set; }
        //입술 건조도

        [JsonPropertyName("jaw_sagging")]
        public int JawSagging { get; set; }
        //턱쳐짐
    }
}
