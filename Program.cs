using System.Reflection;

var provider = new DataProvider();
InvokeAndDisplayAttribute(provider);


static void InvokeAndDisplayAttribute(object obj)
{
    var methods = obj.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

    foreach (var method in methods)
    {
        var attribute = method.GetCustomAttribute<TypeAttribute>();
        if (attribute != null)
        {
            Console.WriteLine($"Invoking Method '{method.Name}' expecting type '{attribute.ParamType.Name}':");

            var result = method.Invoke(obj, null);

            Console.WriteLine($"Result: {result} (Actual type: {result.GetType().Name})");
            Console.WriteLine(result.GetType() == attribute.ParamType
                ? "✅ Type matches the attribute specification."
                : "❌ Type does NOT match the attribute specification.");

            Console.WriteLine();
        }
    }
}

public class TypeAttribute : Attribute
{
    public Type ParamType { get; }

    public TypeAttribute(Type t)
    {
        ParamType = t;
    }
}

public class DataProvider
{
    [TypeAttribute(typeof(string))]
    public object ProvideString() => "Hello, World!";

    [TypeAttribute(typeof(int))]
    public object ProvideInt() => 42;
}

