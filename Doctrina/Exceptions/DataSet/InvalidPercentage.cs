namespace Doctrina.Exceptions.DataSetExceptions;

public class InvalidPercentageException : Exception
{
    public InvalidPercentageException() { }
    public InvalidPercentageException(string message) : base(message) { }
    public InvalidPercentageException(string message, Exception inner) : base(message, inner) { }
    protected InvalidPercentageException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    
    public override string Message =>
        "Percentage must be lower than 1 and bigger than 0";
}