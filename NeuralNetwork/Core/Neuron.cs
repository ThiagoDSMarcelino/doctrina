namespace Doctrina.NeuralNetworkLib.Core;

using Exceptions.NeuralNetworkExceptions;
using System;

public class Neuron
{
    public Neuron(int dataSize, ActivationFunction function)
    {
        this.Function = function;
        this.B = gaussianDist();
        this.W = new float[dataSize];
        for (int i = 0; i < dataSize; i++)
            this.W[i] = gaussianDist();
    }
    
    public Neuron(float bias, float[] weights, ActivationFunction function)
    {
        this.B = bias;
        this.W = weights;
        this.Function = function;
    }

    public float B { get; set; }
    public float[] W { get; private set; }
    public ActivationFunction Function { get; private set; }
    public float Output(float[] x)
    {
        if (x.Length != W.Length)
            throw new InvalidParameterSizeException();
        
        float s = this.B;
        for (int i = 0; i < x.Length; i++)
            s += x[i] * this.W[i];
        
        float z = this.Function.Compute(s);
        return z;
    }

    private float gaussianDist(int n = 10)
    {
        float sum = 0f;

        for (int i = 0; i < n; i++)
            sum += 2 * Random.Shared.NextSingle() - 1;
        
        return sum / n;
    }
}