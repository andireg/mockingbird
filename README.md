# mockingbird


## example

Let's say, you want to test a class `ServiceUnderTest`, but you are too lazy (like me) to mock the required interface `IRequiredService`.


```csharp
public class ServiceUnderTest
{
    private readonly IRequiredService requiredService;

    public ServiceUnderTest(IRequiredService requiredService)
    {
        this.requiredService = requiredService;
    }

    public string MethodUnderTest(string input) => requiredService.TestMethod(input);
}
```

This library can create all required referenced instances for you. As an example check this unit test. 

```csharp
public class ServiceUnderTestTests
{
    [Fact]
    public void SampleUnitTest()
    {
        using IMockContext<ServiceUnderTest> context = MockContextFactory.Start<ServiceUnderTest>();
        string actual = context.Instance.MethodUnderTest("INPUT");
        Assert.Equal("OUTPUT", actual);
        context.Verify();
    }
}
```

By running the test, two `JSON` files will be created: a **setup** and a **snapshot** file. 

```json
[
  {
    "TypeName": "Sample.IRequiredService",
    "Invocations": [
      {
        "InvocationName": "TestMethod",
        "Arguments": {
          "input": "INPUT"
        },
        "Result": "OUTPUT",
        "Number": 1
      }
    ]
  }
]
```