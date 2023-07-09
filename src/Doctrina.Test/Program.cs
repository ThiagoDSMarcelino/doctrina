using Doctrina.DecisionTreeLib;
using System;

float[][] X = new float[][]
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

float[] Y = new float[]
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

DecisionTree dt = new();
dt.Fit(X, Y, 2, 3);

for (int i = 0; i < X.Length; i++)
    Console.WriteLine($"{dt.Choose(X[i])} {Y[i]}");

dt.Save("Test");

// NeuralNetwork neuralNetwork = new NeuralNetwork(Functions.Sigmoid, 3, 3, 5, 2);

// neuralNetwork.Fit(train, 1000);

// System.Console.WriteLine(neuralNetwork.Accuracy(test));