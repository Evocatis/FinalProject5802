public class DataDecoratorsFactory
{
    public static IDataObfuscation ApplyDecorators(IDataObfuscation baseStrategy, List<string> decorators)
    {
        foreach (var decorator in decorators)
        {
            baseStrategy = decorator switch
            {
                "html" => new HtmlWrapperDecorator(baseStrategy),
                "disclaimer" => new DisclaimerDecorator(baseStrategy),
                _ => baseStrategy // Unknown keys are ignored
            };
        }

        return baseStrategy;
    }
}