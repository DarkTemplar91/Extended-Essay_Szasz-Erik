using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Tensorflow.Binding;


namespace ConsoleApp7
{
    class Player
    {
        public string season;

        public string Name;
        public double Age;
        public double WS;
        public double G;
        public double GS;
        public double MP;
        public double FG;
        public double FGA;
        public double _2P;
        public double _2PA;
        public double _3P;
        public double _3PA;
        public double FT;
        public double FTA;
        public double TRB;
        public double AST;
        public double STL;
        public double BLK;
        public double TOV;
        public double PF;
        public double PTS;
        public double MVP;
        public double RoTY;
        public double DPoTY;
        public double SMoTY;
        public double MIP;

        public double PredScore;
    }
    
    class Program
    {

        static public List<Player> LoadAll(string[] exception)
        {
            List<Player> Players = new List<Player>();
            StreamReader StreamReader = new StreamReader(Environment.CurrentDirectory + "/MVP/all.csv");

            string line;
            while ((line = StreamReader.ReadLine()) != null)
            {
                string[] array = line.Split(';');
                try
                {
                    if (exception.Contains(Convert.ToString(array[1])) == false)
                    {

                        Players.Add(new Player
                        {

                            Name = array[0],
                            season = array[1],
                            Age = Convert.ToDouble(array[2]),
                            G = Convert.ToDouble(array[4]),
                            WS = Convert.ToDouble(array[5].Replace('.', ',')),
                            GS = Convert.ToDouble(array[6]),
                            MP = Convert.ToDouble(array[7].Replace('.', ',')),
                            FG = Convert.ToDouble(array[8].Replace('.', ',')),
                            FGA = Convert.ToDouble(array[9].Replace('.', ',')),
                            _2P = Convert.ToDouble(array[10].Replace('.', ',')),
                            _2PA = Convert.ToDouble(array[11].Replace('.', ',')),
                            _3P = Convert.ToDouble(array[12].Replace('.', ',')),
                            _3PA = Convert.ToDouble(array[13].Replace('.', ',')),
                            FT = Convert.ToDouble(array[14].Replace('.', ',')),
                            FTA = Convert.ToDouble(array[15].Replace('.', ',')),
                            TRB = Convert.ToDouble(array[16].Replace('.', ',')),
                            AST = Convert.ToDouble(array[17].Replace('.', ',')),
                            STL = Convert.ToDouble(array[18].Replace('.', ',')),
                            BLK = Convert.ToDouble(array[19].Replace('.', ',')),
                            TOV = Convert.ToDouble(array[20].Replace('.', ',')),
                            PF = Convert.ToDouble(array[21].Replace('.', ',')),
                            PTS = Convert.ToDouble(array[22].Replace('.', ',')),
                            MVP = Convert.ToDouble(array[23].Replace('.', ',')),
                            RoTY = Convert.ToDouble(array[24]),
                            DPoTY = Convert.ToDouble(array[25]),
                            SMoTY = Convert.ToDouble(array[26]),
                            MIP = Convert.ToDouble(array[27])

                        });
                    }
                }
                catch (Exception e) { continue; }

            }
            StreamReader.Close();
            return Players;
        }
        static public List<Player> Load(string year)
        {
            List<Player> Players = new List<Player>();
            StreamReader StreamReader = new StreamReader(Environment.CurrentDirectory + "/MVP/" + year + ".csv");
           
            string line;
            while ((line = StreamReader.ReadLine()) != null)
            {
                string[] array = line.Split(';');
                try
                {


                    Players.Add(new Player
                    {

                        Name = array[0],
                        season = array[1],
                        Age = Convert.ToDouble(array[2]),
                        G = Convert.ToDouble(array[4]),
                        WS = Convert.ToDouble(array[5].Replace('.', ',')),
                        GS = Convert.ToDouble(array[6]),
                        MP = Convert.ToDouble(array[7].Replace('.', ',')),
                        FG = Convert.ToDouble(array[8].Replace('.', ',')),
                        FGA = Convert.ToDouble(array[9].Replace('.', ',')),
                        _2P = Convert.ToDouble(array[10].Replace('.', ',')),
                        _2PA = Convert.ToDouble(array[11].Replace('.', ',')),
                        _3P = Convert.ToDouble(array[12].Replace('.', ',')),
                        _3PA = Convert.ToDouble(array[13].Replace('.', ',')),
                        FT = Convert.ToDouble(array[14].Replace('.', ',')),
                        FTA = Convert.ToDouble(array[15].Replace('.', ',')),
                        TRB = Convert.ToDouble(array[16].Replace('.', ',')),
                        AST = Convert.ToDouble(array[17].Replace('.', ',')),
                        STL = Convert.ToDouble(array[18].Replace('.', ',')),
                        BLK = Convert.ToDouble(array[19].Replace('.', ',')),
                        TOV = Convert.ToDouble(array[20].Replace('.', ',')),
                        PF = Convert.ToDouble(array[21].Replace('.', ',')),
                        PTS = Convert.ToDouble(array[22].Replace('.', ',')),
                        MVP = Convert.ToDouble(array[23].Replace('.', ',')),
                        RoTY = Convert.ToDouble(array[24]),
                        DPoTY = Convert.ToDouble(array[25]),
                        SMoTY = Convert.ToDouble(array[26]),
                        MIP = Convert.ToDouble(array[27])

                    });
                }
                catch (Exception e) { continue; }
                
            }
            StreamReader.Close();
            return Players;
        }
        
        static public void TrainNetwork(FeedForwardNetwork network, List<Player> datasets, double lRate, double lambda, int batch)
        {
            for (int n = 0; n < batch; n++)
            {
                for (int p = 0; p < datasets.Count; p++)
                {
                    network.Train(new double[] {datasets[p].Age, datasets[p].G, datasets[p].WS, datasets[p].GS,
                    datasets[p].MP,datasets[p].FG,datasets[p].FGA,datasets[p]._2P,datasets[p]._2PA,
                    datasets[p]._3P,datasets[p]._3PA,datasets[p].FT,datasets[p].FT,datasets[p].TRB,
                    datasets[p].AST,datasets[p].STL,datasets[p].BLK,datasets[p].TOV,datasets[p].PF,datasets[p].PTS,},
                    new double[] { datasets[p].MVP }, lRate, lambda);
                }
            }
            SaveWeights(network);
        }

        static void SaveWeights(FeedForwardNetwork network)
        {
            bool NaN = false;
            for (int n = 0; n < network.NumberOfLayers - 1; n++)
            {
                StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + "/Weights/l" + n + ".csv");
                for (int r = 0; r < network.layers[n].weight.Rows; r++)
                {
                    for (int c = 0; c < network.layers[n].weight.Col; c++)
                    {

                        if (double.IsNaN(network.layers[n].weight[r, c]))
                        {
                            NaN = true;
                            throw new Exception("Weight matrice contain NaN");
                        }
                        sw.Write("{0}; ", network.layers[n].weight[r, c]);
                    }
                    if (NaN == true)
                    {
                        break;
                    }
                    sw.Write("\b\b \n");

                }
                sw.Close();
            }
        }
        static void LoadWeights(FeedForwardNetwork network)
        {
            for (int n = 0; n < network.NumberOfLayers - 1; n++)
            {
                StreamReader sr = new StreamReader(Environment.CurrentDirectory + "/Weights/l" + n + ".csv");
                for (int r = 0; r < network.layers[n].Weight().Rows; r++)
                {
                    string line = sr.ReadLine();
                    string[] array = line.Split(';');
                    for (int c = 0; c < network.layers[n].Weight().Col; c++)
                    {
                        network.layers[n].weight[r, c] = Convert.ToDouble(array[c]);
                    }

                }
                sr.Close();
            }
        }
        
        static void TestSeason(FeedForwardNetwork network, List<Player> datasets)
        {
           for (int p = 0; p < datasets.Count; p++)
            {
                datasets[p].PredScore=network.ComputeOutput(new double[] {datasets[p].Age, datasets[p].G, datasets[p].WS, datasets[p].GS,
                    datasets[p].MP,datasets[p].FG,datasets[p].FGA,datasets[p]._2P,datasets[p]._2PA,
                    datasets[p]._3P,datasets[p]._3PA,datasets[p].FT,datasets[p].FT,datasets[p].TRB,
                    datasets[p].AST,datasets[p].STL,datasets[p].BLK,datasets[p].TOV,datasets[p].PF,datasets[p].PTS })[0];
                
            }
            datasets=datasets.OrderByDescending(x => x.PredScore).ToList();
            for (int n = 0; n < datasets.Count; n++)
            {
                Console.WriteLine((n + 1) + " " + datasets[n].Name+" "+datasets[n].PredScore+" "+datasets[n].MVP) ;
            }
            SaveTop50(datasets);
        }
        static void SaveTop50(List<Player> datasets)
        {
            StreamWriter writer = new StreamWriter("TOP50.txt");

            
            for (int n = 0; n < 50; n++)
            {
                string a = Convert.ToString(n+1)+";"+datasets[n].Name + ";" + datasets[n].PredScore + ";" + datasets[n].MVP;
                writer.WriteLine(a);
            }
        }
        private static Random rng = new Random();

        public static void Shuffle(List<Player> list)
        {
            list.OrderBy(a => rng.Next());
        }
        public static int[] except()
        {
            int[] a = new int[7];
            Random rnd = new Random();
            for (int n = 0; n < 7; n++)
            {
                int y = rnd.Next(1981, 2019);
                if (a.Contains(y))
                {
                    n--;
                    continue;
                }
                a[n] = y;
            }
            return a;
        }
        static List<Player> CreateTrainingSet(List<Player> players)
        {

            List<Player> training = new List<Player>();
            players.OrderByDescending(x => x.MVP);
            
            for(int n = 0; n < players.Count; n++)
            {
                if (players[n].MVP == 1)
                {
                    training.Add(players[n]);
                }
            }
            Random rnd = new Random();
            for (int i = 0; i < 100; i++)
            {
                int a = rnd.Next(0,players.Count-1);
                if (training.Contains(players[a]) == false)
                {
                    training.Add(players[a]);
                }
                else
                {
                    i--;
                }
            }
            Shuffle(training);
            return training;
        }
        static void Main(string[] args)
        {            
            FeedForwardNetwork network = new FeedForwardNetwork("Log");
            network.AddLayer(new FeedForwardLayer(20, 20, "TanH"));
            network.AddLayer(new FeedForwardLayer(20, 1, "Sigmoid"));
            network.AddLayer(new FeedForwardLayer(1));

            int[] exceptions = { 2018 };

            List<Player> datasets = LoadAll(new string[] {"2018-19" });
            datasets = CreateTrainingSet(datasets);
            Shuffle(datasets);
            TrainNetwork(network,datasets, 0.00001, 0, 500);
            TestSeason(network, Load("2018-19"));
            Console.WriteLine("Done");
            Console.ReadLine();
        }
        
    }
}
