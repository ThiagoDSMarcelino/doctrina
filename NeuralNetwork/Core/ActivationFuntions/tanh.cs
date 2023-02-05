namespace Doctrina.NeuralNetworkLib.Core;

using System;

public class Tanh : ActivationFunction
{
    public override float Compute(float x) => MathF.Tanh(x);
    public override float Derivate(float x) => MathF.Tanh(x);
}