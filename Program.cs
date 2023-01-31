using MachineLearningLib;
using MachineLearningLib.NeuralNetworkLib;

float[][] x = new float[][]
{
    new float[] { 1, 1, 1 },
    new float[] { 0, 0, 1 },
    new float[] { 1, 0, 1 },
    new float[] { 0, 1, 1 },
    new float[] { 1, 1, 0 },
    new float[] { 1, 0, 0 },
    new float[] { 0, 0, 0 },
    new float[] { 0, 1, 0 }
};

int[] y = new int[]
{
    1,
    1,
    1,
    1,
    1,
    0,
    0,
    0
};

DataSet<float, int> ds = new DataSet<float, int>(x, y);
NeuralNetwork neuralNetwork = new NeuralNetwork(Functions.Sigmoid, 3, 3, 5, 2);
neuralNetwork.Fit(ds, 10000);

System.Console.WriteLine(neuralNetwork.Choose(1f, 0f, 1f));