namespace Doctrina.NeuralNetworkLib.Core;

public class Layer
{
    public Layer(Neuron[] neurons) =>
        Neurons = neurons;

    public Layer(int neuronsSize, int dataSize, ActivationFunction function)
    {
        Neurons = new Neuron[neuronsSize];

        for (int i = 0; i < neuronsSize; i++)
            Neurons[i] = new Neuron(dataSize, function);
    }

    public Neuron[] Neurons { get; private set; }

    public float[] Output(float[] x)
    {
        float[] y = new float[Neurons.Length];
        
        for (int i = 0; i < y.Length; i++)
            y[i] = Neurons[i].Output(x);

        return y;
    }
}