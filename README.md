# Doctrina
![GitHub](https://img.shields.io/github/license/ThiagoDSMarcelino/Doctrina?color=blue)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/ThiagoDSMarcelino/Doctrina)

## Introduction
Doctrina, the most user-friendly machine learning library for teaching and explaining AI. This library is designed from scratch to address the complex challenges of teaching students about the world of artificial intelligence, using simple syntax and being an open source project. It is perfect for explaining and demonstrating how ML/DL algorithms really work.

## Installation

// TODO

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
