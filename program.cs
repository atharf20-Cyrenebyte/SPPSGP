using System;

class Program
{
    static string[] history = new string[24];   
    static int currentHour = 0;                 

    static void Main()
    {
        for (int i = 0; i < 24; i++)
            history[i] = "Undefined";

        while (true)
        {
            Console.WriteLine("\n=== SIMULATOR STATUS GUNUNG BERAPI ===");
            Console.WriteLine("1. Cek Status Gunung Berapi");
            Console.WriteLine("2. Lihat History Status");
            Console.WriteLine("3. Reset Data");
            Console.WriteLine("4. Keluar");
            Console.Write("Pilih menu: ");
            string pilih = Console.ReadLine();

            switch (pilih)
            {
                case "1":
                    CekStatus();
                    break;

                case "2":
                    LihatHistory();
                    break;

                case "3":
                    ResetData();
                    break;

                case "4":
                    return;

                default:
                    Console.WriteLine("Menu tidak dikenal!");
                    break;
            }
        }
    }

    // MENU 1 – CEK STATUS
    static void CekStatus()
    {
        if (currentHour >= 24)
        {
            Console.WriteLine("Pencatatan sudah penuh 24 jam! Harus reset terlebih dahulu.");
            return;
        }

        Console.WriteLine("\n=== INPUT DATA KRITERIA ===");

        // 9 input
        double magnitudo = InputDouble("Magnitudo Gempa: ");
        string awan = InputYaTidak("Awan Panas (ya/tidak): ");
        double suhu = InputDouble("Suhu (°C): ");
        double kelembapan = InputDouble("Kelembapan (%): ");
        string gas = InputYaTidak("Gas Berbahaya (ya/tidak): ");
        string lahar = InputYaTidak("Lahar Letusan (ya/tidak): ");
        string lava = InputYaTidak("Lava (ya/tidak): ");
        string lontar = InputYaTidak("Lontaran Batu Pijar (ya/tidak): ");
        string hujan = InputYaTidak("Hujan Abu (ya/tidak): ");

        string status = TentukanStatus(
            magnitudo, awan, suhu, kelembapan,
            gas, lahar, lava, lontar, hujan
        );

        Console.WriteLine($"\n>> STATUS GUNUNG BERAPI: {status}");

        // Pesan tambahan sesuai status
        if (status.Contains("WASPADA"))
        {
            Console.WriteLine("Perlu disiapkan masker sebanyak penduduk yang ada.");
        }
        else if (status.Contains("SIAGA"))
        {
            Console.WriteLine("Perlu disiapkan masker 3x jumlah penduduk, alat radio panggil setiap desa, dan evakuasi radius < 6 KM.");
        }
        else if (status.Contains("AWAS"))
        {
            Console.WriteLine("Perlu disiapkan masker 6x jumlah penduduk, radio panggil setiap desa, evakuasi radius < 10 KM.");
        }

        // Simpan ke history
        string record = $"{magnitudo};{awan};{suhu};{kelembapan};{gas};{lahar};{lava};{lontar};{hujan};{status}";
        history[currentHour] = record;
        Console.WriteLine($"\nData tercatat pada jam ke-{currentHour + 1}");
        currentHour++;
    }

    // MENU 2 – LIHAT HISTORY=
    static void LihatHistory()
    {
        Console.WriteLine("\n=== LIHAT HISTORY ===");
        Console.WriteLine("1. Tampilkan semua data");
        Console.WriteLine("2. Tampilkan data per jam");
        Console.Write("Pilih: ");
        string pilih = Console.ReadLine();

        if (pilih == "1")
        {
            for (int i = 0; i < 24; i++)
            {
                Console.Write($"Pukul {i + 1}: ");
                if (history[i] == "Undefined")
                    Console.WriteLine("Pencatatan status gunung berapi belum dilakukan");
                else
                    Console.WriteLine(history[i]);
            }
        }
        else if (pilih == "2")
        {
            Console.Write("Masukkan jam (1–24): ");
            int j = int.Parse(Console.ReadLine()) - 1;

            if (j < 0 || j > 23)
                Console.WriteLine("Jam tidak valid!");
            else if (history[j] == "Undefined")
                Console.WriteLine("Pencatatan status gunung berapi belum dilakukan");
            else
                Console.WriteLine(history[j]);
        }
        else
        {
            Console.WriteLine("Pilihan tidak tersedia!");
        }
    }
    // MENU 3 – RESET DATA
    static void ResetData()
    {
        Console.Write("Apakah yakin ingin reset data? (ya/tidak): ");
        string ans = Console.ReadLine().ToLower();

        if (ans == "ya")
        {
            for (int i = 0; i < 24; i++)
                history[i] = "Undefined";

            currentHour = 0;
            Console.WriteLine(">> Semua data berhasil direset.");
        }
        else
        {
            Console.WriteLine("Reset dibatalkan.");
        }
    }
    // LOGIKA PENENTUAN STATUS
    static string TentukanStatus(
        double m, string awan, double suhu, double kelembapan,
        string gas, string lahar, string lava,
        string lontar, string hujan)
    {
        // Level 4 – AWAS
        if (m > 5.2 || suhu > 40 || kelembapan <= 4 ||
            lahar == "ya" || lava == "ya" ||
            lontar == "ya" || hujan == "ya" || gas == "ya")
            return "AWAS_Level_IV";

        // Level 3 – SIAGA
        if ((m >= 4.0 && m <= 5.2) ||
            (suhu >= 38 && suhu <= 40) ||
            (kelembapan >= 5 && kelembapan <= 9))
            return "SIAGA_Level_III";

        // Level 2 – WASPADA
        if ((m >= 2.9 && m <= 3.9) ||
            (suhu >= 33 && suhu <= 37) ||
            (kelembapan >= 10 && kelembapan <= 14))
            return "WASPADA_Level_II";

        // Level 1 – NORMAL
        return "NORMAL_Level_I";
    }
    // VALIDATOR INPUT
    static double InputDouble(string msg)
    {
        double x;
        Console.Write(msg);
        while (!double.TryParse(Console.ReadLine(), out x))
        {
            Console.Write("Input tidak valid, masukkan angka: ");
        }
        return x;
    }

    static string InputYaTidak(string msg)
    {
        Console.Write(msg);
        string x = Console.ReadLine().ToLower();
        while (x != "ya" && x != "tidak")
        {
            Console.Write("Input harus 'ya' atau 'tidak': ");
            x = Console.ReadLine().ToLower();
        }
        return x;
    }
}
