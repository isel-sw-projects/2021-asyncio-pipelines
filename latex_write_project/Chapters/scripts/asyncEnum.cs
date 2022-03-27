static async Task Main(string[] args)
{
    IAsyncEnumerable<int> enumerable = FetchItems(1000);
    int i = 0;
    await foreach (var item in enumerable)
    {
        if (i++ == 10){ break;}
    }
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
