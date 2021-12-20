CompletableFuture
        .runAsync(()-> System.out.println("I am asynchronous!"));

System.out.println("What you are!?");

//Output:
//What you are!?
//I am asynchronous!