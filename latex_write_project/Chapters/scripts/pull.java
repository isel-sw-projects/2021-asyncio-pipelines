Integer []  arr = new Integer[]{1,2,3,4,5,6};

 Flowable<Integer> flow = Flowable
    .fromArray(arr);

Iterator<Integer> iterator = flow.blockingIterable().iterator();

    while(iterator.hasNext()) {
        System.out.println(iterator.next());
    }

//Output:
//1
//2
//3
//4
//5