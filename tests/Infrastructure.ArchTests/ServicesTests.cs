using System.Diagnostics;
using NetArchTest.Rules;
using Xunit;

namespace Infrastructure.ArchTests;

public class ServicesTests
{
    private static readonly PredicateList ServicesPredicateList = Types
        .InAssembly(typeof(ServiceCollectionExtension).Assembly)
        .That().ResideInNamespace("Infrastructure.Services")
        .And()
        .AreClasses();
    
    [Fact]
    public void Services_ShouldBe_Sealed()
    {
        var testResult = ServicesPredicateList
            .Should()
            .BeSealed()
            .GetResult();
        
        if(testResult.FailingTypeNames is not null && testResult.FailingTypeNames.Any())
            testResult.FailingTypeNames.ToList().ForEach(typeName => Debug.WriteLine(typeName));
        
        Assert.True(testResult.IsSuccessful);
    }
    
    [Fact]
    public void Services_ShouldNotBe_Public()
    {
        var testResult = ServicesPredicateList
            .ShouldNot()
            .BePublic()
            .GetResult();
        
        if(testResult.FailingTypeNames is not null && testResult.FailingTypeNames.Any())
            testResult.FailingTypeNames.ToList().ForEach(typeName => Debug.WriteLine(typeName));
        
        Assert.True(testResult.IsSuccessful);
    }
}