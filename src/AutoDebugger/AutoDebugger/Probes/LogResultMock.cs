namespace AutoDebugger.Probes;

internal class LogResultMock : ILogResult
{
    public Task<string?> GetLogResult()
    {
        return Task.FromResult<string?>(
        @"{
  ""snapshot"": {
    ""stack"": [
      {
        ""fileName"": ""CourseService.cs"",
        ""function"": ""CourseService.UnassignRoomFromCourse"",
        ""lineNumber"": 30
      },
    ],
    ""captures"": {
      ""lines"": {
        ""30"": {
          ""arguments"": {
            ""id"": {
              ""type"": ""System.String"",
              ""value"": ""abc""
            },
            
          },
          ""locals"": {
            ""course"": {
              ""isNull"": true,
              ""type"": ""WebService.Models.Course""
            },
            ""throwable"": {
                ""type"": ""System.InvalidOperationException"",
                ""value"": ""No element found in the collection""
            }
          },
          
        }
      }
    },
    ""language"": ""dotnet"",
    ""id"": ""fc555694-9a33-4767-95b1-53be479ff58c"",
    ""probe"": {
      ""location"": {
        ""file"": ""dotnet/Services/CourseService.cs"",
        ""method"": ""UnassignRoomFromCourse"",
        ""lines"": [
          ""30""
        ],
        ""type"": ""CourseService""
      },
      ""id"": ""c2a3fd12-9c9b-48ab-85f3-7cfaa6bc31f6"",
      ""version"": 13
    },
    ""timestamp"": 1702261465498
  }
}");
    }
}