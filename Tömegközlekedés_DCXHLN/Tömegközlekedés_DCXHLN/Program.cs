using System;
using System.Collections.Generic;
using System.IO;

namespace Tömegközlekedés_DCXHLN
{
    class Program
    {
        static void Kiirattavolsag(double km)
        {
            Console.WriteLine("-Távolság: " + km + "km");
        }
        static void Kiirathonnanhova(string honnan, string hova)
        {
            Console.WriteLine("-{0} -> {1} útvonalterv:", honnan, hova);
        }
        //3 fileból beolvasás (járművek, városok, városok közötti kapcsolat)
        #region BEOLVASÁS
        static List<Jarmu> JarmuBeolvasas(string vehicles)
        {
            string[] sorok = File.ReadAllLines(vehicles);
            List<Jarmu> jarmuvek = new List<Jarmu>();
            for (int i = 0; i < sorok.Length; i++)
            {
                string[] egysor = sorok[i].Split(" ");
                string jarmunev = egysor[0];
                int hatotav = int.Parse(egysor[1]);
                int uzemanyagszint = int.Parse(egysor[2]);

                if (jarmunev == "Auto")
                {
                    jarmuvek.Add(new Auto(jarmunev, hatotav, uzemanyagszint));
                }
                else if (jarmunev == "Busz")
                {
                    jarmuvek.Add(new Busz(jarmunev, hatotav, uzemanyagszint));
                }
                else if (jarmunev == "Vonat")
                {
                    jarmuvek.Add(new Vonat(jarmunev, hatotav, uzemanyagszint));
                }
            }
            return jarmuvek;
        }
        static void TelepulesBeolvasas(string varosok, Graf<string> graf)
        {
            string[] sorok = File.ReadAllLines(varosok);
            for (int i = 0; i < sorok.Length; i++)
            {
                graf.UjCsucs(sorok[i]);
            }
        }
        static void ElBeolvasas(string kapcsolatok, Graf<string> graf)
        {
            string[] sorok = File.ReadAllLines(kapcsolatok);
            for (int i = 0; i < sorok.Length; i++)
            {
                string[] egysor = sorok[i].Split(" ");
                graf.UjEl(egysor[0], egysor[1], int.Parse(egysor[2]));
            }
        }
        #endregion
        static void Main(string[] args)
        {
            List<Jarmu> jarmuvek = JarmuBeolvasas("jarmuvek.txt");
            Graf<string> graf = new GrafSzomszedsagiLista<string>();
            graf.ElkeszultazUtvonal += Kiirattavolsag;
            graf.HonnanHova += Kiirathonnanhova;
            TelepulesBeolvasas("telepulesek.txt", graf);
            ElBeolvasas("elek.txt", graf);

            Console.Write("Kiindulási település: "); //bekérem a kiindulási pontot
            string honnan = Console.ReadLine();

            Console.Write("Úti cél: "); //bekérem az érkezési pontot
            string hova = Console.ReadLine();

            string[] varosok = File.ReadAllLines("telepulesek.txt");
            bool letezikIndulas = false;
            bool LetezikCel = false;

            //2 ciklussal külön-külön megvizsgálom, hogy a 2 input valid 
            foreach (string v in varosok)
            {
                if (v == honnan)
                {
                    letezikIndulas = true;
                }
            }
            foreach (string v1 in varosok)
            {
                if (v1 == hova)
                {
                    LetezikCel = true;
                }
            }
            try
            {
                if (letezikIndulas == true && LetezikCel == true) // csak akkor futtatom a programot ha valid értékek az inputok
                {
                    #region program
                    double km = 0;
                    List<string> allomasok = graf.Dijkstra(honnan, hova, ref km); //Dijkstra algoritmus futtatása
                    Console.WriteLine("-Megközelíthető az alábbi városokat érintve:");
                    Console.WriteLine("\t" + string.Join("-->", allomasok)); //kiírom az érintett településeket "-->" elválasztással

                    List<double> fogyasztások = new List<double>(); // a kiszámolt fogyasztásokat egy listában tárolom el
                    for (int i = 0; i < jarmuvek.Count; i++)
                    {
                        Console.WriteLine("Ha {0} a jármű akkor:", jarmuvek[i].JárműNév);
                        try
                        {
                            if ((jarmuvek[i].Hatotav - km) >= 0) //ha az adott jármű hatótávja(km) nem kevesebb az út hosszánál 
                            {
                                double felhasznaltFuel = 0; //fogyasztás
                                if (jarmuvek[i] is Auto) //Külön számolom a fogyasztásokat a 3 járműre
                                {
                                    felhasznaltFuel = jarmuvek[i].ÜzemanyagSzint - (allomasok.Count * (km * 0.08)); //adott jármű üzemenyagkapacitásából(liter) kivonom az állomások számával megszorzott km/liter fogyasztást
                                    if (felhasznaltFuel > 0)
                                    {
                                        fogyasztások.Add(jarmuvek[i].ÜzemanyagSzint - Math.Ceiling(felhasznaltFuel));
                                        Console.WriteLine("\tA(z) {0} járművel megtehető az út, a táv során a tank {1}%-a fogyott el ({2} liter a {3} literből)", jarmuvek[i].JárműNév, 100 - Math.Ceiling(felhasznaltFuel * 100 / jarmuvek[i].ÜzemanyagSzint), jarmuvek[i].ÜzemanyagSzint - Math.Ceiling(felhasznaltFuel), jarmuvek[i].ÜzemanyagSzint);
                                    }
                                    else //kivételt dobok, ha az üzemanyagszint negatív értére jönne ki, ilyenkor üres a tank
                                    {
                                        throw new UresUzemanyagException();
                                    }
                                }
                                else if (jarmuvek[i] is Busz)
                                {
                                    felhasznaltFuel = jarmuvek[i].ÜzemanyagSzint - (allomasok.Count * (km * 0.35)); //0.35 l/km azert, mert 35l/100km egy regebbi busz fogyasztása 
                                    if (felhasznaltFuel > 0)
                                    {
                                        fogyasztások.Add(jarmuvek[i].ÜzemanyagSzint - Math.Ceiling(felhasznaltFuel));
                                        Console.WriteLine("\tA(z) {0} járművel megtehető az út, a táv során a tank {1}%-a fogyott el ({2} liter a {3} literből)", jarmuvek[i].JárműNév, 100 - Math.Ceiling(felhasznaltFuel * 100 / jarmuvek[i].ÜzemanyagSzint), jarmuvek[i].ÜzemanyagSzint - Math.Ceiling(felhasznaltFuel), jarmuvek[i].ÜzemanyagSzint);
                                    }
                                    else //kivételt dobok, ha az üzemanyagszint negatív értére jönne ki, ilyenkor üres a tank
                                    {
                                        throw new UresUzemanyagException();
                                    }
                                }
                                else if (jarmuvek[i] is Vonat)
                                {
                                    felhasznaltFuel = jarmuvek[i].ÜzemanyagSzint - (allomasok.Count * (km * 0.5));
                                    if (felhasznaltFuel > 0)
                                    {
                                        fogyasztások.Add(jarmuvek[i].ÜzemanyagSzint - Math.Ceiling(felhasznaltFuel));
                                        Console.WriteLine("\tA(z) {0} járművel megtehető az út, a táv során a tank {1}%-a fogyott el ({2} liter a {3} literből)", jarmuvek[i].JárműNév, 100 - Math.Ceiling(felhasznaltFuel * 100 / jarmuvek[i].ÜzemanyagSzint), jarmuvek[i].ÜzemanyagSzint - Math.Ceiling(felhasznaltFuel), jarmuvek[i].ÜzemanyagSzint);
                                    }
                                    else //kivételt dobok, ha az üzemanyagszint negatív értére jönne ki, ilyenkor üres a tank
                                    {
                                        throw new UresUzemanyagException();
                                    }
                                }
                            }
                            else // kivételt dobok, ha az út hossza meghaladja a jármű hatótávját
                            {
                                throw new HatotavException();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    fogyasztások.Sort(); // rendezem a listát növekvő sorrendbe, így az első elem a legkevesebb üzemanyagot felhasználó jármű lesz
                    Console.WriteLine("Érdemes lenne a {0} litert fogyasztó járművel utazni.", fogyasztások[0]);
                    #endregion
                }
                //külön-külön kezelem az invalid input kivételeket
                else if (LetezikCel == true && letezikIndulas == false)
                {
                    throw new KiindulasException();
                }
                else if (letezikIndulas == true && LetezikCel == false)
                {
                    throw new CelException();
                }
                else
                {
                    throw new TelepulesException();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}