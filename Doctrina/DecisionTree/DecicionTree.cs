using System.Text;
using System.IO;

namespace Doctrina.DecisionTreeLib;

using Exceptions.DecisionTreeExceptions;
using Core;

public class DecisionTree
{
    public Node? Root { get; private set; }
    
    private int dataLength;
    
    public void Fit(DataSet<int, int> ds, int minSample, int maxDepth)
    {
        Root = new Node();
        dataLength = ds.X[0].Length;
        Root.Epoch(ds.X, ds.Y, minSample, maxDepth);
    }

    public float Choose(int[] data)
    {
        if (Root is null)
            throw new NecessaryTrainingException();

        if (dataLength != data.Length)
            throw new InvalidParameterSizeException();
        
        float result = CheckNode(Root, data);
        
        return result;
    }

    private float CheckNode(Node node, int[] data)
    {
        if (node.Decision(data[node.ColumnIndex]))
        {
            if (node.Right is not null)
                return CheckNode(node.Right, data);

            return node.Probability;
        }
        else
        {
            if (node.Left is not null)
                return CheckNode(node.Left, data);
            
            return node.Probability;
        }
    }

    public void Save(string path, int num = 0, string className = "DefaultModel")
    {
        if (Root is null)
            throw new NecessaryTrainingException();
        
        path += ".cs";
        
        StringBuilder func = new($"public partial class {className}" + "\n{\n");
        func.Append($"\tpublic float Choose{num}(int[] data)\n" + "\t{\n");
        func.Append(AppendNode(Root, 2, "if"));
        func.Append("\t}\n}");

        using StreamWriter sw = File.CreateText(path);
        sw.Write(func);
        sw.Close();
    }

    private StringBuilder AppendNode(Node node, int tabCount, string comparative)
    {
        string tab = string.Concat(Enumerable.Repeat("\t", tabCount));
        
        if (comparative == "if")
            comparative += $"(data[{node.ColumnIndex}] {ComparativeSing(node.Comparison)} {node.Target})";
        
        StringBuilder nodeCode = new(tab + comparative + $"\n{tab}" + "{\n");

        if (node.Right is not null)
                nodeCode.Append(AppendNode(node.Right, tabCount + 1, "if"));
                
        if (node.Left is not null)
            nodeCode.Append(AppendNode(node.Left, tabCount + 1, "else" ));

        if (!float.IsNaN(node.Probability))
            nodeCode.Append(tab + $"\treturn {node.Probability}f;\n".Replace(',', '.'));
        
        nodeCode.Append(tab + "}\n");

        return nodeCode;
    }

    private static string ComparativeSing(ComparisonSigns comparator)
    {
        return comparator switch
        {
            ComparisonSigns.Equal => "=",
            ComparisonSigns.Different => "!=",
            ComparisonSigns.Bigger => ">",
            ComparisonSigns.BiggerEqual => ">=",
            ComparisonSigns.Less => "<",
            ComparisonSigns.LessEqual => "<=",
            _ => throw new ArgumentException("Invalid comparator parameter", nameof(comparator))
        };
    }
}