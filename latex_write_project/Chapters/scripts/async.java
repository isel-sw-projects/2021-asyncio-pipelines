
HttpClient client = HttpClient.newHttpClient();

HttpRequest request = HttpRequest.newBuilder(
        URI.create("https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY"))
                .header("accept", "application/json")
                .build();

var futureResponse = client
        .sendAsync(request, new JsonBodyHandler<>(DTO.class)) 
        .thenAccept(res -> System.out.println(res.body().get().title)); //consumer callback for when the operation finishes

futureResponse.get();