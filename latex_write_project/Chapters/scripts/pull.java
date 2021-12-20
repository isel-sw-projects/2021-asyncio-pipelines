Stream<Integer> stream = Stream.of(1,2,3,4,5);
        Iterator it = stream.iterator();

        while(it.hasNext()) {
            System.out.println(it.next());
        }

//Output:
//1
//2
//3
//4
//5