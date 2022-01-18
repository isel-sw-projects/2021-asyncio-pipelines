static async Task Main(string[] args)
{
    Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: Start");
    IAsyncEnumerable<int> enumerable = FetchItems(1000);
    int i = 0;
    await foreach (var item in enumerable)
    {
        if (i++ == 10){ break;}
        Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {item}");
    }
    Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: End");
}

static async IAsyncEnumerable<int> FetchItems(int delay)
{
    int count = 0;
    while(true)
    {
        await Task.Delay(delay);
        yield return count++;
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
