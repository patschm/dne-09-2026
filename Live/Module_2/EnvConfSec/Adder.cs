using System;
using System.Collections.Generic;
using System.Text;

namespace EnvConfSec;

internal class Adder : ICalculator
{
    private int _teller = 0;
    public int Bereken(int a, int b)
    {
        Console.WriteLine(++_teller);
        return a + b;
    }
}
