namespace Doctrina.DecisionTreeLib;

using Exceptions.DecisionTreeExceptions;
using System.Text;
using System.Linq;
using System.IO;
using System;
using Core;

public class DecisionTree<T1, T2>
    where T1 : unmanaged
    where T2 : unmanaged
{
    public Node<T1, T2> Root { get; private set; }
    private int dataLength { get; set; }
    public void Fit(DataSet<T1, T2> ds, int minSample, int maxDepth)
    {
        this.Root = new Node<T1, T2>();
        this.dataLength = ds.X[0].Length;
        this.Root.Epoch(ds, minSample, maxDepth);
    }

    public float Choose(T1[] data)
    {
        if (this.Root is null)
            throw new NecessaryTrainingException();

        if (this.dataLength != data.Length)
            throw new InvalidParameterSizeException();
        
        float result = this.CheckNode(this.Root, data);
        
        return result;
    }
    private float CheckNode(Node<T1, T2> node, T1[] data)
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

    public void Save(string path, int num = 0, string className = "DefaultModel")
    {
        path += ".cs";
        
        StringBuilder func = new StringBuilder($"public partial class {className}" + "\n{\n");
        func.Append($"\tpublic float Choose{num}(int[] data)\n" + "\t{\n");
        func.Append(this.appendNode(this.Root, 2, "if"));
        func.Append("\t}\n}");
        using (StreamWriter sw = File.CreateText(path))
        {
            sw.Write(func);
        }
    }

    private StringBuilder appendNode(Node<T1, T2> node, int tabCount, string comparative)
    {
        string tab = String.Concat(Enumerable.Repeat("\t", tabCount));
        if (comparative == "if")
            comparative += $"(data[{node.ColumnIndex}] {this.comparativeSing(node.Comparison)} {node.Target})";
        
        StringBuilder nodeCode = new StringBuilder(tab + comparative + $"\n{tab}" + "{\n");

        if (node.Right is not null)
                nodeCode.Append(this.appendNode(node.Right, tabCount + 1, "if"));
                
        if (node.Left is not null)
            nodeCode.Append(this.appendNode(node.Left, tabCount + 1, "else" ));

        if (!float.IsNaN(node.Probability))
            nodeCode.Append(tab + $"\treturn {node.Probability}f;\n".Replace(',', '.'));
        nodeCode.Append(tab + "}\n");
        return nodeCode;
    }
    private string comparativeSing(ComparisonSigns comparison)
    {
        switch (comparison)
        {
            case ComparisonSigns.Equal:
                return "=";

            case ComparisonSigns.Bigger:
                return ">";

            case ComparisonSigns.BiggerEqual:
                return ">=";

            case ComparisonSigns.Less:
                return "<";

            case ComparisonSigns.LessEqual:
                return "<=";

            default:
                return "!=";
        }
    }
}