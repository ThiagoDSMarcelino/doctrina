using System.Collections;
using System.IO;

namespace Doctrina;

using Exceptions.DataSetExceptions;

public class DataSet<T1, T2> : IEnumerable<(T1[], T2)>
    where T1 : unmanaged
    where T2 : unmanaged
{
    public T1[][] X { get; private set; }
    public T2[] Y { get; private set; }
    public int Length => end - start;
    private int start;
    private int end;

    private DataSet() { }
    public static DataSet<T1, T2> Load(T1[][] x, T2[] y, int start = 0, int end = -1)
    {
        var ds = new DataSet<T1, T2>
        {
            X = x,
            Y = y,
            start = start,
            end = end < 0 ? x.Length : end
        };

        return ds;
    }
    
    public static DataSet<T1, T2> Load(string path, char separator, string targetLabel, int start = 0, int end = -1)
    {
        var ds = new DataSet<T1, T2>();
        var data = open(path);
        
        ds.start = start;
        ds.end = end < 0 ? data.Count() - 1 : end;
        ds.X = new T1[ds.Length - 1][];
        ds.Y = new T2[ds.Length - 1];

        int
            index = 0,
            labelIndex = data
                .First()
                .Split(separator)
                .Select((item, index) => (item, index))
                .First(i => i.item == targetLabel)
                .index;
            
        foreach (var line in data.Skip(1))
        {
            string[] lineData = line.Split(separator);
            var x = new T1[lineData.Length - 1];
            int flag = 0;

            for (int i = 0; i < lineData.Length; i++)
            {
                if (i == labelIndex)
                {
                    ds.Y[index] = (T2)Convert.ChangeType(lineData[i], typeof(T2));
                    flag++;
                }
                else
                {
                    x[i - flag] = (T1)Convert.ChangeType(lineData[i], typeof(T1));
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
        var data = open(path);
        
        ds.start = start;
        ds.end = end < 0 ? data.Count() - 1 : end;
        ds.X = new T1[ds.Length - 1][];
        ds.Y = new T2[ds.Length - 1];

        int index = 0;
            
        foreach (var line in data)
        {
            string[] lineData = line.Split(separator);
            var x = new T1[lineData.Length - 1];
            int flag = 0;

            for (int i = 0; i < lineData.Length; i++)
            {
                if (i == targetIndex)
                {
                    ds.Y[index] = (T2)Convert.ChangeType(lineData[i], typeof(T2));
                    flag++;
                }
                else
                {
                    x[i - flag] = (T1)Convert.ChangeType(lineData[i], typeof(T1));
                }
            }

            ds.X[index] = x;
            index++;
        }

        return ds;
    }

    public (DataSet<T1, T2> train, DataSet<T1, T2> test) SplitTrainTest(float pct)
    {
        if (1f < pct || pct < 0f)
            throw new InvalidPercentageException();
        
        int split = (int)((1 - pct) * X.Length);
        DataSet<T1, T2>
            dsTrain = Load(X, Y, 0, split),
            dsTest = Load(X, Y, split);

        return (dsTrain, dsTest);
    }

    private static IEnumerable<string> open(string file)
    {
        var stream = new StreamReader(file);

        while (!stream.EndOfStream)
            yield return stream.ReadLine();

        stream.Close();
    }
    
    public IEnumerator<(T1[], T2)> GetEnumerator()
    {
        for (int i = start; i < end; i++)
            yield return (X[i], Y[i]);
    }

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}