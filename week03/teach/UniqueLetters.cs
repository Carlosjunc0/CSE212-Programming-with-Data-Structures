public static class UniqueLetters {
    public static void Run() {
        var test1 = "abcdefghjiklmnopqrstuvwxyz"; // Expect True because all letters unique
        Console.WriteLine(AreUniqueLetters(test1));

        var test2 = "abcdefghjiklanopqrstuvwxyz"; // Expect False because 'a' is repeated
        Console.WriteLine(AreUniqueLetters(test2));

        var test3 = "";
        Console.WriteLine(AreUniqueLetters(test3)); // Expect True because its an empty string
    }

    /// <summary>Determine if there are any duplicate letters in the text provided</summary>
    /// <param name="text">Text to check for duplicate letters</param>
    /// <returns>true if all letters are unique, otherwise false</returns>
    private static bool AreUniqueLetters(string text) {
        // We use a HashSet to store letters that have already been found.
        // HashSet provides approximately O(1) lookup and insertion,
        // making the overall algorithm O(n).
        var letters = new HashSet<char>();

        foreach (var letter in text) {
            // If the letter is already in the set, it is duplicated.
            if (letters.Contains(letter))
                return false;

            // Add the letter to the set.
            letters.Add(letter);
        }

        // No duplicate letters were found.
        return true;
    }
}