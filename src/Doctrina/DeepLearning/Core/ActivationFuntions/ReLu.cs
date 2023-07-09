namespace Doctrina.DeepLearning.Core.ActivationFuntions;

public class ReLu : ActivationFunction
{
    public override float Compute(float x)
    {
        if (x < 0f)
            return 0f;

        return x;
    }

    public override float Derivate(float x)
    {
        if (x < 0f)
            return 0f;

        return x;
    }
}