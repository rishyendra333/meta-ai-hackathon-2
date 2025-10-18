using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class LLMBoatPartContext : MonoBehaviour
{
    [Header("Hardcoded Boat Parts Knowledge Base")]
    private Dictionary<string, string> boatPartsDatabase = new Dictionary<string, string>()
    {
        // Side Panels
        {"LeftSidePanel", "This is the left side panel of the wooden boat. It's a long pine board (1x6x8 feet) from Walmart that forms the port side wall. It runs the full 8-foot length and provides structural integrity."},
        
        {"RightSidePanel", "This is the right side panel of the wooden boat. It's a long pine board (1x6x8 feet) from Walmart that forms the starboard side wall. It mirrors the left panel and runs the full length."},
        
        // Transoms
        {"FrontTransom", "This is the front transom piece, also called the bow transom. It's a 3-foot section of pine board from Walmart that connects the two side panels at the front of the boat. It's attached with wood glue and screws."},
        
        {"BackTransom", "This is the back transom piece, also called the stern transom. It's a 3-foot section of pine board from Walmart that closes off the back of the boat and connects both side panels. This is where you'd mount a small motor if desired."},
        
        // Bottom Planks
        {"BottomPlank1", "This is the first bottom plank. It's one of several pine boards from Walmart that form the floor of the boat. These planks are laid perpendicular to the side panels with small gaps for water drainage."},
        
        {"BottomPlank2", "This is the second bottom plank. Part of the boat's floor made from Walmart pine. These are secured with wood screws every 6 inches and coated with marine epoxy for waterproofing."},
        
        {"BottomPlank3", "This is the third bottom plank that helps form the boat's floor. Another Walmart pine board screwed into the frame with proper spacing for drainage."},
        
        {"BottomPlank4", "This is the fourth bottom plank, completing the boat's floor structure. Like the others, it's pine from Walmart, secured and sealed with epoxy."},
        
        // Seat
        {"SeatBoard", "This is the seat board that goes across the middle of the boat. It's a pine board from Walmart that serves dual purposes: providing a place to sit and adding structural cross-bracing to keep the boat rigid."},
        
        // Hardware (if you add these)
        {"WoodScrew", "This is a wood screw from Walmart's hardware section. These screws are used every 6 inches along the frame to secure the bottom planks and other joints."},
        
        {"WoodGlue", "This is wood glue from Walmart. It's applied to all edge joints before screwing pieces together for extra strength and water resistance."},
        
        {"MarineEpoxy", "This is marine epoxy from Walmart. It's applied to all seams after assembly and must cure for 24 hours. This waterproofs the boat and prevents leaks."},
        
        {"Sandpaper", "This is sandpaper from Walmart (80 grit and 120 grit). Used to smooth all wood surfaces to prevent splinters and prepare the wood for epoxy application."}
    };
    
    [Header("General Boat Context")]
    private string generalBoatContext = @"
WOODEN BOAT PROJECT CONTEXT:
This is a simple wooden boat built entirely from materials available at Walmart. 
The boat is approximately 8 feet long and 3 feet wide.
Construction uses basic pine boards (1x6 inch dimensions), wood screws, wood glue, marine epoxy, and sandpaper.
The design is a flat-bottom rowboat style suitable for calm waters.
Total material cost is approximately $70-90 from Walmart.
The boat consists of: 2 side panels, 2 transom pieces (front and back), 4-5 bottom planks, and 1 seat board.
All wood should be sanded smooth and sealed with marine epoxy before use in water.
";

    // This method provides context to the Meta AI SDK
    public string GetLLMContext(List<string> visibleObjectNames = null)
    {
        StringBuilder context = new StringBuilder();
        context.AppendLine(generalBoatContext);
        context.AppendLine("\nCURRENTLY VISIBLE BOAT PARTS:");
        
        if (visibleObjectNames == null || visibleObjectNames.Count == 0)
        {
            // If no specific objects, give all parts info
            context.AppendLine("(All boat parts)");
            foreach (var part in boatPartsDatabase)
            {
                context.AppendLine($"\n{part.Key}: {part.Value}");
            }
        }
        else
        {
            // Only include visible parts
            foreach (string objName in visibleObjectNames)
            {
                if (boatPartsDatabase.ContainsKey(objName))
                {
                    context.AppendLine($"\n{objName}: {boatPartsDatabase[objName]}");
                }
            }
        }
        
        return context.ToString();
    }
    
    // Get description for a specific part
    public string GetPartDescription(string partName)
    {
        if (boatPartsDatabase.ContainsKey(partName))
        {
            return boatPartsDatabase[partName];
        }
        return "Unknown boat part.";
    }
    
    // Method to call when user asks "what do I see"
    public string GenerateVisualContext()
    {
        // Find all boat parts in the scene
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("BoatPart");
        List<string> visibleParts = new List<string>();
        
        // You could add frustum culling here to only include visible parts
        foreach (GameObject obj in allObjects)
        {
            visibleParts.Add(obj.name);
        }
        
        // If no tagged objects, try finding by name patterns
        if (visibleParts.Count == 0)
        {
            foreach (var partName in boatPartsDatabase.Keys)
            {
                GameObject obj = GameObject.Find(partName);
                if (obj != null)
                {
                    visibleParts.Add(partName);
                }
            }
        }
        
        return GetLLMContext(visibleParts);
    }
    
    // For Meta XR AI integration - call this to get context string
    public string GetContextForMetaAI()
    {
        return GenerateVisualContext();
    }
    
    // Debug method
    void Start()
    {
        Debug.Log("LLM Context Provider Ready");
        Debug.Log($"Loaded {boatPartsDatabase.Count} boat part descriptions");
    }
    
    // You can call this from Meta's voice command system
    public void OnUserAsksWhatTheyAreLookingAt()
    {
        string context = GenerateVisualContext();
        Debug.Log("Generated context for LLM:\n" + context);
        
        // Pass this context to Meta's AI SDK
        // Example: MetaAIService.SendPrompt(context + "\n\nUser asked: What am I looking at?");
    }
}

this is not valid c# code at all!!!
