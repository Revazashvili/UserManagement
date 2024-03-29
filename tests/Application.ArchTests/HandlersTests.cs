﻿using System.Diagnostics;
using Application.Common.Wrappers;
using NetArchTest.Rules;
using Xunit;

namespace Application.ArchTests;

public class HandlersTests
{
    private static PredicateList HandlersPredicateList => Types
        .InAssembly(typeof(ServiceCollectionExtension).Assembly)
        .That()
        .ImplementInterface(typeof(IHandlerWrapper<,>))
        .And()
        .AreClasses();
    
    [Fact]
    public void AllHandlers_ShouldBe_Sealed()
    {
        var testResult = HandlersPredicateList
            .Should()
            .BeSealed()
            .GetResult();
        
        if(testResult.FailingTypeNames is not null && testResult.FailingTypeNames.Any())
            testResult.FailingTypeNames.ToList().ForEach(typeName => Debug.WriteLine(typeName));
        
        Assert.True(testResult.IsSuccessful);
    }
    
    [Fact]
    public void AllHandlers_ShouldNotBe_Public()
    {
        var testResult = HandlersPredicateList
            .Should()
            .NotBePublic()
            .GetResult();
        
        if(testResult.FailingTypeNames is not null && testResult.FailingTypeNames.Any())
            testResult.FailingTypeNames.ToList().ForEach(typeName => Debug.WriteLine(typeName));
        
        Assert.True(testResult.IsSuccessful);
    }
}