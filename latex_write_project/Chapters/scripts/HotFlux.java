ConnectableFlowable<Long> hot = Flowable
                .intervalRange(0, 100, 0, 100, TimeUnit.MILLISECONDS)
                .publish();
flux.connect(); 
Thread.sleep(1000);
flux.blockingSubscribe(System.out::println);