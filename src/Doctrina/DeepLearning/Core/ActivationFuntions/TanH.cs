namespace Doctrina.DeepLearning.Core.ActivationFuntions;

public class TanH : ActivationFunction
{
    public override float Compute(float x)
        => MathF.Tanh(x);

    public override float Derivate(float x)
        => MathF.Tanh(x);
}