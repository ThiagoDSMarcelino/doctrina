using Doctrina.NeuralNetworkLib;
using Doctrina.DecisionTreeLib;
using Doctrina;
using System;

int[][] X = new int[][]
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

int[] Y = new int[]
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

DataSet<int, int> ds = DataSet<int, int>.Load(X, Y);

var (train, test) = ds.SplitTrainTest(0.25f);

DecisionTree dt = new();
dt.Fit(train, 2, 3);

foreach ((int[] x, int y) in test)
    Console.WriteLine($"{dt.Choose(x)} {y}");

dt.Save("Test", 2, "TestModel");

// NeuralNetwork neuralNetwork = new NeuralNetwork(Functions.Sigmoid, 3, 3, 5, 2);

// neuralNetwork.Fit(train, 1000);

// System.Console.WriteLine(neuralNetwork.Accuracy(test));