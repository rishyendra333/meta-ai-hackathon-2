using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class BoatInstructionScraper : MonoBehaviour
{
    private string[] attemptedURLs = new string[]
    {
        "https://www.wikihow.com/Build-a-Flat-Bottomed-Boat",
        "https://www.instructables.com/How-to-Build-a-Simple-Wooden-Boat/",
        "https://www.popularmechanics.com/home/how-to/a6244/how-to-build-a-boat/",
        "https://www.diynetwork.com/how-to/skills-and-know-how/carpentry-and-woodworking/how-to-build-a-wooden-boat"
    };
    
    private List<string> scrapedInstructions = new List<string>();
    
    public IEnumerator AttemptScrape(string url)
    {
        Debug.LogWarning("Attempting web scrape (this probably won't work)...");
        
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("User-Agent", "Mozilla/5.0");
        
        yield return request.SendWebRequest();
        
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Scrape failed: {request.error}");
            Debug.LogError("Reason: Quest doesn't handle web requests well, sites block bots, CORS issues");
            yield break;
        }
        
        string html = request.downloadHandler.text;
        
        TryParseWikiHow(html);
        TryParseInstructables(html);
        TryParseGenericHTML(html);
        
        if (scrapedInstructions.Count == 0)
        {
            Debug.LogError("Parsing failed. HTML structure didn't match expected patterns.");
        }
    }
    
    void TryParseWikiHow(string html)
    {
        MatchCollection steps = Regex.Matches(html, 
            @"<div[^>]*class=[""']step[""'][^>]*>(.*?)</div>", 
            RegexOptions.Singleline);
        
        foreach (Match match in steps)
        {
            string stepText = StripHTML(match.Groups[1].Value);
            if (stepText.Length > 30)
            {
                scrapedInstructions.Add(stepText);
            }
        }
    }
    
    void TryParseInstructables(string html)
    {
        MatchCollection steps = Regex.Matches(html, 
            @"<div[^>]*class=[""']step-body[""'][^>]*>(.*?)</div>", 
            RegexOptions.Singleline);
        
        foreach (Match match in steps)
        {
            string stepText = StripHTML(match.Groups[1].Value);
            if (stepText.Length > 30)
            {
                scrapedInstructions.Add(stepText);
            }
        }
    }
    
    void TryParseGenericHTML(string html)
    {
        MatchCollection listItems = Regex.Matches(html, @"<li>(.*?)</li>", RegexOptions.Singleline);
        
        foreach (Match match in listItems)
        {
            string text = StripHTML(match.Groups[1].Value);
            if (text.Length > 20 && text.Length < 500)
            {
                scrapedInstructions.Add(text);
            }
        }
    }
    
    string StripHTML(string html)
    {
        string text = Regex.Replace(html, @"<[^>]+>", " ");
        text = System.Net.WebUtility.HtmlDecode(text);
        text = Regex.Replace(text, @"\s+", " ");
        return text.Trim();
    }
    
    public string[] GetScrapedInstructions()
    {
        if (scrapedInstructions.Count == 0)
        {
            Debug.LogWarning("Scraping didn't work. Using hardcoded fallback.");
        }
        return scrapedInstructions.ToArray();
    }
    
    void TestAllURLs()
    {
        foreach (string url in attemptedURLs)
        {
            StartCoroutine(AttemptScrape(url));
        }
    }
}

// We tried :(


There is no way this will work, who knows.