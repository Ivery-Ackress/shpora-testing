
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{

    [TestCase(0, 0, false, TestName = "Ctor: precision=0 недопустим")]
    [TestCase(-1, 0, false, TestName = "Ctor: precision<0 недопустим")]
    [TestCase(2, -1, false, TestName = "Ctor: scale<0 недопустим")]
    [TestCase(2, 2, false, TestName = "Ctor: scale==precision недопустим")]
    public void Ctor_InvalidArguments_Throws(int precision, int scale, bool onlyPositive)
    {
        Action act = () => new NumberValidator(precision, scale, onlyPositive);

        act.Should().Throw<ArgumentException>(
            because: $"precision={precision}, scale={scale} недопустимы");
    }

    [TestCase(17, 2, true, TestName = "Ctor: валидные аргументы не бросают")]
    public void Ctor_ValidArguments_DoesNotThrow(int precision, int scale, bool onlyPositive)
    {
        Action act = () => new NumberValidator(precision, scale, onlyPositive);

        act.Should().NotThrow(
            because: $"precision={precision}, scale={scale} допустимы");
    }

    [TestCase(17, 2, true, "123", TestName = "Valid: целое '123'")]
    [TestCase(17, 2, true, "0.0", TestName = "Valid: с дробью '0.0'")]
    [TestCase(17, 2, true, "1,23", TestName = "Valid: запятая '1,23'")]
    [TestCase(17, 2, true, "+1.23", TestName = "Valid: явный плюс '+1.23'")]
    [TestCase(17, 2, false, "-1.23", TestName = "Valid: отрицательное при onlyPositive=false")]
    [TestCase(5, 2, true, "123.45", TestName = "Valid: ровно на границе precision")]
    public void IsValidNumber_ValidValues_ReturnsTrue(
        int precision, int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);

        validator.IsValidNumber(value).Should().BeTrue(
            because: $"'{value}' должен быть валиден при precision={precision}, scale={scale}, onlyPositive={onlyPositive}");
    }

   [TestCase(17, 2, true, null, TestName = "Invalid: null")]
    [TestCase(17, 2, true, "", TestName = "Invalid: пустая строка")]
    [TestCase(17, 2, true, "a.sd", TestName = "Invalid: не число 'a.sd'")]
    [TestCase(3, 2, true, "1234", TestName = "Invalid: превышение precision '1234'")]
    [TestCase(17, 2, true, "0.000", TestName = "Invalid: превышение scale '0.000'")]
    [TestCase(17, 2, true, "-1.23", TestName = "Invalid: отрицательное при onlyPositive=true")]
    public void IsValidNumber_InvalidValues_ReturnsFalse(
        int precision, int scale, bool onlyPositive, string value)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);

        validator.IsValidNumber(value).Should().BeFalse(
            because: $"'{value}' не должен быть валиден при precision={precision}, scale={scale}, onlyPositive={onlyPositive}");
    }

    //[Test]
    //public void Test()
    //{
    //    Assert.Throws<ArgumentException>(() => new NumberValidator(-1, 2, true));
    //    Assert.DoesNotThrow(() => new NumberValidator(1, 0, true));
    //    Assert.Throws<ArgumentException>(() => new NumberValidator(-1, 2, false));
    //    Assert.DoesNotThrow(() => new NumberValidator(1, 0, true));

    //    ClassicAssert.IsTrue(new NumberValidator(17, 2, true).IsValidNumber("0.0"));
    //    ClassicAssert.IsTrue(new NumberValidator(17, 2, true).IsValidNumber("0"));
    //    ClassicAssert.IsTrue(new NumberValidator(17, 2, true).IsValidNumber("0.0"));
    //    ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("00.00"));
    //    ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("-0.00"));
    //    ClassicAssert.IsTrue(new NumberValidator(17, 2, true).IsValidNumber("0.0"));
    //    ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("+0.00"));
    //    ClassicAssert.IsTrue(new NumberValidator(4, 2, true).IsValidNumber("+1.23"));
    //    ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("+1.23"));
    //    ClassicAssert.IsFalse(new NumberValidator(17, 2, true).IsValidNumber("0.000"));
    //    ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("-1.23"));
    //    ClassicAssert.IsFalse(new NumberValidator(3, 2, true).IsValidNumber("a.sd"));
    //}
}