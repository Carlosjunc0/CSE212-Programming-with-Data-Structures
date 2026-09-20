using System.Text.Json;

public static class SetsAndMaps
{
    /// <summary>
    /// The words parameter contains a list of two character 
    /// words (lower case, no duplicates). Using sets, find an O(n) 
    /// solution for returning all symmetric pairs of words.  
    ///
    /// For example, if words was: [am, at, ma, if, fi], we would return :
    ///
    /// ["am & ma", "if & fi"]
    /// </summary>
    /// <param name="words">An array of 2-character words (lowercase, no duplicates)</param>
    public static string[] FindPairs(string[] words)
    {
        var seen = new HashSet<string>();
        var result = new List<string>();

        foreach (var word in words)
        {
            // Ignorar palabras con caracteres idénticos como "aa"
            if (word.Length == 2 && word[0] == word[1])
            {
                continue;
            }

            // Invertir la palabra de 2 caracteres
            string reversed = $"{word[1]}{word[0]}";

            if (seen.Contains(reversed))
            {
                result.Add($"{word} & {reversed}");
            }
            else
            {
                seen.Add(word);
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Read a census file and summarize the degrees (education)
    /// earned by those contained in the file.
    /// The degree information is in the 4th column of the file.
    /// </summary>
    /// <param name="filename">The name of the file to read</param>
    /// <returns>A dictionary of degrees and their counts</returns>
    public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        var degrees = new Dictionary<string, int>();

        foreach (var line in File.ReadLines(filename))
        {
            var fields = line.Split(",");
            if (fields.Length >= 4)
            {
                var degree = fields[3].Trim();
                if (!string.IsNullOrEmpty(degree))
                {
                    if (degrees.ContainsKey(degree))
                    {
                        degrees[degree]++;
                    }
                    else
                    {
                        degrees[degree] = 1;
                    }
                }
            }
        }

        return degrees;
    }

    /// <summary>
    /// Determine if 'word1' and 'word2' are anagrams using a dictionary.
    /// Case-insensitive and spaces are ignored.
    /// </summary>
    public static bool IsAnagram(string word1, string word2)
    {
        var charCounts = new Dictionary<char, int>();

        // Procesar primer string
        foreach (char c in word1)
        {
            if (c == ' ') continue;
            char lower = char.ToLowerInvariant(c);

            if (charCounts.TryGetValue(lower, out int count))
            {
                charCounts[lower] = count + 1;
            }
            else
            {
                charCounts[lower] = 1;
            }
        }

        // Procesar segundo string restando las frecuencias
        foreach (char c in word2)
        {
            if (c == ' ') continue;
            char lower = char.ToLowerInvariant(c);

            if (!charCounts.TryGetValue(lower, out int count))
            {
                return false;
            }

            if (count == 1)
            {
                charCounts.Remove(lower);
            }
            else
            {
                charCounts[lower] = count - 1;
            }
        }

        return charCounts.Count == 0;
    }

    /// <summary>
    /// Reads JSON earthquake data and returns a list of locations and magnitudes.
    /// </summary>
    public static string[] EarthquakeDailySummary()
    {
        const string uri = "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";
        using var client = new HttpClient();
        using var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        using var response = client.Send(getRequestMessage);
        using var jsonStream = response.Content.ReadAsStream();
        using var reader = new StreamReader(jsonStream);
        var json = reader.ReadToEnd();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, options);

        if (featureCollection?.Features == null)
        {
            return Array.Empty<string>();
        }

        var results = new List<string>();
        foreach (var feature in featureCollection.Features)
        {
            if (feature?.Properties != null)
            {
                string place = feature.Properties.Place;
                double mag = feature.Properties.Mag ?? 0.0;
                results.Add($"{place} - Mag {mag}");
            }
        }

        return results.ToArray();
    }
}