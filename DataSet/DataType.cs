namespace Doctrina;

using System;

public class DataType<T>
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
    public static bool operator >(DataType<T> a, DataType<T> b) =>
        (dynamic)a.Value > (dynamic)b.Value;

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
    public static implicit operator DataType<T>(float value) =>
        new DataType<T>((T)Convert.ChangeType(value, typeof(T)));  

    // Overrides
    public override string ToString() =>
        this.Value.ToString();

    public override bool Equals(object obj) =>
        base.Equals(obj);

    public override int GetHashCode() =>
        base.GetHashCode();
}
