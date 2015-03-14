using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GravitationalClustering.Algorithms;
using System.IO;
using System.Diagnostics;

namespace GravitationalClustering
{
    public partial class Form1 : Form
    {
        public readonly double ScaleCluster = 7;
        public double ShiftCluster = 40;
        public readonly double ScaleRadius = 0.01;
        public Form1()
        {
            InitializeComponent();

            PredictOllivetiDataProbProto();
            PredictOllivetiDataSimProto();

            //PredictDigitsDataProbProto();
            //PredictDigitsDataSimProto();

            //PredictIrisDataProbProto();
            //PredictIrisDataSimProto();

            //PredictOllivetiDataProb();
            //PredictOllivetiDataSim();

            //PredictDigitsDataProb();
            //PredictDigitsDataSim();

            //PredictIrisDataProb();
            //PredictIrisDataSim();

        }


        public void DrawClusters(Algorithms.GravitationalClustering clust)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int x = i; x < 4; x++)
                {
                    if (i != x)
                    {
                        Graphics g = this.pictureBox1.CreateGraphics();
                        foreach (var item in clust.EnumeratePlanets())
                        {
                            this.DrawCircle(g, Pens.Red, new Point(
                                (int)(item.Position[i] * this.ScaleCluster + this.ShiftCluster),
                                (int)(item.Position[x] * this.ScaleCluster + 300)),
                                (int)((item.Radius * ScaleRadius) * this.ScaleCluster * this.ScaleCluster));
                        }
                        this.ShiftCluster += 100;
                    }
                }
            }
        }
        private void DrawCircle(Graphics drawingArea, Pen penToUse, Point center, int radius)
        {
            Rectangle rect = new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            drawingArea.DrawEllipse(penToUse, rect);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.DrawClusters(this.PredictIrisDataProb());
        }

        private Algorithms.GravitationalClustering PredictBreastCancerProb()
        {
            double[][] input = new double[569][];
            for (int x = 0; x < input.Length; x++)
            {
                input[x] = new double[32];
            }

            // The expected output for each test case.
            double[] output = new double[569];
            var data = File.ReadAllLines("wdbc.txt")
                .Select(x =>
                {
                    var left = x.Split(',').Skip(2).Select(y => double.Parse(y)).ToArray();
                    var right = x.Split(',').Skip(1).First().First() == 'M' ? 0 : 1;
                    return Tuple.Create(left, right);
                })
                .ToArray();

            Algorithms.GravitationalClustering clustering = new Algorithms.GravitationalClustering(50, 0.01, 100, new SquaredEuclideanDistance());

            return RunGCAlgorithm(ref data, clustering, true);
        }
        private Algorithms.GravitationalClustering PredictBreastCancerSim()
        {
            double[][] input = new double[569][];
            for (int x = 0; x < input.Length; x++)
            {
                input[x] = new double[32];
            }

            // The expected output for each test case.
            double[] output = new double[569];
            var data = File.ReadAllLines("wdbc.txt")
                .Select(x =>
                {
                    var left = x.Split(',').Skip(2).Select(y => double.Parse(y)).ToArray();
                    var right = x.Split(',').Skip(1).First().First() == 'M' ? 0 : 1;
                    return Tuple.Create(left, right);
                })
                .ToArray();

            Algorithms.GravitationalClustering clustering = new Algorithms.GravitationalClustering(5000, 0.01, 100, new SquaredEuclideanDistance());

            return RunGCAlgorithm(ref data, clustering, true);
        }

        private Algorithms.GravitationalClustering PredictIrisDataProb()
        {
            var data = File.ReadAllLines("iris.csv")
                .Skip(1)
                .Select(x =>
                {
                    var left = x.Split(',').Skip(1).Select(y => double.Parse(y)).ToArray();
                    var right = int.Parse(x.Split(',').Last());
                    return Tuple.Create(left, right);
                })
                .ToArray();

            Algorithms.GravitationalClustering clustering = new Algorithms.GravitationalClustering(1, 0.01, 100, new SquaredEuclideanDistance());

            return RunGCAlgorithm(ref data, clustering, false);
        }
        private Algorithms.GravitationalClustering PredictIrisDataSim()
        {
            var data = File.ReadAllLines("iris.csv")
                .Skip(1)
                .Select(x =>
                {
                    var left = x.Split(',').Skip(1).Select(y => double.Parse(y)).ToArray();
                    var right = int.Parse(x.Split(',').Last());
                    return Tuple.Create(left, right);
                })
                .ToArray();

            Algorithms.GravitationalClustering clustering = new Algorithms.GravitationalClustering(0.01, 0.1, 100, new SquaredEuclideanDistance());

            return RunGCAlgorithm(ref data, clustering, true);
        }

        private Algorithms.GravitationalClustering PredictDigitsDataProb()
        {
            var data = File.ReadAllLines("digits.csv")
                .Skip(1)
                .Select(x =>
                {
                    var left = x.Split(',').Skip(1).Select(y => double.Parse(y)).ToArray();
                    var right = int.Parse(x.Split(',').Last());
                    return Tuple.Create(left, right);
                })
                .ToArray();

            Algorithms.GravitationalClustering clustering = new Algorithms.GravitationalClustering(40, 0.01, 100, new SquaredEuclideanDistance());

            return RunGCAlgorithm(ref data, clustering, false, 50);
        }
        private Algorithms.GravitationalClustering PredictDigitsDataSim()
        {
            var data = File.ReadAllLines("digits.csv")
                .Skip(1)
                .Select(x =>
                {
                    var left = x.Split(',').Skip(1).Select(y => double.Parse(y)).ToArray();
                    var right = int.Parse(x.Split(',').Last());
                    return Tuple.Create(left, right);
                })
                .ToArray();

            Algorithms.GravitationalClustering clustering = new Algorithms.GravitationalClustering(80, 0.1, 100, new SquaredEuclideanDistance());

            return RunGCAlgorithm(ref data, clustering, true, 50);
        }

        private Algorithms.GravitationalClustering PredictOllivetiDataProb()
        {
            int index = 0;
            var pred = File.ReadAllLines("olevitti_p.csv");
            var data = File.ReadAllLines("olevitti.csv")
                .Select(x =>
                {
                    var left = x.Split(',').Skip(1).Select(y => double.Parse(y)).ToArray();
                    return Tuple.Create(left, (int)double.Parse(pred[index++]));
                })
                .ToArray();

            Algorithms.GravitationalClustering clustering = new Algorithms.GravitationalClustering(2, 0.01, 100, new SquaredEuclideanDistance());

            return RunGCAlgorithm(ref data, clustering, false, 50);
        }
        private Algorithms.GravitationalClustering PredictOllivetiDataSim()
        {
            int index = 0;
            var pred = File.ReadAllLines("olevitti_p.csv");
            var data = File.ReadAllLines("olevitti.csv")
                .Select(x =>
                {
                    var left = x.Split(',').Skip(1).Select(y => double.Parse(y)).ToArray();
                    return Tuple.Create(left, (int)double.Parse(pred[index++]));
                })
                .ToArray();

            Algorithms.GravitationalClustering clustering = new Algorithms.GravitationalClustering(10, 0.1, 1, new SquaredEuclideanDistance());

            return RunGCAlgorithm(ref data, clustering, true, 4);
        }



        private Algorithms.GravitationalClustering PredictBreastCancerProbProto()
        {
            double[][] input = new double[569][];
            for (int x = 0; x < input.Length; x++)
            {
                input[x] = new double[32];
            }

            double[] output = new double[569];
            var data = File.ReadAllLines("wdbc.txt")
                .Select(x =>
                {
                    var left = x.Split(',').Skip(2).Select(y => double.Parse(y)).ToArray();
                    var right = x.Split(',').Skip(1).First().First() == 'M' ? 0 : 1;
                    return Tuple.Create(left, right);
                })
                .ToArray();

            Algorithms.GravitationalClustering clustering = new Algorithms.GravitationalClustering(50, 0.01, 100, new SquaredEuclideanDistance());

            return RunProtoGCAlgoritym(ref data, clustering, false);
        }
        private Algorithms.GravitationalClustering PredictBreastCancerSimProto()
        {
            double[][] input = new double[569][];
            for (int x = 0; x < input.Length; x++)
            {
                input[x] = new double[32];
            }

            double[] output = new double[569];
            var data = File.ReadAllLines("wdbc.txt")
                .Select(x =>
                {
                    var left = x.Split(',').Skip(2).Select(y => double.Parse(y)).ToArray();
                    var right = x.Split(',').Skip(1).First().First() == 'M' ? 0 : 1;
                    return Tuple.Create(left, right);
                })
                .ToArray();

            Algorithms.GravitationalClustering clustering = new Algorithms.GravitationalClustering(5000, 0.01, 100, new SquaredEuclideanDistance());

            return RunProtoGCAlgoritym(ref data, clustering, true);
        }

        private Algorithms.GravitationalClustering PredictIrisDataProbProto()
        {
            var data = File.ReadAllLines("iris.csv")
                .Skip(1)
                .Select(x =>
                {
                    var left = x.Split(',').Skip(1).Select(y => double.Parse(y)).ToArray();
                    var right = int.Parse(x.Split(',').Last());
                    return Tuple.Create(left, right);
                })
                .ToArray();

            Algorithms.GravitationalClustering clustering = new Algorithms.GravitationalClustering(1, 0.01, 100, new SquaredEuclideanDistance());

            return RunProtoGCAlgoritym(ref data, clustering, false);
        }
        private Algorithms.GravitationalClustering PredictIrisDataSimProto()
        {
            var data = File.ReadAllLines("iris.csv")
                .Skip(1)
                .Select(x =>
                {
                    var left = x.Split(',').Skip(1).Select(y => double.Parse(y)).ToArray();
                    var right = int.Parse(x.Split(',').Last());
                    return Tuple.Create(left, right);
                })
                .ToArray();

            Algorithms.GravitationalClustering clustering = new Algorithms.GravitationalClustering(0.01, 0.1, 100, new SquaredEuclideanDistance());
            return RunProtoGCAlgoritym(ref data, clustering, true);

        }

        private Algorithms.GravitationalClustering PredictDigitsDataProbProto()
        {
            var data = File.ReadAllLines("digits.csv")
                .Skip(1)
                .Select(x =>
                {
                    var left = x.Split(',').Skip(1).Select(y => double.Parse(y)).ToArray();
                    var right = int.Parse(x.Split(',').Last());
                    return Tuple.Create(left, right);
                })
                .ToArray();

            Algorithms.GravitationalClustering clustering = new Algorithms.GravitationalClustering(40, 0.01, 100, new SquaredEuclideanDistance());

            return RunProtoGCAlgoritym(ref data, clustering, false);
        }
        private Algorithms.GravitationalClustering PredictDigitsDataSimProto()
        {
            var data = File.ReadAllLines("digits.csv")
                .Skip(1)
                .Select(x =>
                {
                    var left = x.Split(',').Skip(1).Select(y => double.Parse(y)).ToArray();
                    var right = int.Parse(x.Split(',').Last());
                    return Tuple.Create(left, right);
                })
                .ToArray();

            Algorithms.GravitationalClustering clustering = new Algorithms.GravitationalClustering(80, 0.1, 100, new SquaredEuclideanDistance());

            return RunProtoGCAlgoritym(ref data, clustering, true);
        }

        private Algorithms.GravitationalClustering PredictOllivetiDataProbProto()
        {
            int index = 0;
            var pred = File.ReadAllLines("olevitti_p.csv");
            var data = File.ReadAllLines("olevitti.csv")
                .Select(x =>
                {
                    var left = x.Split(',').Skip(1).Select(y => double.Parse(y)).ToArray();
                    return Tuple.Create(left, (int)double.Parse(pred[index++]));
                })
                .ToArray();

            Algorithms.GravitationalClustering clustering = new Algorithms.GravitationalClustering(2, 0.01, 100, new SquaredEuclideanDistance());

            return RunProtoGCAlgoritym(ref data, clustering, false);
        }
        private Algorithms.GravitationalClustering PredictOllivetiDataSimProto()
        {
            int index = 0;
            var pred = File.ReadAllLines("olevitti_p.csv");
            var data = File.ReadAllLines("olevitti.csv")
                .Select(x =>
                {
                    var left = x.Split(',').Skip(1).Select(y => double.Parse(y)).ToArray();
                    return Tuple.Create(left, (int)double.Parse(pred[index++]));
                })
                .ToArray();

            Algorithms.GravitationalClustering clustering = new Algorithms.GravitationalClustering(10, 0.1, 1, new SquaredEuclideanDistance());

            return RunProtoGCAlgoritym(ref data, clustering, true);
        }

        private Algorithms.GravitationalClustering RunGCAlgorithm(ref Tuple<double[], int>[] data, Algorithms.GravitationalClustering clustering, bool sim, int rand = 4)
        {
            RandomizeArray(ref data, rand);

            int take = data.Length / 2;

            foreach (var item in data.Take(take))
            {
                clustering.OnlineTrainSingular(item.Item1, item.Item2);
            }
            Debug.WriteLine("Number of Clusters " + clustering.EnumeratePlanets().Count());
            var d = data.Skip(take).Select(xx =>
            {
                int outP = 0;
                if (sim)
                {
                    outP = clustering.PredictSimulation(xx.Item1);
                }
                else
                {
                    outP = clustering.PredictProbabilistic(xx.Item1);
                }

                Debug.WriteLine("Predicted Class " + outP);
                if (outP == xx.Item2)
                {
                    return 1;
                }
                return 0;
            }).Sum();
#if DEBUG
            {
                Debug.WriteLine("Percent Accuracy " + ((double)d / ((double)data.Length - take)).ToString());
            }
#else
            {
                MessageBox.Show(((double)d / ((double)data.Length - take)).ToString());
            }
#endif
            return clustering;
        }
        private Algorithms.GravitationalClustering RunProtoGCAlgoritym(ref Tuple<double[], int>[] data, Algorithms.GravitationalClustering clustering, bool sim)
        {
            var newData = this.GetProto(data);
            foreach (var item in newData)
            {
                clustering.OnlineTrainSingular(item.Item1, item.Item2);
            }
            Debug.WriteLine("Number of Clusters " + clustering.EnumeratePlanets().Count());
            var d = data.Select(xx =>
            {
                int outP = 0;
                if (sim)
                {
                    outP = clustering.PredictSimulation(xx.Item1);
                }
                else
                {
                    outP = clustering.PredictProbabilistic(xx.Item1);
                }

                Debug.WriteLine("Predicted Class " + outP);
                if (outP == xx.Item2)
                {
                    return 1;
                }
                return 0;
            }).Sum();
#if DEBUG
            {
                Debug.WriteLine("Percent Accuracy " + ((double)d / ((double)data.Length - take)).ToString());
            }
#else
            {
                MessageBox.Show(((double)d / ((double)data.Length)).ToString());
            }
#endif
            return clustering;
        }
        private void RandomizeArray<T>(ref T[] left, int rand)
        {
            Random rnd = new Random(rand);
            int[] randar = Enumerable.Range(0, left.Length).OrderBy(x => rnd.Next()).ToArray();
            List<T> lefta = new List<T>();
            foreach (var item in randar)
            {
                lefta.Add(left[item]);
            }
            left = lefta.ToArray();
        }
        private List<Tuple<Double[], int>> GetProto(Tuple<double[], int>[] data)
        {
            Dictionary<int, Tuple<Double[], int>> dict = new Dictionary<int, Tuple<double[], int>>();

            foreach (var item in data)
            {
                if (!dict.ContainsKey(item.Item2))
                {
                    dict.Add(item.Item2, item);
                }
            }
            return dict.Select(x => x.Value).ToList();
        }
    }
}
