namespace Doctrina.DeepLearning.Core.ActivationFuntions;

public abstract class ActivationFunction
{
    public abstract float Compute(float input);
    public virtual void Compute(float[] input)
    {
        for (int i = 0; i < input.Length; i++)
            input[i] = Compute(input[i]);
    }

    public abstract float Derivate(float input);
    public virtual void Derivate(float[] input)
    {
        for (int i = 0; i < input.Length; i++)
            input[i] = Derivate(input[i]);
    }
}