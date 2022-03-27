import collections.Set;

import java.util.Scanner;

/**
 *
 */
public class SetApp {
    private static final int MAX_SETS = 10;

    public static void main(String[] args) {
        Scanner in = new Scanner(System.in);

        Set[] sets = new Set[MAX_SETS];
        int numSets = 0;

        boolean finish = false;
        do {
            System.out.println("collections.Set " + numSets + ":");
            Set s = new Set();
            do {
                int i = in.nextInt();
                if (i < 0) {
                    break;
                }
                s.add(i);
            } while (true);
            if(s.getSize() != 0) {
                sets[numSets++] = s;
            } else {
                finish = true;
            }
        } while(!finish);


        for (int i = 0; i < numSets; i++) {
            printSet(sets[i]);
        }

    }

    private static void printSet(Set set) {  // comment
        int[] elements = set.getElements();
        System.out.print('{');
        for (int i = 0; i < elements.length; i++) {
            System.out.print(elements[i] + (i == elements.length-1 ? "" : ","));
        }
        System.out.println('}');
    }
}
