using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oldtimer
{
    internal class Auto
    {

        string rendszam;
        string szin;
        string nev;
        int evjarat;
        int ar;
        string kategoriaNev;

        public Auto(string rendszam, string szin, string nev, int evjarat, int ar, string kategoriaNev)
        {
            this.Rendszam = rendszam;
            this.Szin = szin;
            this.Nev = nev;
            this.Evjarat = evjarat;
            this.Ar = ar;
            this.KategoriaNev = kategoriaNev;
        }

        public string Rendszam { get => rendszam; set => rendszam = value; }
        public string Szin { get => szin; set => szin = value; }
        public string Nev { get => nev; set => nev = value; }
        public int Evjarat { get => evjarat; set => evjarat = value; }
        public int Ar { get => ar; set => ar = value; }
        public string KategoriaNev { get => kategoriaNev; set => kategoriaNev = value; }
    }
}
