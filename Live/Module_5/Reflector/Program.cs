//using SomeLibrary;

using System.Reflection;

namespace Reflector;

internal class Program
{
    static void Main(string[] args)
    {
        //Person person = new Person { FirstName = "Jan", LastName = "Peters", Age = 45 };
        //person.Introduce();
        //DeOnderWereldIn();
        HetBetereHackwerk();

    }

    private static void HetBetereHackwerk()
    {
        var asm = Assembly.LoadFile(@"D:\DNE\dne-09-2026\Live\Module_5\SomeLib.dll\SomeLibrary.dll");

        var type = asm.GetType("SomeLibrary.Person");
        Console.WriteLine(type.Name);
        object? p1 = Activator.CreateInstance(type);

        var pFirst = type.GetProperty("FirstName");
        var pLast = type.GetProperty("LastName");
        var pAge = type.GetProperty("Age");

        pFirst.SetValue(p1, "Jan");
        pLast.SetValue(p1, "Pieters");
        pAge.SetValue(p1, 47);

        var page = type.GetField("_age", BindingFlags.Instance | BindingFlags.NonPublic);
        page.SetValue(p1, -47);
        var mIntro = type.GetMethod("Introduce");
        mIntro.Invoke(p1, []);

        dynamic p2 = Activator.CreateInstance(type);
        p2.FirstName = "Marieke";
        p2.LastName = "Hendriks";
        //p2.Age = 23;
        p2._age = 23;
        p2.Introduce();


    }

    private static void DeOnderWereldIn()
    {
        var asm = Assembly.LoadFile(@"D:\DNE\dne-09-2026\Live\Module_5\SomeLib.dll\SomeLibrary.dll");
        Console.WriteLine(asm.FullName);
        HaalTypesOp(asm);
    }

    private static void HaalTypesOp(Assembly asm)
    {
        foreach(Type tp in asm.GetTypes())
        {
            Console.WriteLine(tp.FullName);
            foreach(Type tp2 in tp.GetInterfaces())
            {
                Console.WriteLine($"Implements {tp2.FullName}");
            }

            foreach(var member  in tp.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                Console.WriteLine(member.Name);
                //Console.WriteLine($"{member.FieldType} {member.Name}");
            }
            Console.WriteLine(new string('=', 80));
        }
    }
}
