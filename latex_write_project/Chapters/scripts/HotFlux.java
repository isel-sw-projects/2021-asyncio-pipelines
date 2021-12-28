ConnectableFlowable<Long> hot = Flowable
                .intervalRange(0, 100, 0, 100, TimeUnit.MILLISECONDS)
                .publish();
flux.connect(); // Start emition
Thread.sleep(1000);
flux.blockingSubscribe(System.out::println); // 10,12,13,14,15,16 ...