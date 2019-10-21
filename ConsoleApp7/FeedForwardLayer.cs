using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp7
{
    class FeedForwardLayer
    {
        
        public bool IsInput { get => Is_Input; set => Is_Input = value; }
        public bool IsHidden { get => Is_Hidden; set => Is_Hidden = value; }
        public bool IsOutput { get => Is_Output; set => Is_Output = value; }

        int InputNeurons; //No. of input neurons
        int output; //No. of output neurons
        internal double[] inputs;
        internal double[] outputs;
        internal double[] Gammas; //stores values that are used for derivation of the hidden layers
        public Matrix weight; //stores the weight values of each input neuron corresponding to each output neuron
        public Matrix DeltaWeight;
        private bool Is_Input = false;
        private bool Is_Hidden = false;
        private bool Is_Output = false;
        internal string Activation;

        private double ActivationFunctions(string FuncName, double value)
        {

            if (FuncName == "Sigmoid")
            {
                return ActivationFunc.Sigmoid(value);
            }
            else if (FuncName == "TanH")
            {
                return Math.Tanh(value);
            }
            else if (FuncName == "Softplus")
            {
                return ActivationFunc.Softplus(value);
            }
            else if (FuncName == "ReLU")
            {
                return ActivationFunc.ReLU(value);
            }
            else if (FuncName == "Swish")
            {
                return ActivationFunc.Swish(value);
            }
            else
            {
                return value;
            }
        }
        
        public FeedForwardLayer(int n, int output, string act)
        {
            this.output = output;
            outputs = new double[output];
            InputNeurons = n;
            weight = new Matrix(n + 1, output); //Weights stored in matrix. Last row is for the bias node
            DeltaWeight = new Matrix(weight.Rows, weight.Col);
            weight.RandomWeight();
            for (int k = 0; k < weight.Col; k++)
            {
                weight[weight.Rows - 1, k] = 0;

            }
            inputs = new double[InputNeurons];
            Activation = act;
            Gammas = new double[n];
        }
        public FeedForwardLayer(int n)
        {
            output = n;
            outputs = new double[n];
            InputNeurons = n;
            IsOutput = true;
            inputs = new double[InputNeurons];
        }

        public double[] ComputeOutput(double[] input_)
        {
            //Calculates the value of nodes in the next layer
            //Node = Sum of previous layer's nodes multiplied by their corresponding weights connected to this node.
            inputs = input_;
            Array.Resize(ref inputs, input_.Length + 1);
            inputs[inputs.Length - 1] = 1; //initializing the bias

            for (int n = 0; n < outputs.Length; n++)
            {
                outputs[n] = 0;
            }

            for (int n = 0; n < weight.Rows; n++)
            {
                for (int j = 0; j < weight.Col; j++)
                {
                    outputs[j] += inputs[n] * weight[n, j];

                }
            }

            for (int n = 0; n < weight.Col; n++)
            {
                outputs[n] = ActivationFunctions(Activation, outputs[n]);
            }
            
            

            return outputs;
        }
        public Matrix Weight()
        {
            return weight;
        }

    }
}
