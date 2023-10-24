using TheNextWeek.Contracts.Chapters;

namespace TheNextWeek;

internal class Program
{
    static void Main(string[] args)
    {
        _ = args;

        DateTime beginTime = DateTime.Now;

        IChapter chapter = null;

        chapter.Run();

        Console.WriteLine($"Total time: {DateTime.Now - beginTime}");

        Console.ReadKey();
    }
}
