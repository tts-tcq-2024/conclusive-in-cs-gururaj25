
namespace Conclusive
{

public class ConsoleOutput : IDisposable
{
    private readonly System.IO.StringWriter stringWriter;
    private readonly System.IO.TextWriter originalOutput;

    public ConsoleOutput()
    {
        stringWriter = new System.IO.StringWriter();
        originalOutput = Console.Out;
        Console.SetOut(stringWriter);
    }

    public string GetOutput()
    {
        return stringWriter.ToString().Trim();
    }

    public void Dispose()
    {
        Console.SetOut(originalOutput);
        stringWriter.Dispose();
    }
}
}
