namespace MachineLearningLib.NeuralNetwork.Core;

public class Layer
{
    public Layer(Neuron[] neurons) =>
        this.Neurons = neurons;

    public Layer(int neuronsSize, int dataSize, ActivationFunction function)
    {
        this.Neurons = new Neuron[neuronsSize];

        for (int i = 0; i < neuronsSize; i++)
            this.Neurons[i] = new Neuron(dataSize, function);
    }

    public Neuron[] Neurons { get; private set; }
    public float[] Output(float[] x)
    {
        float[] y = new float[this.Neurons.Length];
        
        for (int i = 0; i < y.Length; i++)
            y[i] = this.Neurons[i].Output(x);

        return y;
    }
}