namespace TestingTechniques;

public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }

    public int Subtract(int a, int b)
    {
        return a - b;
    }

    public int Multiply(int a, int b)
    {
        return a * b;
    }

    public float Divide(int a, int b)
    {
        EnsureThatDivisorIsNotZero(b);

        return a / b;
    }

    private static void EnsureThatDivisorIsNotZero(float value)
    {
        if (value == 0)
        {
            throw new DivideByZeroException();
        }
    }
}

