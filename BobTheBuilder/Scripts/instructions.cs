using UnityEngine;
using Meta.WitAi.TTS.Utilities;
using Meta.WitAi;
using TMPro;

public class BoatBuildingInstructor : MonoBehaviour
{
    [SerializeField] private TTSSpeaker ttsSpeaker;
    [SerializeField] private TextMeshProUGUI instructionText;
    private int currentStep = 0;
    
    private string[] boatInstructions = new string[]
    {
        "Welcome to the wooden boat building tutorial. Say next to begin.",
        "Step 1: Gather materials from Walmart. You'll need eight pine boards, 1 by 6 inches by 8 feet long.",
        "Step 2: Get wood glue, marine epoxy, sandpaper, and wood screws from the hardware section.",
        "Step 3: Lay two boards parallel, 3 feet apart. These will be the side panels of your boat.",
        "Step 4: Cut two boards to 3 feet length for the front and back transom pieces.",
        "Step 5: Apply wood glue to the edges and attach the transom pieces to create a rectangular frame.",
        "Step 6: Cut remaining boards to fit as the bottom planks. Space them slightly for water drainage.",
        "Step 7: Secure bottom planks with wood screws every 6 inches along the frame.",
        "Step 8: Sand all surfaces smooth to prevent splinters. Use 80 grit, then 120 grit sandpaper.",
        "Step 9: Apply marine epoxy to all seams and let cure for 24 hours.",
        "Step 10: Add a seat board across the middle. Your basic boat is complete! Test in shallow water first."
    };

    void Start()
    {
        // Initialize Wit for voice commands
        VoiceService voiceService = FindObjectOfType<VoiceService>();
        if (voiceService != null)
        {
            voiceService.VoiceEvents.OnPartialTranscription.AddListener(OnPartialTranscription);
        }
        
        SpeakInstruction(currentStep);
    }

    void OnPartialTranscription(string transcription)
    {
        if (transcription.ToLower().Contains("next"))
        {
            NextInstruction();
        }
    }

    public void NextInstruction()
    {
        currentStep++;
        if (currentStep >= boatInstructions.Length)
        {
            currentStep = boatInstructions.Length - 1;
            ttsSpeaker.Speak("You've completed all steps!");
            return;
        }
        
        SpeakInstruction(currentStep);
    }

    void SpeakInstruction(int stepIndex)
    {
        string instruction = boatInstructions[stepIndex];
        
        // Display text
        if (instructionText != null)
            instructionText.text = instruction;
        
        // Speak with TTS
        if (ttsSpeaker != null)
            ttsSpeaker.Speak(instruction);
        
        Debug.Log(instruction);
    }
}


yo I don't knwo what's wrong with the code here!!#sfjnask