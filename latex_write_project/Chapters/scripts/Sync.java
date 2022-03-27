// create a client
HttpClient client = HttpClient.newHttpClient();

// create a request
HttpRequest request = HttpRequest.newBuilder(

         URI.create("https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY"))
         .header("accept", "application/json")
         .build();

HttpResponse<Supplier<DTO>> response = client.send(request, new JsonBodyHandler<>(DTO.class));

