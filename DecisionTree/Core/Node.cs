namespace MachineLearningLib.DecisionTreeLib.Core;

using System.Collections.Generic;
using System.Linq;
using System;

public class Node
{
    public Node Left { get; private set; }
    public Node Right { get; private set; }
    public ComparisonSigns Comparison { get; private set; } = ComparisonSigns.Bigger;
    public float Target { get; private set; }
    public int ColumnIndex { get; private set; }
    public float Probability { get; private set; }
        
    public void Epoch(int[][] x, int[] y, int minSample, int maxDepth)
    {
        if (maxDepth == 0 || x.GetLength(0) < minSample)
        {
            this.Probability = y.Count(i => i == 1) * 1f / (y.Length == 0 ? 1 : y.Length);
            return;
        }

        float[] I = this.InformationEntropy(x, y),
                C = this.ContentEntropy(x, y),
                G = new float[I.Length];
        
        for (int i = 0; i < G.Length; i++)
            G[i] = I[i] / C[i];

        int colIndex = G
            .Select((num, index) => (num, index))
            .MaxBy(e => e.num)
            .index;
        
        this.ColumnIndex = colIndex;

        this.Target = this.SelectTarget(
            x.Length,
            new int[x.Length]
                .Select((e, i) => x[i][this.ColumnIndex])
                .ToArray()
        );
        
        List<int[]> leftX = new List<int[]>(),
                    rightX = new List<int[]>();
        
        List<int> leftY = new List<int>(),
                  rightY = new List<int>();
        
        for (int i = 0; i < x.GetLength(0); i++)
        {
            if (this.Decision(x[i][colIndex]))
            {
                rightX.Add(x[i]);
                rightY.Add(y[i]);
            }
            
            else
            {
                leftX.Add(x[i]);
                leftY.Add(y[i]);
            }
        }

        this.Left = new Node();
        this.Right = new Node();

        this.Right.Epoch(rightX.ToArray(), rightY.ToArray(), minSample, maxDepth - 1);
        this.Left.Epoch(leftX.ToArray(), leftY.ToArray(), minSample, maxDepth - 1);
    }

    public float SelectTarget(int n, int[] col)
    {
        return (col.Max() + col.Min()) / 2;
    }

    public float[] InformationEntropy(int[][] x, int[] y)
    {
        float n = y.Count(),
              trueValues = y.Count(i => i == 1) / n,
              falseValues = y.Count(i => i == 0) / n,
              E0 =  -(trueValues * MathF.Log2(trueValues)) +
                    -(falseValues * MathF.Log2(falseValues));
        
        float[] result = new float[x.GetLength(1)];
        
        for (int j = 0; j < x.GetLength(1); j++)
        {
            int[]
                col = new int[x.Length]
                    .Select((e, i) => x[i][j])
                    .ToArray(),
                attrs = col
                    .Distinct()
                    .ToArray();
            float ECol = 0;

            for (int i = 0; i < attrs.Length; i++)
            {
                var attr = attrs[i];

                float attrCount = 0,
                      trueCount = 0,
                      falseCount = 0;
                    
                for (int k = 0; k < col.Length; k++)
                {
                    if (col[k] == attr)
                    {
                        attrCount++;
                        if (y[k] == 1)
                            trueCount++;
                        else
                            falseCount++;
                    }
                }

                if (trueCount == 0 || falseCount == 0 ||
                    trueCount == attrCount || falseCount == attrCount)
                    continue;
                
                float total = (attrCount / n),
                      trueTotal = -((trueCount / attrCount) * MathF.Log2(trueCount / attrCount)),
                      falseTotal = -((falseCount / attrCount) * MathF.Log2(falseCount / attrCount)),
                      temp = total * (trueTotal + falseTotal);
                
                ECol += temp;
            }

            result[j] = E0 - ECol;
        }

        return result;
    }


    public float[] ContentEntropy(int[][] x, int[] y)
    {
        float n = y.Length;
        
        float[] result = new float[x[0].Length];

        for (int j = 0; j < x[0].Length; j++)
        {
            int[]
                col = new int[x.Length]
                    .Select((e, i) => x[i][j])
                    .ToArray(),
                attrs = col
                    .Distinct()
                    .ToArray();
            float ECol = 0;

            for (int i = 0; i < attrs.Length; i++)
            {
                var attr = attrs[i];

                float attrCount = 0;
                    
                for (int k = 0; k < col.Length; k++)
                {
                    if (col[k] == attr)
                        attrCount++;
                }
                float total = attrCount / n,
                      temp = -total * MathF.Log2(total);

                ECol += temp;
            }

            result[j] = ECol;
        }
        return result;
    }

    public bool Decision(int value)
    {
        switch (this.Comparison)
        {
            case ComparisonSigns.Equal:
                return value == this.Target;

            case ComparisonSigns.Bigger:
                return value > this.Target;

            case ComparisonSigns.BiggerEqual:
                return value >= this.Target;

            case ComparisonSigns.Less:
                return value < this.Target;

            case ComparisonSigns.LessEqual:
                return value <= this.Target;

            default:
                return value != this.Target;
        }
    }
}