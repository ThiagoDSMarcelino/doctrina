![GitHub](https://img.shields.io/github/license/ThiagoDSMarcelino/Doctrina?color=blue)
![GitHub last commit](https://img.shields.io/github/last-commit/ThiagoDSMarcelino/Doctrina)
![GitHub commit activity](https://img.shields.io/github/commit-activity/m/ThiagoDSMarcelino/Doctrina)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/ThiagoDSMarcelino/Doctrina)

# Doctrina

Introducing Doctrina, the most user-friendly machine learning library for teaching and explaining AI. This library is designed from scratch to address the complex challenges of teaching students about the world of artificial intelligence, using simple syntax and being an open source project. It is perfect for explaining and demonstrating how ML/DL algorithms really work.

## Installation

It is necessary to have .NET Framework 7 or above installed on the machine, and that is all.

## Usage

```csharp
using Doctrina.DecisionTreeLib;
using Doctrina;
using System;

// DecisionTree example

var ds = DataSet<float, float>.Load("Test/trainingdata.txt", ',', 1);

var (train, test) = ds.SplitTrainTest(0.25f);

DecisionTree<float, float> dt = new DecisionTree<float, float>();

dt.Fit(ds, 2, 3);

// Choose returns the probability of being true
Console.WriteLine($"{dt.Choose(test.X[0])} {test.Y}");


// You can save to use the same model without train again
dt.Save("Test/TestDecisionTree", "TestModel");
```
