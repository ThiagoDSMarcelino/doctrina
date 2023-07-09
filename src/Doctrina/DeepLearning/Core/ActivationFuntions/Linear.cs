namespace Doctrina.DeepLearning.Core.ActivationFuntions;

public class Linear : ActivationFunction
{
    public override float Compute(float x)
        => x;

    public override float Derivate(float x)
        => x;
}