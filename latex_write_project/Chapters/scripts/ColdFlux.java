Flowable<Long> cold = Flowable.interval(100, TimeUnit.MILLISECONDS);
Thread.sleep(1000);
cold.blockingSubscribe(System.out::println); 