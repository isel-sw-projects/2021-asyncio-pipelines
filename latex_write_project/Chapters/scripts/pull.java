Flowable<Long> flow = Flowable
                .interval(1, TimeUnit.SECONDS);

Iterator<Long> iterator = flow.blockingIterable().iterator();

while (iterator.hasNext()) {
    System.out.println(iterator.next());
}