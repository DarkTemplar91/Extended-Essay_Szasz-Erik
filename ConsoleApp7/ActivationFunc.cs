using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp7
{
    //All the activation functions, error functions and their derivatives

    class ActivationFunc
    {
        //Math class contains TanH
        public static double Der_TanH(double value)
        {
            double res = 0;
            res = 1 - (value * value);
            return res;
        }

        public static double Sigmoid(double x)
        {
            double fin = Math.Exp(x) / (Math.Exp(x) + 1);
            return fin;
        }
        public static double Der_Sigmoid(double x)
        {
            double fin = Sigmoid(x) * (1 - Sigmoid(x));
            return fin;
        }
        
        public static int BinaryStep(double x)
        {
            if (x < 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        public static double[] Der_LogLoss(FeedForwardNetwork network, double[] input, double[] expected)
        {

            double[] fin = network.layers[network.NumberOfLayers - 1].inputs;

            double[] error = new double[fin.Length];
            for (int n = 0; n < fin.Length; n++)
            {
                error[n] = (fin[n] - expected[n]) / ((fin[n] + 1) * fin[n]);
            }
            return error;
        }
        public static double[] Der_Sqr(FeedForwardNetwork network, double[] input, double[] expected)
        {
            double[] fin = network.layers[network.NumberOfLayers].outputs;

            double[] error = new double[fin.Length];
            for (int n = 0; n < fin.Length; n++)
            {
                error[n] = -(expected[n] - fin[n]);
            }
            return error;
        }
        public static double Softplus(double x)
        {
            return Math.Log(1 + Math.Exp(x));
        }
        public static double ReLU(double x)
        {
            return Math.Max(0, x);
        }
        public static double Der_ReLU(double x)
        {
            if (x > 0)
            {
                return x;
            }
            else
                return 0;
        }
        public static double Swish(double x)
        {
            return x * Sigmoid(x);
        }
        public static double Der_Swish(double x)
        {
            double y = Swish(x);
            return y + Sigmoid(x) * (1 - y);
        }

    }
}
