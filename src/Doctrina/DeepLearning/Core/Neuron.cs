namespace Doctrina.DeepLearning.Core;

using ActivationFuntions;
using Exceptions;

public class Neuron
{
    public float B { get; set; }

    public float[] W { get; private set; }
    
    public ActivationFunction Function { get; private set; }
    
    public Neuron(int dataSize, ActivationFunction function)
    {
        Function = function;
        B = GaussianDist();
        W = new float[dataSize];

        for (int i = 0; i < dataSize; i++)
            W[i] = GaussianDist();
    }
    
    public Neuron(float bias, float[] weights, ActivationFunction function)
    {
        B = bias;
        W = weights;
        Function = function;
    }

    public float Output(float[] x)
    {
        if (x.Length != W.Length)
            throw new InvalidParameterSizeException();
        
        float s = B;
        for (int i = 0; i < x.Length; i++)
            s += x[i] * W[i];
        
        float z = Function.Compute(s);

        return z;
    }

    private static float GaussianDist(int n = 10)
    {
        float sum = 0f;

        for (int i = 0; i < n; i++)
            sum += 2 * Random.Shared.NextSingle() - 1;
        
        return sum / n;
    }
}