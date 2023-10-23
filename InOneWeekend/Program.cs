using InOneWeekend.Chapters;
using InOneWeekend.Contracts.Chapters;

namespace InOneWeekend;

internal class Program
{
    static void Main(string[] args)
    {
        _ = args;

        DateTime beginTime = DateTime.Now;

        IChapter chapter = new Chapter14();

        chapter.Run();

        Console.WriteLine($"Total time: {DateTime.Now - beginTime}");

        Console.ReadKey();
    }
}
