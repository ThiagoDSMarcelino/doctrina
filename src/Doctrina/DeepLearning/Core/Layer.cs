namespace Doctrina.DeepLearning.Core;

using ActivationFuntions;

public class Layer
{
    public Neuron[] Neurons { get; private set; }

    public Layer(Neuron[] neurons)
        => Neurons = neurons;

    public Layer(int neuronsSize, int dataSize, ActivationFunction function)
    {
        Neurons = new Neuron[neuronsSize];

        for (int i = 0; i < neuronsSize; i++)
            Neurons[i] = new Neuron(dataSize, function);
    }

    public float[] Output(float[] x)
    {
        float[] y = new float[Neurons.Length];
        
        for (int i = 0; i < y.Length; i++)
            y[i] = Neurons[i].Output(x);

        return y;
    }
}