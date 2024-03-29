namespace Doctrina.DeepLearning;

using Core.ActivationFuntions;

public static class Functions
{
    private static readonly ActivationFunction reLu = new ReLu();
    public static ActivationFunction ReLu => reLu;


    private static readonly ActivationFunction sigmoid = new Sigmoid();
    public static ActivationFunction Sigmoid => sigmoid;


    private static readonly ActivationFunction linear = new Linear();
    public static ActivationFunction Linear => linear;


    private static readonly ActivationFunction tanh = new TanH();
    public static ActivationFunction TanH => tanh;
}