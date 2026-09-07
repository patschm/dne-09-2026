using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnvConfSec;

internal class Executor
{
    private readonly ICalculator _calculator; // NEEE! = new Subtracter();

    public Executor([FromKeyedServices("sub")]ICalculator calculator)
    {
        _calculator = calculator;
    }

    public int Calculate(int a, int b)
    {
        return _calculator.Bereken(a, b);
    }
}
