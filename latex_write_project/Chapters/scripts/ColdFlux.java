Flowable<Long> cold = Flowable.interval(100, TimeUnit.MILLISECONDS);
Thread.sleep(1000);
cold.blockingSubscribe(System.out::println); //prints: 0,1,2,3,4 .... no items lost