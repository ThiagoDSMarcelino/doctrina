using MachineLearningLib.NeuralNetworkLib;
using MachineLearningLib.DecisionTreeLib;
using MachineLearningLib;

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

DataSet<float, int> ds = DataSet<float, int>.Load(x, y);

var (train, test) = ds.SplitTrainTest(0.25f);

// DecisionTree dt = new DecisionTree();

// NeuralNetwork neuralNetwork = new NeuralNetwork(Functions.Sigmoid, 3, 3, 5, 2);

// neuralNetwork.Fit(train, 1000);

// System.Console.WriteLine(neuralNetwork.Accuracy(test));