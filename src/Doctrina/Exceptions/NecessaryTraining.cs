namespace Doctrina.Exceptions;

public class NecessaryTrainingException : Exception
{
    public NecessaryTrainingException() { }
    public NecessaryTrainingException(string message) : base(message) { }
    public NecessaryTrainingException(string message, Exception inner) : base(message, inner) { }
    protected NecessaryTrainingException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
        : base(info, context) { }
    public override string Message
        => "It is necessary to train the decision tree before using it.";
}