public abstract class ObfuscationDecorator : IDataObfuscation
{
    protected readonly IDataObfuscation Inner;

    protected ObfuscationDecorator(IDataObfuscation inner)
    {
        Inner = inner;
    }

    public abstract string Obfuscate();
}

public class HtmlWrapperDecorator : ObfuscationDecorator
{
    public HtmlWrapperDecorator(IDataObfuscation inner) : base(inner) { }

    public override string Obfuscate()
    {
        return $"<div class='user'>{Inner.Obfuscate()}</div>";
    }
}

public class DisclaimerDecorator : ObfuscationDecorator
{
    public DisclaimerDecorator(IDataObfuscation inner) : base(inner) { }

    public override string Obfuscate()
    {
        return $"{Inner.Obfuscate()}\n\nNote: This information is partially redacted.";
    }
}