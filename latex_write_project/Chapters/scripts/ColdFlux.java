Stream<Integer> stream = Stream.of(1,2,3,4,5);
Thread.sleep(2000);
stream.forEach(System.out::println); //1,2,3,4,5