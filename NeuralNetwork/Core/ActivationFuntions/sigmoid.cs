namespace MachineLearningLib.NeuralNetworkLib.Core;

using System;

public class Sigmoid : ActivationFunction
{
    public override float Compute(float x) =>
        1f / (1f + MathF.Exp(-x));
    public override float Derivate(float x) =>
        1f / (1f + MathF.Exp(-x));
}