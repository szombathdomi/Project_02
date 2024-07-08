using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tömegközlekedés_DCXHLN
{
    abstract class Graf<T>
    {
        //eseménykezelés delegált fügvényei illetve eseményei
        public delegate void EventHandler(double km);
        public event EventHandler ElkeszultazUtvonal;
        public delegate void EventHandler2(T x, T y);
        public event EventHandler2 HonnanHova;
        protected class El
        {
            public T hova;
            public double suly;
        }
        public abstract void UjCsucs(T tartalom);
        public abstract void UjEl(T honnan, T hova, double suly = 1);
        protected abstract T AdottIndexuCsucs(int index);
        protected abstract int AdottCsucsIndexe(T csucs);
        protected abstract int Csucsokszama { get; }
        protected abstract List<El> Szomszedok(T csucs);
        public List<T> Dijkstra(T start, T cel, ref double osszsuly)
        {
            double[] d = new double[Csucsokszama];
            T[] n = new T[Csucsokszama];
            List<T> S = new List<T>();
            for (int i = 0; i < Csucsokszama; i++)
            {
                T x = AdottIndexuCsucs(i);
                d[i] = double.PositiveInfinity;
                n[i] = default(T);
                S.Add(x);
            }
            d[AdottCsucsIndexe(start)] = 0;

            while (S.Count != 0)
            {
                T u = MinKivesz(S, d);
                foreach (El x in Szomszedok(u))
                {
                    int idx_x = AdottCsucsIndexe(x.hova);
                    int idx_u = AdottCsucsIndexe(u);

                    if (d[idx_u] + x.suly < d[idx_x])
                    {
                        d[idx_x] = d[idx_u] + x.suly;
                        n[idx_x] = u;
                    }
                }
            }

            //össz km - kész
            int celindex = AdottCsucsIndexe(cel);
            osszsuly = d[celindex];
            //Eventek
            ElkeszultazUtvonal(osszsuly);
            HonnanHova(start, cel);
            //megállók listája
            List<T> allomasok = new List<T>();
            while (!cel.Equals(start))
            {
                allomasok.Add(cel);
                cel = n[celindex];
                celindex = AdottCsucsIndexe(cel);
            }
            allomasok.Add(start);
            allomasok.Reverse();
            return allomasok;
        }

        private T MinKivesz(List<T> S, double[] d)
        {
            int minindex = 0;
            double min = double.PositiveInfinity;

            for (int i = 0; i < S.Count; i++)
            {
                int idx = AdottCsucsIndexe(S[i]);
                double sulyertek = d[idx];
                if (sulyertek < min)
                {
                    min = sulyertek;
                    minindex = i;
                }
            }
            T torlendo = S[minindex];
            S.RemoveAt(minindex);
            return torlendo;
        }
    }

    class GrafSzomszedsagiLista<T> : Graf<T>
    {
        List<T> tartalmak; // városokat tárolom
        List<List<El>> szomszedok; //szomszédokat tárolom

        public GrafSzomszedsagiLista()
        {
            tartalmak = new List<T>();
            szomszedok = new List<List<El>>();
        }

        protected override int Csucsokszama
        {
            get
            {
                return tartalmak.Count;
            }
        }

        public override void UjCsucs(T tartalom)
        {
            tartalmak.Add(tartalom);
            szomszedok.Add(new List<El>());
        }

        public override void UjEl(T honnan, T hova, double suly = 1)
        {
            int index = tartalmak.IndexOf(honnan); // tartalmak kozott hanyadik indexen van
            szomszedok[index].Add(new Graf<T>.El()
            {
                hova = hova,
                suly = suly
            });

            //előző folyamatnak megcsinálom a fordítottját is, mert irányítatlan a gráf
            index = tartalmak.IndexOf(hova);
            szomszedok[index].Add(new Graf<T>.El()
            {
                hova = honnan,
                suly = suly
            });
        }

        protected override int AdottCsucsIndexe(T csucs)
        {
            return tartalmak.IndexOf(csucs);
        }

        protected override T AdottIndexuCsucs(int index)
        {
            return tartalmak[index];
        }

        protected override List<El> Szomszedok(T csucs)
        {
            int index = tartalmak.IndexOf(csucs);
            return szomszedok[index];
        }
    }
}
