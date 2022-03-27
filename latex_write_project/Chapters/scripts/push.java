Flowable<Long> flow = Flowable
                .interval(1, TimeUnit.SECONDS);
        
flow.blockingSubscribe(System.out::println);
