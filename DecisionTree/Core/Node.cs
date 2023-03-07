using System.Collections.Generic;
using System.Linq;
using System;

namespace Doctrina.DecisionTreeLib.Core;

public class Node<T1, T2>
    where T1 : unmanaged, IComparable
    where T2 : unmanaged, IComparable
    
{
    public Node<T1, T2> Left { get; private set; }
    public Node<T1, T2> Right { get; private set; }
    public ComparisonSigns Comparison { get; private set; } = ComparisonSigns.Bigger;
    public DataType<T1> Target { get; private set; }
    public int ColumnIndex { get; private set; }
    public double Probability { get; private set; } = double.NaN;
        
    public void Epoch(DataSet<T1, T2> ds, int minSample, int maxDepth)
    {
        if (maxDepth == 0 || ds.X.Length < minSample)
        {
            this.Probability = ds.Y.Count(i => i == 1) * 1f / (ds.Y.Length == 0 ? 1 : ds.Y.Length);
            return;
        }

        float[] I = this.InformationEntropy(ds),
                C = this.ContentEntropy(ds),
                G = new float[I.Length];
        
        for (int i = 0; i < G.Length; i++)
            G[i] = I[i] / C[i];

        int colIndex = G
            .Select((num, index) => (num, index))
            .MaxBy(e => e.num)
            .index;
        
        this.ColumnIndex = colIndex;
        
        var col = new T1[ds.X.Length]
            .Select((_, i) => ds.X[i][this.ColumnIndex])
            .ToArray();

        this.Target = (col.Max() + col.Min()) / 2;
        
        List<DataType<T1>[]>
            leftX = new List<DataType<T1>[]>(),
            rightX = new List<DataType<T1>[]>();
        
        List<DataType<T2>>
            leftY = new List<DataType<T2>>(),
            rightY = new List<DataType<T2>>();
        
        for (int i = 0; i < ds.X.Length; i++)
        {
            if (this.Decision(ds.X[i][colIndex]))
            {
                rightX.Add(ds.X[i]);
                rightY.Add(ds.Y[i]);
            }
            
            else
            {
                leftX.Add(ds.X[i]);
                leftY.Add(ds.Y[i]);
            }
        }

        DataSet<T1, T2>
            dsRight = DataSet<T1, T2>.Load(rightX.ToArray(), rightY.ToArray()),
            dsLeft = DataSet<T1, T2>.Load(leftX.ToArray(), leftY.ToArray());
        
        this.Left = new Node<T1, T2>();
        this.Right = new Node<T1, T2>();

        this.Right.Epoch(dsRight, minSample, maxDepth - 1);
        this.Left.Epoch(dsLeft, minSample, maxDepth - 1);
    }

    public float[] InformationEntropy(DataSet<T1, T2> ds)
    {
        float n = ds.Y.Count(),
              trueValues = ds.Y.Count(i => i == 1) / n,
              falseValues = ds.Y.Count(i => i == 1) / n,
              E0 =  -(trueValues * MathF.Log2(trueValues)) +
                    -(falseValues * MathF.Log2(falseValues));
        
        float[] result = new float[ds.X[0].Length];
        
        for (int j = 0; j < ds.X[0].Length; j++)
        {
            DataType<T1>[]
                col = new int[ds.X.Length]
                    .Select((e, i) => ds.X[i][j])
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
                        if (ds.Y[k] == 1)
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


    public float[] ContentEntropy(DataSet<T1, T2> ds)
    {
        float n = ds.Y.Length;
        
        float[] result = new float[ds.X[0].Length];

        for (int j = 0; j < ds.X[0].Length; j++)
        {
            DataType<T1>[]
                col = new int[ds.X.Length]
                    .Select((e, i) => ds.X[i][j])
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

    public bool Decision(DataType<T1> value)
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