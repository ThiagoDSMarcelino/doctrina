using System;

namespace Doctrina;

public class DataType<T> : IComparable
    where T: IComparable
{
    public T Value { get; private set; }

    public DataType(T value) =>
        this.Value = value;

    // Sum
    public static DataType<T> operator +(DataType<T> a, DataType<T> b) =>
        new DataType<T>((dynamic)a.Value + (dynamic)b.Value);

    // Sub
    public static DataType<T> operator -(DataType<T> a, DataType<T> b) =>
        new DataType<T>((dynamic)a.Value - (dynamic)b.Value);

    // Mul
    public static DataType<T> operator *(DataType<T> a, DataType<T> b) =>
        new DataType<T>((dynamic)a.Value * (dynamic)b.Value);

    // Div
    public static DataType<T> operator /(DataType<T> a, DataType<T> b) =>
        new DataType<T>((dynamic)a.Value / (dynamic)b.Value);
    public static DataType<T> operator /(DataType<T> a, float b) =>
        new DataType<T>((dynamic)a.Value / b);

    // Equal
    public static bool operator ==(DataType<T> a, DataType<T> b) =>
        (dynamic)a.Value == (dynamic)b.Value;
    public static bool operator ==(DataType<T> a, float b) =>
        (dynamic)a.Value == b;

    // Different
    public static bool operator !=(DataType<T> a, DataType<T> b) =>
        (dynamic)a.Value != (dynamic)b.Value;
    public static bool operator !=(DataType<T> a, float b) =>
        (dynamic)a.Value != b;

    // Bigger
    public static bool operator >(DataType<T> a, DataType<T> b)
    {
        if (a is null || b is null)
            return false;
        return (dynamic)a.Value > (dynamic)b.Value;
    }

    // Lower
    public static bool operator <(DataType<T> a, DataType<T> b) =>
        (dynamic)a.Value < (dynamic)b.Value;
    
    // Bigger or Equal
    public static bool operator >=(DataType<T> a, DataType<T> b) =>
        (dynamic)a.Value >= (dynamic)b.Value;

    // Lower or Equal
    public static bool operator <=(DataType<T> a, DataType<T> b) =>
        (dynamic)a.Value <= (dynamic)b.Value;

    // Cast
    public static explicit operator DataType<T>(float value) =>
        new DataType<T>((T)Convert.ChangeType(value, typeof(T)));

    // Overrides
    public override string ToString() =>
        this.Value.ToString();

    public override bool Equals(object obj) =>
        base.Equals(obj);

    public override int GetHashCode() =>
        base.GetHashCode();

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;

        DataType<T> otherDataType = obj as DataType<T>;

        // if (otherDataType != null)
            return this.Value.CompareTo(otherDataType.Value);
        
        // throw new ArgumentException("Object is not a DataType");
    }
}
