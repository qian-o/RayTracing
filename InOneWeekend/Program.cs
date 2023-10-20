using InOneWeekend.Chapters;
using InOneWeekend.Contracts.Chapters;

namespace InOneWeekend;

internal class Program
{
    static void Main(string[] args)
    {
        _ = args;

        IChapter chapter = new Chapter7();

        chapter.Run();

        Console.ReadKey();
    }
}
