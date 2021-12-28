Integer []  arr = new Integer[]{1,2,3,4,5,6};

Flowable<Integer> flow = Flowable
    .fromArray(arr);

flow.forEach(System.out::println);

//Output:
//1
//2
//3
//4
//5