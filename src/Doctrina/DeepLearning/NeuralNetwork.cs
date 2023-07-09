
namespace Doctrina.DeepLearning;

using Core.ActivationFuntions;
using Core;

public class NeuralNetwork
{
    public Layer[] Layers { get; private set; }

    public NeuralNetwork(Layer[] layer)
        =>  Layers = layer;
    
    public NeuralNetwork(ActivationFunction function, params int[] layersData)
    {
        Layers = new Layer[layersData.Length - 1];

        for (int i = 0; i < layersData.Length - 1; i++)
            Layers[i] = new Layer(
                layersData[i + 1],
                layersData[i],
                function
            );
    }

    public float[] Output(params float[] x)
    {
        for (int i = 0; i < Layers.Length; i++)
            x = Layers[i].Output(x);

        return x;
    }

    public NeuralNetworkResult Choose(params float[] x) {
        float[] output = Output(x);

        var result = new NeuralNetworkResult()
        {
            Result = int.MinValue,
            Probability = float.MaxValue
        };

        for (int i = 0; i < output.Length; i++)
        {
            float value = output[i];

            if (result.Probability > value)
            {
                result.Probability = value;
                result.Result = i;
            }
        }

        return result;
    }
        
    public float Score(float[][] x, float[] y)
    {
        float E = 0;

        for (int i = 0; i < x.Length; i++)
        {
            float[] z = Output(x[i]);

            for (int j = 0; j < z.Length; j++)
            {
                float value = z[j] - (y[i] == j ? 1 : 0);
                value *= value;
                E += value;
            }
        }

        return E / (0.5f * x.Length * x[0].Length);
    }

    public float Accuracy(float[] x, float[] y)
    {
        int count = 0;

        for (int i = 0; i < x.Length; i++)
        {
            if (Choose(x[i]).Result == y[i])
                count++;
        }

        return count / x.Length;
    }

    public void Fit(float[][] x, float[] y, int epochs = 100, float eta = 0.05f)
    {
        for (int i = 0; i < epochs; i++)
            Epoch(x, y, eta);
    }

    private void Epoch(float[][] x, float[] y, float eta)
    {
        for (int i = 0; i < Layers.Length; i++)
        {
            for (int j = 0; j < Layers[i].Neurons.Length; j++)
            {
                Neuron neuron = Layers[i].Neurons[j];

                float error = Score(x, y);
                neuron.B += 0.1f;

                float newError = Score(x, y);
                neuron.B -= 0.1f;
                
                float dE = (newError - error) * 10f;
                neuron.B -= eta * dE;

                error = Score(x, y);

                for (int k = 0; k < neuron.W.Length; k++)
                {
                    neuron.W[k] += 0.1f;
                    newError = Score(x, y);
                    neuron.W[k] -= 0.1f;

                    dE = (newError - error) * 10f;
                    neuron.W[k] -= eta * dE;
                    error = Score(x, y);
                }
            }
        }
    }
}