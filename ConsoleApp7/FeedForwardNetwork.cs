using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp7
{
    class FeedForwardNetwork
    {
        private bool firstIteration = true;
        int layernumber;
        public int NumberOfLayers => layernumber;
        internal List<FeedForwardLayer> layers = new List<FeedForwardLayer>(); //A list containing the network's layers.
        public double[] output;
        double totalError;
        double[] deltas; 
        double[][] values; //holds the values of the neurons
        string ErrorFunc;

        public FeedForwardNetwork(string ErrorFunc)
        {
            this.ErrorFunc = ErrorFunc;
        }
        public FeedForwardNetwork() => ErrorFunc = "MSE";
        
        public double[][] ReturnValues()
        {

            for (int n = 0; n < layers.Count; n++)
            {
                values[n] = layers[n].inputs;

            }
            return values;
        }
        
        public void AddLayer(FeedForwardLayer a)
        {
            layers.Add(a);
            layernumber++;
            values = new double[layernumber][];
        }
        public double[] ComputeOutput(double[] input)
        {
            
            if (firstIteration)
            {
                firstIteration = false;
                for (int n = 0; n < layers.Count; n++)
                {
                    if (n == 0)
                    {
                        layers[0].IsInput = true;

                    }
                    else if (n != 0 && n + 1 < layers.Count)
                    {
                        layers[n].IsHidden = true;
                    }
                    else if (n + 1 == layers.Count)
                    {
                        layers[n].IsOutput = true;
                    }
                }
            }


            layers[0].ComputeOutput(input); //computes the output of the first layer which is stored in the object's output array
            for (int n = 1; n < layers.Count; n++)
            {
                if (layers[n].IsOutput)
                {
                    layers[n].outputs = layers[n - 1].outputs;
                    break;
                }
                layers[n].ComputeOutput(layers[n - 1].outputs);
                //Uses the pevious layer's output as the input of the current one.
            }

            output = layers[layers.Count - 1].outputs;
            return layers[layers.Count - 1].outputs;
        }

        public double[] ErrorFunctions(string errorf, double[] input, double[] expected)
        {
            
            if (errorf == "MSE")
            {

                CalcDeltas(input, expected);
                return deltas;
            }
            else if (errorf == "Log")
            {
                return Der_LogLoss(input, expected);
            }
            else
            {
                
                CalcDeltas(input, expected);
                return deltas;
            }
        }
        public double TotalError(double[] input, double[] expected)
        {
            totalError = 0;
            double[] fin = ComputeOutput(input);

            for (int n = 0; n < fin.Length; n++)
            {
                totalError += 0.5 * Math.Pow((expected[n] - fin[n]), 2);

            }

            return totalError;
        }
        public double LogLoss(double[] input, double[] expected)
        {
            totalError = 0;
            double[] fin = ComputeOutput(input);
            for (int n = 0; n < fin.Length; n++)
            {
                totalError += (-expected[n] * Math.Log(fin[n]) - (1 - expected[n]) * Math.Log(1 - fin[n]));
            }
            totalError = Math.Abs(totalError / fin.Length);
            return totalError;
        }

        public void CalcDeltas(double[] input, double[] expected)
        {
            deltas = new double[expected.Length];
            double[] fin = ComputeOutput(input);
            for (int n = 0; n < fin.Length; n++)
            {

                deltas[n] = (fin[n] - expected[n]);
            }
        }
        
        public double[] Der_LogLoss(double[] input, double[] expected)
        {
            double[] fin = ComputeOutput(input);
            double[] log = new double[expected.Length];
            for (int n = 0; n < fin.Length; n++)
            {
                log[n] = -(fin[n] - expected[n]) / ((fin[n] - 1) * fin[n]);
            }
            return log;
        }
        private double Der_ActivationFunctions(string FuncName, double value)
        {


            if (FuncName == "Sigmoid")
            {
                return ActivationFunc.Der_Sigmoid(value);
            }
            else if (FuncName == "TanH")
            {
                return ActivationFunc.Der_TanH(value);
            }
            else if (FuncName == "Softplus")
            {
                return ActivationFunc.Sigmoid(value);
            }
            else if (FuncName == "ReLU")
            {
                return ActivationFunc.Der_ReLU(value);
            }
            else if (FuncName == "Swish")
            {
                return ActivationFunc.Der_Swish(value);
            }
            else
            {
                return value;
            }
        }
        public void UpdateWeights(double lambda)
        {
            for (int n=0;n<layernumber-1; n++)
            {
                for (int r = 0; r < layers[n].Weight().Rows; r++)
                {
                    for (int c = 0; c < layers[n].Weight().Col; c++)
                    {
                        layers[n].weight[r, c] -= (layers[n].DeltaWeight[r, c]+lambda*layers[n].weight[r,c]);
                        
                    }
                }
            }
        }
        public void Train(double[] input, double[] expected, double lRate, double lambda)
        {
            double[] gamma;
            double[] cost_der = ErrorFunctions(ErrorFunc, input, expected);

            for (int n = NumberOfLayers - 1; n >= 0; n--)
            {
                if (n == NumberOfLayers - 1 || layers[n].IsOutput)
                {
                    
                    gamma = new double[layers[n - 1].Weight().Col];
                    //finding the partial derivative of the cost function in terms of the weight
                    //chain rule:  ∂Cost/∂weight = ∂Cost/∂NeuronOut * ∂NOut/∂NIn * ∂NIn/∂w
                    //= the derivaties of the cost, activation function and the node multiplied together
                    for (int k = 0; k < layers[n - 1].Weight().Col; k++)
                    {
                        gamma[k] = cost_der[k] * Der_ActivationFunctions(layers[n - 1].Activation, layers[n].outputs[k]);
                        //gamma array stores values that will also be needed later on
                    }
                    layers[n].Gammas = gamma;
                    
                    for (int r = 0; r < layers[n - 1].Weight().Rows; r++)
                    {
                        for (int c = 0; c < layers[n - 1].Weight().Col; c++)
                        {
                            
                            layers[n - 1].DeltaWeight[r,c] = (gamma[c] * layers[n - 1].inputs[r])*lRate;
                            //updating the weight
                            
                            
                        }
                    }


                }
                //Every other layer
                
                else if (n != 0 && n != NumberOfLayers - 1)
                {
                    gamma = new double[layers[n - 1].Weight().Col];
                    for (int k = 0; k < layers[n - 1].Weight().Col; k++)
                    {
                        for (int j = 0; j < layers[n + 1].Gammas.Length; j++)
                        {
                            gamma[k] += layers[n + 1].Gammas[j] * layers[n].Weight()[k, j];
                        }
                        gamma[k] *= Der_ActivationFunctions(layers[n - 1].Activation, layers[n].inputs[k]);
                    }
                    layers[n].Gammas = gamma;
                    for (int r = 0; r < layers[n - 1].Weight().Rows; r++)
                    {
                        for (int c = 0; c < layers[n - 1].Weight().Col; c++)
                        {
                            layers[n - 1].DeltaWeight[r, c] = (gamma[c] * layers[n - 1].inputs[r])*lRate;
                        }
                    }
                }
                UpdateWeights(lambda);
            }
        }


    }
}
