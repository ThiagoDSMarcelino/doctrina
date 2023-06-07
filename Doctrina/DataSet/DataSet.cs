using System.Collections;
using System.IO;

namespace Doctrina;

using Exceptions.DataSetExceptions;

public class DataSet<T1, T2> : IEnumerable<(T1[], T2)>
    where T1 : IConvertible
    where T2 : IConvertible
{
    public T1[][] X { get; private set; }

    public T2[] Y { get; private set; }
    
    public int Length =>
        end - start;
    
    private readonly int start;
    private readonly int end;

    private DataSet(T1[][] x, T2[] y, int start, int end)
    {
        X = x;
        Y = y;
        this.start = start;
        this.end = end;
    }

    public static DataSet<T1, T2> Load(T1[][] x, T2[] y) =>
        new (x, y, 0, x.Length);

    public static DataSet<T1, T2> Load(T1[][] x, T2[] y, int start, int end) =>
        new(x, y, start, end);

    public static DataSet<T1, T2> Load(string path, string targetLabel, char separator = ';')
    {
        var data = Open(path);

        var length = data.Count() - 1;
        var ds = new DataSet<T1, T2>(new T1[length][], new T2[length], 0, length);

        var label = data
            .First()
            .Split(separator)
            .Select((label, index) => (label, index))
            .FirstOrDefault(i => i.label == targetLabel);

        if (label is (null, int))
            throw new Exception(); // TODO

        var labelIndex = label.index;

        foreach (var (line, index) in data.Skip(1).Select((data, index) => (data, index)))
        {
            string[] lineData = line.Split(separator);
            var x = new T1[length - 1];
            int flag = 0;

            
            for (int i = 0; i < length; i++)
            {
                if (i == labelIndex)
                {
                    ds.Y[index] = ConvertType<T2>(lineData[i]);
                    flag++;
                }
                else
                    x[i - flag] = ConvertType<T1>(lineData[i]);
            }

            ds.X[index] = x;
        }

        return ds;
    }

    private static U ConvertType<U>(string val) =>
        (U)Convert.ChangeType(val, typeof(U));

    public (DataSet<T1, T2> train, DataSet<T1, T2> test) SplitTrainTest(float pct)
    {
        if (1f < pct || pct < 0f)
            throw new InvalidPercentageException();
        
        int split = (int)((1 - pct) * X.Length);
        DataSet<T1, T2>
            dsTrain = Load(X, Y, 0, split),
            dsTest = Load(X, Y, split, X.Length);

        return (dsTrain, dsTest);
    }

    private static IEnumerable<string> Open(string file)
    {
        var stream = new StreamReader(file);

        while (!stream.EndOfStream)
        {
            var data = stream.ReadLine();
            if (data is not null)
                yield return data;
        }

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