namespace Doctrina;

using Exceptions.DataSetExceptions;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System;

public class DataSet<T1, T2> : IEnumerable
    where T1 : unmanaged, IComparable
    where T2 : unmanaged, IComparable
{
    public DataType<T1>[][] X { get; private set; }
    public DataType<T2>[] Y { get; private set; }
    public int Length => end - start;
    private int start;
    private int end;

    private DataSet() { }
    public static DataSet<T1, T2> Load(DataType<T1>[][] x, DataType<T2>[] y, int start = 0, int end = -1)
    {
        var ds = new DataSet<T1, T2>();
        ds.X = x;
        ds.Y = y;
        ds.start = start;
        ds.end = end < 0 ? x.Length : end;

        return ds;
    }
    
    public static DataSet<T1, T2> Load(string path, char separator, string targetLabel, int start = 0, int end = -1)
    {
        var ds = new DataSet<T1, T2>();
        var data = DataSet<T1, T2>.open(path);
        
        ds.start = start;
        ds.end = end < 0 ? data.Count() : end;
        ds.X = new DataType<T1>[ds.Length][];
        ds.Y = new DataType<T2>[ds.Length];

        int index = 0,
            labelIndex = data
                .First()
                .Split(separator)
                .Select((item, index) => (item, index))
                .First(i => i.item == targetLabel)
                .index;
            
        foreach (var line in data.Skip(1))
        {
            string[] lineData = line.Split(separator);
            var x = new DataType<T1>[lineData.Length - 1];
            int flag = 0;

            for (int i = 0; i < lineData.Length; i++)
            {
                if (i == labelIndex)
                {
                    ds.Y[index] = (DataType<T2>)Convert.ChangeType(lineData[i], typeof(T2));
                    flag++;
                }
                else
                {
                    x[i - flag] = (DataType<T1>)Convert.ChangeType(lineData[i], typeof(T1));
                }
            }

            ds.X[index] = x;
            index++;
        }

        return ds;
    }

    public static DataSet<T1, T2> Load(string path, char separator, int targetIndex, int start = 0, int end = -1)
    {
        var ds = new DataSet<T1, T2>();
        var data = DataSet<T1, T2>.open(path);
        
        ds.start = start;
        ds.end = end < 0 ? data.Count() : end;
        ds.X = new DataType<T1>[ds.Length][];
        ds.Y = new DataType<T2>[ds.Length];
        int index = 0;

        foreach (var line in data)
        {
            string[] lineData = line.Split(separator);
            var x = new DataType<T1>[lineData.Length - 1];
            int flag = 0;

            for (int i = 0; i < lineData.Length; i++)
            {
                if (i == targetIndex)
                {
                    ds.Y[index] = (DataType<T2>)float.Parse(lineData[i]);
                    flag++;
                }
                else
                {
                    x[i - flag] = (DataType<T1>)float.Parse(lineData[i]);
                }
            }
            
            ds.X[index] = x;
            index++;
        }

        return ds;
    }

    public (DataSet<T1, T2> train, DataSet<T1, T2> test) SplitTrainTest(float pct)
    {
        if (pct < 0f || pct > 1f)
            throw new InvalidPercentageException();
        
        int split = (int)((1 - pct) * this.X.Length);
        DataSet<T1, T2>
            dsTrain = DataSet<T1, T2>.Load(this.X, this.Y, 0, split),
            dsTest = DataSet<T1, T2>.Load(this.X, this.Y, split);

        return (dsTrain, dsTest);
    }

    private static IEnumerable<string> open(string file)
    {
        var stream = new StreamReader(file);

        while (!stream.EndOfStream)
            yield return stream.ReadLine();

        stream.Close();
    }
    
    public IEnumerator<(DataType<T1>[], DataType<T2>)> GetEnumerator()
    {
        for (int i = start; i < end; i++)
            yield return (X[i], Y[i]);
    }

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}