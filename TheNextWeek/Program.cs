using TheNextWeek.Chapters;
using TheNextWeek.Contracts.Chapters;

DateTime beginTime = DateTime.Now;

IChapter chapter = new Chapter9();

chapter.Run();

Console.WriteLine($"Total time: {DateTime.Now - beginTime}");

Console.ReadKey();