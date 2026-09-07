using System;
using System.Collections.Generic;
using System.Text;

namespace EnvConfSec;

internal class Subtracter : ICalculator
{
    public int Bereken(int a, int b)
    {
        return a - b;
    }
}
