using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biyoinformatikFinalProjesi
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // başlangıç popülasyonunu oluşturma işlemi
            List<Alignment> populasyon = new List<Alignment>();
            for (int i = 0; i < popoulasyonUzunlugu; i++)
            {
                populasyon.Add(RastgeleHizalama());
            }

            // nesil sayacı
            int nesil = 0;

            // genetik algoritma döngüsü
            while (nesil < iterasyon)
            {
                // popülasyondan en yüksek uygunluk skoruna sahip olan hizalamayı bul
                Alignment bestAlignment = populasyon.OrderByDescending(a => a.uygunluk).First();
                // en iyi hizalamayı ekrana yazdır
                Console.WriteLine("En iyi Hizalama:");
                bestAlignment.yaz();
                Console.WriteLine("iterasyon: " + nesil);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("--------------");
                Console.ResetColor();
                // eğer en iyi skor maksimum skora yakınsa, o zaman döngüyü durma işlemi gerçekleştir
                int eniyiSkor = sequences.Length * (sequences.Length - 1) / 2 * uzunluk;
                if (bestAlignment.uygunluk >= eniyiSkor * 0.9)
                {
                    break;
                }
                // yeni popülasyonu oluştur
                List<Alignment> yeniPopulasyon = new List<Alignment>();
                for (int i = 0; i < popoulasyonUzunlugu / 2; i++)
                {
                    // popülasyondan iki hizalamayı seçme
                    Alignment parent1 = populasyon[random.Next(popoulasyonUzunlugu)];
                    Alignment parent2 = populasyon[random.Next(popoulasyonUzunlugu)];
                    // Çaprazlama yapma olasılığına bak
                    if (random.NextDouble() < crossoverOrani)
                    {
                        // çaprazlama yap ve iki çocuk hizalama elde et
                        Alignment[] children = Crossover(parent1, parent2);
                        // mutasyon yapma olasılığına bak
                        if (random.NextDouble() < mutasyonOrani)
                        {
                            // birinci çocuk hizalamasını mutasyon yap
                            children[0] = Mutasyon(children[0]);
                        }
                        if (random.NextDouble() < mutasyonOrani)
                        {
                            // ikinci çocuk hizalamasını mutasyon yap
                            children[1] = Mutasyon(children[1]);
                        }
                        // çocuk hizalamaları yeni popülasyona ekle
                        yeniPopulasyon.AddRange(children);
                    }
                    else
                    {// çaprazlama yapma işlemi ve ebeveyn hizalamalarını yeni popülasyona ekle
                        yeniPopulasyon.Add(parent1);
                        yeniPopulasyon.Add(parent2);
                    }
                }
                // popülasyonu güncelle
                populasyon = yeniPopulasyon;

                // nesil sayacını arttır
                nesil++;
            }

        }

        // sabit değişkenler
        const int uzunluk = 10; // DNA dizilerinin uzunluğu
        const int popoulasyonUzunlugu = 100; // popülasyon boyutu
        const double mutasyonOrani = 0.1; // mutasyon oranı
        const double crossoverOrani = 0.8; // çaprazlama oranı
        const int iterasyon = 30; // maksimum nesil sayısı

        // radndom sayı üreteci
        static Random random = new Random();

        // dNA dizileri
        static string[] sequences = new string[] { "GTCGGCTAAG", "TTCGGCTTGT", "ATCGGCTAGG", "CTCGGCTATG" };

        // hizalama sınıfı
        public class Alignment
        {
            // hizalanmış diziler
            public string[] alignedSequences;
            // uygunluk puanı
            public int uygunluk;

            
            public Alignment(string[] alignedSequences)
            {
                this.alignedSequences = alignedSequences;
                this.uygunluk = uygunlukHesapla();
            }

            // uygunluk skorunu hesaplama işlemi
            public int uygunlukHesapla()
            {
                int score = 0;
                for (int i = 0; i < uzunluk; i++)
                {
                    // sütundaki harfler
                    char[] column = new char[sequences.Length];
                    for (int j = 0; j < sequences.Length; j++)
                    {
                        column[j] = alignedSequences[j][i];
                    }
                    // sütundaki eşleşme sayısı
                    int matchCount = 0;
                    var groups = column.GroupBy(c => c).Where(g => g.Key != '-');
                    if (groups.Any())
                    {
                        matchCount = groups.Max(g => g.Count());
                    }
                    // sütundaki boşluk sayısı
                    int gapCount = column.Count(c => c == '-');
                    // sütundaki yanlış eşleşme sayısı
                    int mismatchCount = column.Length - matchCount - gapCount;
                    // sütun skoru
                    int columnScore = matchCount - gapCount - mismatchCount;
                    // toplam skora ekle
                    score += columnScore;
                }
                return score;
            }

          
            public void yaz()
            {
                foreach (string sequence in alignedSequences)
                {
                    Console.WriteLine(sequence);
                }

                Console.WriteLine("Uygunluk: " + uygunluk);

            }
        }

        // rastgele bir hizalama oluşturma
        public static Alignment RastgeleHizalama()
        {
            string[] alignedSequences = new string[sequences.Length];
            for (int i = 0; i < sequences.Length; i++)
            {
                alignedSequences[i] = sequences[i];
                // rastgele sayıda boşluk ekle
                int gapCount = random.Next(uzunluk / 2);
                for (int j = 0; j < gapCount; j++)
                {
                    // rastgele bir pozisyona boşluk ekle
                    int position = random.Next(uzunluk + j);
                    alignedSequences[i] = alignedSequences[i].Insert(position, "-");
                }
            }
            return new Alignment(alignedSequences);
        }

        // iki hizalamayı çaprazlama işlemi
        public static Alignment[] Crossover(Alignment parent1, Alignment parent2)
        {
            string[] child1Sequences = new string[sequences.Length];
            string[] child2Sequences = new string[sequences.Length];
            for (int i = 0; i < sequences.Length; i++)
            {
                // rastgele bir kesim noktası seç
                int cutPoint = random.Next(uzunluk);
                // parçaları değiştir
                child1Sequences[i] = parent1.alignedSequences[i].Substring(0, cutPoint) + parent2.alignedSequences[i].Substring(cutPoint);
                child2Sequences[i] = parent2.alignedSequences[i].Substring(0, cutPoint) + parent1.alignedSequences[i].Substring(cutPoint);
            }
            return new Alignment[] { new Alignment(child1Sequences), new Alignment(child2Sequences) };
        }

        // bir hizalamaya mutasyon uygulama işlemi
        public static Alignment Mutasyon(Alignment alignment)
        {
            string[] mutatedSequences = new string[sequences.Length];
            for (int i = 0; i < sequences.Length; i++)
            {
                mutatedSequences[i] = alignment.alignedSequences[i];
                // rastgele bir pozisyon seç
                int position = random.Next(uzunluk);
                // rastgele bir işlem seç
                int operation = random.Next(3);
                if (operation == 0)
                {
                    // boşluk ekle
                    mutatedSequences[i] = mutatedSequences[i].Insert(position, "-");
                    // son karakteri sil
                    mutatedSequences[i] = mutatedSequences[i].Remove(uzunluk);
                }
                else if (operation == 1)
                {
                    // boşluk sil
                    if (mutatedSequences[i][position] == '-')
                    {
                        mutatedSequences[i] = mutatedSequences[i].Remove(position, 1);
                        char randomChar = "ATCG"[random.Next(4)];
                        mutatedSequences[i] += randomChar;
                    }
                }
                else if (operation == 2)
                {
                    // harf değiştir
                    if (mutatedSequences[i][position] != '-')
                    {
                        char randomChar = "ATCG"[random.Next(4)];
                        mutatedSequences[i] = mutatedSequences[i].Remove(position, 1).Insert(position, randomChar.ToString());
                    }
                }
            }
            return new Alignment(mutatedSequences);
        }
    }
}
