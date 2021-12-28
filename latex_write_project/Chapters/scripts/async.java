// create a client
HttpClient client = HttpClient.newHttpClient();
// create a request
HttpRequest request = HttpRequest.newBuilder(
        URI.create("https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY"))
                .header("accept", "application/json")
                .build();

//not blocking http asynchronous request
var futureResponse = client
        .sendAsync(request, new JsonBodyHandler<>(DTO.class)) 
        .thenAccept(res -> System.out.println(res.body().get().title)); //consumer callback for when the operation finishes

futureResponse.get(); //blocks until the request is finished