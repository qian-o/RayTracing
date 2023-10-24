using InOneWeekend.Chapters;
using InOneWeekend.Contracts.Chapters;

DateTime beginTime = DateTime.Now;

IChapter chapter = new Chapter14();

chapter.Run();

Console.WriteLine($"Total time: {DateTime.Now - beginTime}");

Console.ReadKey();
