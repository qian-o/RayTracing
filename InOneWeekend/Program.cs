using InOneWeekend.Chapters;
using InOneWeekend.Contracts.Chapters;

namespace InOneWeekend;

internal class Program
{
    static void Main(string[] args)
    {
        _ = args;

        IChapter chapter = new Chapter11();

        chapter.Run();

        Console.ReadKey();
    }
}
