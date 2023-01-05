using BlazorShared.Data;
using System.Runtime.CompilerServices;
using System.Text;

namespace BlazorShared.Services;

public class CrossBridgeService
{
    private readonly Random random = new();

    public async Task RunLongProcedureOnTask()
    {
        await Task.Delay(TimeSpan.FromSeconds(3));
    }

    public void RunLongProcedure()
    {
        Thread.Sleep(TimeSpan.FromSeconds(10));
    }

    public SomeDataModel GetSomeData()
    {
        return new SomeDataModel
        {
            Text = "Hello World",
            Number = random.Next(100),
        };
    }

    public double Power(PowerModel model)
    {
        return Math.Pow(model.Value, model.Power);
    }

    public void ProduceError()
    {
        throw new Exception("Intentional exception from .Net");
    }
}
public ref struct FullNameOf<T>
{
    string _fullName;
    public FullNameOf(T _, [CallerArgumentExpression("_")] string fullName = "")
    {
        _fullName = fullName;
    }

    public static implicit operator string(FullNameOf<T> obj)
         => obj._fullName;
}

public static class FullName
{
    public static string Of<T>(T _, [CallerArgumentExpression("_")] string fullName = "")
     => fullName;
}

public static class StringExtensions
{

    /// <summary>将大驼峰命名转为小驼峰命名</summary>
    public static string ToCamelCase(this string str)
    {
        var firstChar = str[0];

        if (firstChar == char.ToLowerInvariant(firstChar))
        {
            return str;
        }

        var name = str.ToCharArray();
        name[0] = char.ToLowerInvariant(firstChar);

        return new String(name);
    }

    /// <summary>将大驼峰命名转为蛇形命名</summary>
    public static string ToSnakeCase(this string str)
    {
        var builder = new StringBuilder();
        var name = str;
        var previousUpper = false;

        for (var i = 0; i < name.Length; i++)
        {
            var c = name[i];
            if (char.IsUpper(c))
            {
                if (i > 0 && !previousUpper)
                {
                    builder.Append("_");
                }
                builder.Append(char.ToLowerInvariant(c));
                previousUpper = true;
            }
            else
            {
                builder.Append(c);
                previousUpper = false;
            }
        }
        return builder.ToString();
    }
}
