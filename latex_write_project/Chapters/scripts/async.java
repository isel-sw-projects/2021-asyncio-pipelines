
HttpClient client = HttpClient.newHttpClient();

HttpRequest request = HttpRequest.newBuilder(
        URI.create("SOME URL"))
                .header("accept", "application/json")
                .build();

var futureResponse = client
        .sendAsync(request, new JsonBodyHandler<>(DTO.class)) 
        .thenAccept(res -> res.body().get().title); 

Console.WriteLine("prints something")