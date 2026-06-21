using System.Globalization;

namespace Praktyka3;

/// <summary>
/// Завдання 3.2. Комплексне число.
/// Конструктори, друк, ToString, додавання з цілим і комплексним,
/// множення, перевантаження оператора +.
/// </summary>
public class Complex
{
    public double Re { get; set; }
    public double Im { get; set; }

    public Complex() : this(0, 0) { }
    public Complex(double re) : this(re, 0) { }
    public Complex(double re, double im) { Re = re; Im = im; }

    /// <summary>Додавання з цілим числом.</summary>
    public Complex Add(int number) => new(Re + number, Im);

    /// <summary>Додавання з іншим комплексним числом.</summary>
    public Complex Add(Complex other) => new(Re + other.Re, Im + other.Im);

    /// <summary>Множення на інше комплексне число: (a+bi)(c+di) = (ac-bd)+(ad+bc)i.</summary>
    public Complex Multiply(Complex other) =>
        new(Re * other.Re - Im * other.Im, Re * other.Im + Im * other.Re);

    /// <summary>Перевантаження оператора + для двох комплексних чисел.</summary>
    public static Complex operator +(Complex a, Complex b) => a.Add(b);

    /// <summary>Друк у консоль.</summary>
    public void Print() => Console.WriteLine(ToString());

    public override string ToString()
    {
        string re = Re.ToString(CultureInfo.InvariantCulture);
        string im = Math.Abs(Im).ToString(CultureInfo.InvariantCulture);
        string sign = Im >= 0 ? "+" : "-";
        return $"{re} {sign} {im}i";
    }
}
