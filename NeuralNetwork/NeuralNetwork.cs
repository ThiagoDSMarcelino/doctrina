namespace MachineLearningLib.NeuralNetworkLib;

using Core;

public class NeuralNetwork
{
    public NeuralNetwork(Layer[] layer) =>
        this.Layers = layer;
    
    public NeuralNetwork(ActivationFunction function, params int[] layersData)
    {
        this.Layers = new Layer[layersData.Length - 1];

        for (int i = 0; i < layersData.Length - 1; i++)
            this.Layers[i] = new Layer(
                layersData[i + 1],
                layersData[i],
                function
            );
    }

    public Layer[] Layers { get; private set; }

    public float[] Output(params float[] x)
    {
        for (int i = 0; i < this.Layers.Length; i++)
            x = this.Layers[i].Output(x);

        return x;
    }

    public (int, float) Choose(params float[] x) {
        float[] output = this.Output(x);
        float probability = float.MinValue;
        int result = int.MinValue;

        for (int i = 0; i < output.Length; i++)
        {
            float value = output[i];
            if (probability < value)
            {
                probability = value;
                result = i;
            }
        }


        return (result, probability);
    }
        

    public float Score(DataSet<float, int> ds)
    {
        float E = 0;
        foreach (var (x, y) in ds)
        {
            float[] z = this.Output(x);
            for (int i = 0; i < z.Length; i++)
            {
                float value = z[i] - (y == i ? 1 : 0);
                value = value * value;
                E += value;
            }
        }
        return E / (0.5f * ds.Length * ds.X.Length);
    }

    public void Fit(DataSet<float, int> ds, int epochs = 100, float eta = 0.05f)
    {
        for (int i = 0; i < epochs; i++)
            this.epoch(ds, eta);
    }

    private void epoch(DataSet<float, int> ds, float eta)
    {
        for (int i = 0; i < this.Layers.Length; i++)
        {
            for (int j = 0; j < this.Layers[i].Neurons.Length; j++)
            {
                Neuron neuron = this.Layers[i].Neurons[j];

                float error = Score(ds);
                neuron.B += 0.1f;
                float newError = Score(ds);
                neuron.B -= 0.1f;

                float dE = (newError - error) / 0.1f;
                neuron.B -= eta * dE;
                error = Score(ds);

                for (int k = 0; k < neuron.W.Length; k++)
                {
                    neuron.W[k] += 0.1f;
                    newError = Score(ds);
                    neuron.W[k] -= 0.1f;

                    dE = (newError - error) / 0.1f;
                    neuron.W[k] -= eta * dE;
                    error = Score(ds);
                }
            }
        }
    }
}