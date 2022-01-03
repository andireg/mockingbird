# mockingbird

This library should help you to test easily methods without writing all the mocks yourself.

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

The **snapshot** file will be saved on every test run.
The **setup** file will be created with the first test run. After you can modify it by setting the `Result`s of the invocations.

## Internal factories

There are several build-in object factories:

## DatabaseFactory

Used to handle `IDbConnection` instances.

### Example

The invocation name is the query and the arguments the parameters.
The result is an array of array. The first "row" is the name of the column, the second is the date type.

```json
[
  {
    "TypeName": "System.Data.IDbConnection",
    "Invocations": [
      {
        "InvocationName": "SELECT * FROM dbo.FooBar WHERE text = @input",
        "Arguments": {
          "input": "KEY"
        },
        "Result": [
          [
            [ "Text", "Date", "Value", "Boolean" ],
            [ "string", "DateTime", "Decimal", "bool" ],
            [ "FooBar", "2021-01-01", 13.24, true ],
            [ "FooBar2", "2022-01-01", null, false ]
          ]
        ],
        "Number": 1
      }
    ]
  }
]
```
    
## HttpClientFactory

Used to handle `HttpClient` instances.

### Example

```json
[
  {
    "TypeName": "System.Net.Http.HttpClient",
    "Invocations": [
      {
        "InvocationName": "POST https://google.com/",
        "Arguments": {
          "Content": "Key=Value",
          "ContentType": "application/x-www-form-urlencoded"
        },
        "Result": {
          "Status": 200,
          "Content": "true",
          "ContentType": "application/json"
        },
        "Number": 1
      }
    ]
  }
]
```

## MoqFactory

Used to handle interface and abstract class instances.

## ClassFactory

Used to handle non-abstract class instances.

## DefinedImplementationFactory

Internally used to handle defined callback instances.

## ChainedFactory

Internally used to chain multiple factories.