using Doctrina.NeuralNetworkLib;
using Doctrina.DecisionTreeLib;
using Doctrina;
using System;

int[][] x = new int[][]
{
    new int[] { 1, 1, 1 },
    new int[] { 0, 0, 1 },
    new int[] { 1, 0, 1 },
    new int[] { 0, 1, 1 },
    new int[] { 1, 1, 0 },
    new int[] { 1, 0, 0 },
    new int[] { 0, 0, 0 },
    new int[] { 0, 1, 0 }
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

// DataSet<int, int> ds = DataSet<int, int>.Load(x, y);
var ds = DataSet<float, float>.Load("trainingdata.txt", ',', 1);

var (train, test) = ds.SplitTrainTest(0.25f);

DecisionTree<float, float> dt = new DecisionTree<float, float>();
dt.Fit(ds, 2, 3);

// Console.WriteLine($"{dt.Choose(test.X[0])} {dt.Choose(test.Y)}");

dt.Save("Test/Test", 2, "SlaModel");

// NeuralNetwork neuralNetwork = new NeuralNetwork(Functions.Sigmoid, 3, 3, 5, 2);

// neuralNetwork.Fit(train, 1000);

// System.Console.WriteLine(neuralNetwork.Accuracy(test));