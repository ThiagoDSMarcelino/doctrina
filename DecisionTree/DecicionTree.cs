using System.Text;
using System.Linq;
using System.IO;
using System;

namespace Doctrina.DecisionTreeLib;

using Exceptions.DecisionTreeExceptions;
using Core;

/// <summary>
/// Represents a Classification Decision Tree (DT)
/// </summary>
public class DecisionTree<T1, T2>
    where T1 : unmanaged, IComparable
    where T2 : unmanaged, IComparable
{
    /// <summary>
    /// The first node of the tree
    /// </summary>
    public Node<T1, T2> Root { get; private set; }
    private int dataLength { get; set; }

    /// <summary>
    /// Generates the nodes necessary for the DT to be able to classify data
    /// </summary>
    /// <param name="ds">DataSet used to train the DT</param>
    /// <param name="minSample">Minimum number of lines in X to create a new node</param>
    /// <param name="maxDepth">Maximum sequential nodes that can be created</param>
    public void Fit(DataSet<T1, T2> ds, int minSample, int maxDepth)
    {
        this.Root = new Node<T1, T2>();
        this.dataLength = ds.X[0].Length;
        this.Root.Epoch(ds, minSample, maxDepth);
    }

    /// <summary>
    /// Check each node with the values
    /// </summary>
    /// <param name="data">Data to be checked</param>
    /// <returns>Probability of being true according to the data</returns>
    public double Choose(DataType<T1>[] data)
    {
        if (this.Root is null)
            throw new NecessaryTrainingException();

        if (this.dataLength != data.Length)
            throw new InvalidParameterSizeException();
        
        double result = this.CheckNode(this.Root, data);
        
        return result;
    }
    private double CheckNode(Node<T1, T2> node, DataType<T1>[] data)
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

    /// <summary>
    /// Save the decision tree in a C# file
    /// </summary>
    /// <param name="path">Path to save the file with the file name, but without the extension (.cs)</param>
    /// <param name="className">Name of the class that the method will be part of</param>
    /// <param name="methodName">Method name</param>
    public void Save(string path, string className = "DefaultModel", string methodName = "Choose")
    {
        path += ".cs";
        
        StringBuilder func = new StringBuilder($"public partial class {className}" + "\n{\n");
        func.Append($"\tpublic float {methodName}(int[] data)\n" + "\t{\n");
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

        if (!double.IsNaN(node.Probability))
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