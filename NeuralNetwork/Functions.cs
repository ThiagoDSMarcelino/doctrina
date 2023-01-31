namespace MachineLearningLib.NeuralNetworkLib;

using Core;

public static class Functions
{
    private static ActivationFunction reLu = new Relu();
    public static ActivationFunction ReLu => reLu;

    private static ActivationFunction sigmoid = new Sigmoid();
    public static ActivationFunction Sigmoid => sigmoid;

    private static ActivationFunction linear = new Linear();
    public static ActivationFunction Linear => linear;

    private static ActivationFunction tanh = new Tanh();
    public static ActivationFunction Tanh => tanh;
}