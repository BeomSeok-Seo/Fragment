using UnityEngine;
using System.Collections;

public static class CPUDeck {

    public static int[] GetDeck()
    {
        int[] deck = { 24, 23, 10, 19, 21, 26, 36, 2,
                       33, 18, 3, 4, 22, 15, 34, 20,
                       5, 7, 17, 14, 21, 25, 38, 13,
                       42, 9, 27, 6, 12, 8, 40, 41,
                       28, 37, 35, 39, 11, 31, 32, 16 };

        return deck;
    }
}
