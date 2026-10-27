using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.ObjectComparison;
public class ObjectComparison
{
    [Test]
    [Description("Проверка текущего царя")]
    [Category("ToRefactor")]
    public void CheckCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        actualTsar.Should().BeEquivalentTo(expectedTsar, 
             options =>
                 options.Excluding(ctx =>
                     ctx.Path == "Id" || ctx.Path.EndsWith(".Id")));
        // Перепишите код на использование Fluent Assertions.
        //ClassicAssert.AreEqual(actualTsar.Name, expectedTsar.Name);
        //ClassicAssert.AreEqual(actualTsar.Age, expectedTsar.Age);
        //ClassicAssert.AreEqual(actualTsar.Height, expectedTsar.Height);
        //ClassicAssert.AreEqual(actualTsar.Weight, expectedTsar.Weight);

        //ClassicAssert.AreEqual(expectedTsar.Parent!.Name, actualTsar.Parent!.Name);
        //ClassicAssert.AreEqual(expectedTsar.Parent.Age, actualTsar.Parent.Age);
        //ClassicAssert.AreEqual(expectedTsar.Parent.Height, actualTsar.Parent.Height);
        //ClassicAssert.AreEqual(expectedTsar.Parent.Parent, actualTsar.Parent.Parent);
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));
        //1. При изминении Person придется лезть и переписывать AreEqual
        //2. Объемно
        //3. Приходится вчитываться в реализацчию
        //Мой метод лучше, тк:
        //1. Он автоматически проверяет все поля за исключением Id, тк оно уникально у всех по определению,
        //что позволяет его не исправлять при изменении Person
        //2. Он легко читаемый
        //3. В случае ошибки в тесте, он покажет где именно произошла ошибка, а не просто что тест упал
    }

    private bool AreEqual(Person? actual, Person? expected)
    {
        if (actual == expected) return true;
        if (actual == null || expected == null) return false;
        return
            actual.Name == expected.Name
            && actual.Age == expected.Age
            && actual.Height == expected.Height
            && actual.Weight == expected.Weight
            && AreEqual(actual.Parent, expected.Parent);
    }
}
