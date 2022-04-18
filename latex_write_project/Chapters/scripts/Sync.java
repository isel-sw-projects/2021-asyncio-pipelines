
HttpClient client = HttpClient.newHttpClient();

HttpRequest request = HttpRequest.newBuilder(
         URI.create("SOME URL"))
         .header("accept", "application/json")
         .build();

HttpResponse<Supplier<DTO>> response = 
        client.send(request, new JsonBodyHandler<>(DTO.class));//blocks

