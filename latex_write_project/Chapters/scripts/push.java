Flowable<Long> flow = Flowable
                .interval(1, TimeUnit.SECONDS);
        
flow.blockingSubscribe(System.out::println);

//Output:
//1
//2
//3
//4
//5