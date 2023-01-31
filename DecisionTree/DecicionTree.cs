namespace MachineLearningLib.DecisionTreeLib;

using Exceptions;
using Core;

public class DecisionTree
{
    public Node Root { get; private set; }
    private int dataLength { get; set; }
    public void Fit(DataSet ds, int minSample, int maxDepth)
    {
        this.Root = new Node();
        this.dataLength = x[0].Length;
        this.Root.Epoch(x, y, minSample, maxDepth);
    }

    public float Choose(int[] data)
    {
        if (this.Root is null)
            throw new NecessaryTrainingException();

        if (this.dataLength != data.Length)
            throw new InvalidParameterSizeException();
        
        float result = this.CheckNode(this.Root, data);
        
        return result;
    }
    private float CheckNode(Node node, int[] data)
    {
        if (node.Decision(data[node.ColumnIndex]))
        {
            if (node.Right is not null)
                return this.CheckNode(node.Right, data);
            return node.Probability;
        }
        else
        {
            if (node.Left is not null)
                return this.CheckNode(node.Left, data);
            return node.Probability;
        }
    }
}