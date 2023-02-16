namespace Doctrina;

public class DataType<T>
{
    private T value;
    public T Value
    {
        get { return value; }
        set { Value = value; }
    }

    public DataType(T newValue) =>
        this.value = newValue;

    public static DataType<T> operator +(DataType<T> a, DataType<T> b)
    {
        dynamic x = a.Value;
        dynamic y = b.Value;
        dynamic result = x + y;
        return new DataType<T>(result);
    }

    public static DataType<T> operator -(DataType<T> a, DataType<T> b)
    {
        dynamic x = a.Value;
        dynamic y = b.Value;
        dynamic result = x - y;
        return new DataType<T>(result);
    }

    public static DataType<T> operator *(DataType<T> a, DataType<T> b)
    {
        dynamic x = a.Value;
        dynamic y = b.Value;
        dynamic result = x * y;
        return new DataType<T>(result);
    }

    public static DataType<T> operator /(DataType<T> a, DataType<T> b)
    {
        dynamic x = a.Value;
        dynamic y = b.Value;
        dynamic result = x / y;
        return new DataType<T>(result);
    }

    public static bool operator ==(DataType<T> a, DataType<T> b)
    {
        dynamic x = a.Value;
        dynamic y = b.Value;
        return x == y;
    }

    public static bool operator !=(DataType<T> a, DataType<T> b)
    {
        dynamic x = a.Value;
        dynamic y = b.Value;
        return x != y;
    }

    public static bool operator >(DataType<T> a, DataType<T> b)
    {
        dynamic x = a.Value;
        dynamic y = b.Value;
        return x > y;
    }

    public static bool operator <(DataType<T> a, DataType<T> b)
    {
        dynamic x = a.Value;
        dynamic y = b.Value;
        return x < y;
    }

    public override string ToString() =>
        this.value.ToString();
}
