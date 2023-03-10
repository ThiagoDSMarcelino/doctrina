using Doctrina.DecisionTreeLib;
using Doctrina;
using System;

// DecisionTree example
var ds = DataSet<float, float>.Load("Test/trainingdata.txt", ',', 1);

var (train, test) = ds.SplitTrainTest(0.25f);

DecisionTree<float, float> dt = new DecisionTree<float, float>();

dt.Fit(ds, 2, 3);

// Choose returns the probability of being true
Console.WriteLine($"{dt.Choose(test.X[0])} {test.Y[0]}");

// You can save to use the same model without train again
dt.Save("Test/TestDecisionTree", "TestModel", "TestChoose");