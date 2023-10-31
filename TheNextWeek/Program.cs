using TheNextWeek.Chapters;
using TheNextWeek.Contracts.Chapters;

DateTime beginTime = DateTime.Now;

IChapter chapter = new Chapter7();

chapter.Run();

Console.WriteLine($"Total time: {DateTime.Now - beginTime}");

Console.ReadKey();