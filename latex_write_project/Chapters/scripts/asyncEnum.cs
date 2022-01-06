static async Task Main(string[] args)
{
    Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: Start");
    await foreach (var item in FetchItems())
    {
        Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {item}");
    }
    Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: End");
}

static async IAsyncEnumerable<int> FetchItems()
{
    for (int i = 1; i <= 10; i++)
    {
        await Task.Delay(1000);
        yield return i;
    }
}

//Output:
//01:00:01: Start
//01:00:02: 1
//01:00:03: 2
//01:00:04: 3
//01:00:05: 4
//.....
//01:00:05: 9
//01:00:05: End


