using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitationalClustering.Algorithms
{
    public class GravitationalClustering
    {
        public readonly double Radius;
        public readonly double PercentStep;

        public readonly int Iterations;

        public readonly IDistance Distance;

        private List<Planet> planets;

        public GravitationalClustering(double radius, double step, int iterations, IDistance func)
        {
            Trace.Assert(radius > 0);
            Trace.Assert(step > 0);
            Trace.Assert(iterations >= 1);

            Trace.Assert(func != null);

            this.Iterations = iterations;
            this.Radius = radius;
            this.PercentStep = step;

            this.Distance = func;

            this.planets = new List<Planet>(15);

        }

        public void OnlineTrainSingular(double[] pos, int @class, double mass = 1)
        {
            var nearPlanets = this.FindPlanetsInRadius(pos)
                .Where(x => this.planets[x.Item1].Class == @class)
                .Select(x => Tuple.Create(x.Item1, mass * this.planets[x.Item1].Mass / Math.Pow(x.Item2, 2)))
                .ToList();

            if (nearPlanets.Count == 0)
            {
                this.planets.Add(new Planet(pos, mass, mass * this.Radius, @class));
                return;
            }
            var addPlanet = nearPlanets.OrderBy(x => x.Item2).First();

            this.planets[addPlanet.Item1].AddPoint(pos, mass);
        }

        public int PredictSimulation(double[] pos)
        {
            if (this.planets.Count == 0)
            {
                throw new ArgumentException("Train before predicting");
            }

            for (int i = 0; i < this.Iterations; i++)
            {
                var forcerec = this.GetDirectionalForceVector(pos);
                var inverse = this.Magnitude(forcerec);
                inverse = 1 / inverse;
                var magforcevec = this.Multiply(forcerec, inverse);
                pos = this.Add(this.Multiply(magforcevec, this.PercentStep), pos);
                Debug.WriteLine(i);
                Debug.WriteLine(pos.Select(x => x.ToString()).Aggregate((m, n) => m.ToString() + "," + n.ToString()));
            }
            var planets = this.FindPlanetsInRadius(pos);
            if (planets.Count != 0)
            {
                int mode = planets.GroupBy(v => this.planets[v.Item1].Class)
                            .OrderByDescending(g => g.Count())
                            .First()
                            .Key;
                return mode;
            }
            var min = double.MaxValue;
            var index = -1;

            for (int i = 0; i < this.planets.Count; i++)
            {
                double cur = this.Distance.Compute(pos, this.planets[i].Position);
                if (min > cur)
                {
                    min = cur;
                    index = i;
                }
            }
            if (index == -1)
            {
                return this.planets.First().Class;
            }
            return this.planets[index].Class;
        }
        public int PredictProbabilistic(double[] pos)
        {
            var grouped = this.planets.GroupBy(v => v.Class).ToList();
            List<Tuple<int, double>> prob = new List<Tuple<int, double>>(grouped.Count);
            foreach (var item in grouped)
            {
                double d = 1;
                foreach (var planet in item)
                {
                    d *= GaussianTools.CalculateRelativePDF(Math.Sqrt(this.Distance.Compute(planet.Position, pos)) / planet.Mass, Math.Pow(planet.Radius, 2));
                }
                if (d == 0)
                {
                    prob.Add(Tuple.Create(item.First().Class, 0.0));
                    continue;
                }
                prob.Add(Tuple.Create(item.First().Class, Math.Log(d) / item.Count()));
            }
            var val = this.Max(prob);
            Debug.WriteLine(val);
            return val.Item1;
        }

        public IEnumerable<Planet> EnumeratePlanets()
        {
            foreach (var item in this.planets)
            {
                yield return item;
            }
        }

        private double[] GetDirectionalForceVector(double[] pos)
        {
            double[] directionalForceVector = new double[pos.Length];

            for (int i = 0; i < this.planets.Count; i++)
            {
                var dist = this.Distance.Compute(pos, this.planets[i].Position);
                if (dist == 0)
                {
                    return this.Subtract(this.planets[i].Position, pos);
                }
                var subtract = this.Subtract(this.planets[i].Position, pos);
                var magni = (this.planets[i].Mass / (dist * dist));// this.Magnitude(subtract);
                directionalForceVector = this.Add(directionalForceVector, this.Multiply(subtract, magni));
            }
            return (directionalForceVector);
        }
        private List<Tuple<int, double>> FindPlanetsInRadius(double[] pos)
        {
            List<Tuple<int, double>> inrad = new List<Tuple<int, double>>(15);
            for (int i = 0; i < this.planets.Count; i++)
            {
                double cur = this.Distance.Compute(pos, this.planets[i].Position);
                if (this.planets[i].Radius >= cur)
                {
                    inrad.Add(Tuple.Create(i, cur));
                }
            }
            return inrad;
        }

        private double[] Subtract(double[] left, double[] right)
        {
            double[] sub = new double[left.Length];
            for (int i = 0; i < left.Length; i++)
            {
                sub[i] = left[i] - right[i];
            }
            return sub;
        }
        private double[] Add(double[] left, double[] right)
        {
            double[] add = new double[left.Length];
            for (int i = 0; i < left.Length; i++)
            {
                add[i] = left[i] + right[i];
            }
            return add;

        }
        private double[] Multiply(double[] left, double scalar)
        {
            double[] add = new double[left.Length];
            for (int i = 0; i < left.Length; i++)
            {
                add[i] = left[i] * scalar;
            }
            return add;
        }
        private double Magnitude(double[] l)
        {
            return Math.Sqrt(l.Sum(x => x * x));
        }

        private Tuple<int, double> Max(List<Tuple<int, double>> l)
        {
            double max = double.MinValue;
            Tuple<int, double> a = null;
            foreach (var item in l)
            {
                if (item.Item2 > max)
                {
                    max = item.Item2;
                    a = item;
                }
            }
            return a;
        }
        public class Planet
        {
            public double[] Position { get; private set; }
            public double Mass { get; private set; }
            public double Radius { get; private set; }

            public readonly int Class;

            private readonly double ratio;

            public Planet(double[] position, double mass, double radius, int classb)
            {
                this.Position = position;
                this.Mass = mass;
                this.Radius = radius;
                this.Class = classb;
                this.ratio = this.Radius / this.Mass;
            }
            public void AddPoint(double[] pos, double mass)
            {
                double summedMass = this.Mass + mass;

                double leftVal = this.Mass / summedMass;
                double rightVal = mass / summedMass;

                for (int i = 0; i < this.Position.Length; i++)
                {
                    this.Position[i] = this.Position[i] * leftVal + pos[i] * rightVal;
                }
                this.Radius = this.ratio * summedMass;
                this.Mass = summedMass;
            }
        }
    }
}
