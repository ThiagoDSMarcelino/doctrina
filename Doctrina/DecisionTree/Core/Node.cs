namespace Doctrina.DecisionTreeLib.Core;

public class Node
{
    public Node Left { get; private set; }

    public Node Right { get; private set; }
    
    public ComparisonSigns Comparison { get; private set; } = ComparisonSigns.Bigger;
    
    public float Target { get; private set; }
    
    public int ColumnIndex { get; private set; }
    
    public float Probability { get; private set; } = float.NaN;
        
    public void Epoch(int[][] x, int[] y, int minSample, int maxDepth)
    {
        if (maxDepth == 0 || x.GetLength(0) < minSample)
        {
            Probability = y.Count(i => i == 1) * 1f / (y.Length == 0 ? 1 : y.Length);
            return;
        }

        float[] I = InformationEntropy(x, y),
                C = ContentEntropy(x, y),
                G = new float[I.Length];
        
        for (int i = 0; i < G.Length; i++)
            G[i] = I[i] / C[i];

        int colIndex = G
            .Select((num, index) => (num, index))
            .MaxBy(e => e.num)
            .index;
        
        ColumnIndex = colIndex;

        Target = SelectTarget(
            new int[x.Length]
                .Select((_, i) => x[i][this.ColumnIndex])
                .ToArray()
        );
        
        List<int[]>
            leftX = new(),
            rightX = new();
        
        List<int>
            leftY = new(),
            rightY = new();
        
        for (int i = 0; i < x.GetLength(0); i++)
        {
            if (Decision(x[i][colIndex]))
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

        Left = new Node();
        Right = new Node();

        Right.Epoch(rightX.ToArray(), rightY.ToArray(), minSample, maxDepth - 1);
        Left.Epoch(leftX.ToArray(), leftY.ToArray(), minSample, maxDepth - 1);
    }

    public static float SelectTarget(int[] col) =>
        (col.Max() + col.Min()) / 2;

    public static float[] InformationEntropy(int[][] x, int[] y)
    {
        float
            n = y.Length,
            trueValues = y.Count(i => i == 1) / n,
            falseValues = y.Count(i => i == 0) / n,
            E0 =  -(trueValues * MathF.Log2(trueValues)) + -(falseValues * MathF.Log2(falseValues));
        
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

                float
                    attrCount = 0,
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
                
                float
                    total = (attrCount / n),
                    trueTotal = -((trueCount / attrCount) * MathF.Log2(trueCount / attrCount)),
                    falseTotal = -((falseCount / attrCount) * MathF.Log2(falseCount / attrCount)),
                    temp = total * (trueTotal + falseTotal);
                
                ECol += temp;
            }

            result[j] = E0 - ECol;
        }

        return result;
    }


    public static float[] ContentEntropy(int[][] x, int[] y)
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
        switch (Comparison)
        {
            case ComparisonSigns.Equal:
                return value == Target;

            case ComparisonSigns.Bigger:
                return value > Target;

            case ComparisonSigns.BiggerEqual:
                return value >= Target;

            case ComparisonSigns.Less:
                return value < Target;

            case ComparisonSigns.LessEqual:
                return value <= Target;

            default:
                return value != Target;
        }
    }
}