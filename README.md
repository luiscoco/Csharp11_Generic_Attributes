# C# 11 Generic Attributes

## 1. What are attributes?

In C#, **attributes** allow you to add metadata (extra information) to your code.

They don't affect how your code runs directly but give additional context or instructions that tools and frameworks can read.

Previously, creating an attribute that stores a **System.Type** required you to do this:

**Before C# 11**:

```csharp
public class TypeAttribute : Attribute
{
   public TypeAttribute(Type t) => ParamType = t;

   public Type ParamType { get; }
}
```

```csharp
// Usage:
[TypeAttribute(typeof(string))]
public string Method() => default;
```

Here, notice that you need to explicitly pass the type using the typeof keyword, which is slightly verbose and less elegant.

### 1.1. Attributes with type parameters (pre-C# 11 syntax)

**Step 1: Create your TypeAttribute class (pre-C# 11)**

```csharp
using System;

public class TypeAttribute : Attribute
{
    public Type ParamType { get; }

    public TypeAttribute(Type t)
    {
        ParamType = t;
    }
}
```

**Step 2: Apply your attribute to a method in a class**

```csharp
public class DataProvider
{
    [TypeAttribute(typeof(string))]
    public object ProvideString() => "Hello, World!";

    [TypeAttribute(typeof(int))]
    public object ProvideInt() => 42;
}
```

In this example, we have methods annotated explicitly with types using the old typeof approach.

**Step 3: Use reflection to read attributes at runtime (Console App)**

Your console app (Program.cs) could look like this:

```csharp
using System;
using System.Reflection;

class Program
{
    static void Main()
    {
        var provider = new DataProvider();
        InvokeAndDisplayAttribute(provider);
    }

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
}
```

**Run the application**

When you run the application, you'll see:

```
Invoking Method 'ProvideString' expecting type 'String':
Result: Hello, World! (Actual type: String)
Type matches the attribute specification.
```

```
Invoking Method 'ProvideInt' expecting type 'Int32':
Result: 42 (Actual type: Int32)
Type matches the attribute specification.
```

**Explanation of the example**:

We defined a custom attribute (TypeAttribute) that takes a Type parameter using the traditional (typeof) syntax (pre-C# 11).

We applied it explicitly to methods to describe their expected return types.

Using reflection, we retrieved the attribute metadata at runtime and verified if the method result matches the declared type in the attribute.

This demonstrates clearly how the pre-C# 11 syntax works practically in real-world C# applications.
