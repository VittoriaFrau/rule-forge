## Project Info 
- **Project Name**: Rule Forge
- **Project Description**: Rule Forge is a mixed reality application that allows the user to create and edit rules for a Virtual environment. The user can create a rule by using a set of predefined blocks and by connecting them together. The user can also edit the rules by changing the blocks' properties. The application is designed to be used in a virtual reality environment, so the user can interact with the blocks using his hands. The application is developed using Unity and the Mixed Reality Toolkit.

## Project Structure
- **Assets**: Contains all the assets used in the project
- **Packages**: Contains all the packages used in the project
- **ProjectSettings**: Contains all the settings of the project
- **RuleForge/Editor**: Contains all the scripts used in the editor
- **RuleForge/Scripts**: Contains all the scripts used in the application

## How to use the application
- **Create a new rule**: To create a new rule, the user has to press the "Create Rule" button in the main menu. Then, the user has to select the name of the rule and the type of the rule. After that, the user has to press the "Create" button to create the rule.
- **Edit a rule**: To be implemented
- **Delete a rule**: To be implemented

## Development info
- **Unity version:** 2022.3.12f1 (LTS)
- **MRTK version:** 3.2.2
- **OpenXR version:** 1.0.0-preview.9
- **MixedRealityFeatureTool version:** 0.0.1-preview.9
- **Nuget packages:**
	- WebSocketSharp-netstandard
	- Newtonsoft.Json
- **Mixed Reality Toolkit packages:**
	- MRTK Graphics Tools
	- MRTK Windows Text-To-Speech Plugin
	- MRTK UX Components
	- MRTK UX Components (Non - Canvas)
	- MRTK UX Core Scripts
	- MRTK Windows Speech
- **Platform Support:**
  - Mixed Reality OpenXR Plugin
  - **Project Settings:**
    - XR Plugin Management:
	  - OpenXR + Windows Mixed Reality feature group
    - OpenXR:
    - Depth 24 Bit
      - Microsoft Motion Controller Prodile
      - Hand Interaction Profile
      - Eye Gaze Interaction Profile
    - OpenXRFeature groups:
	  - Microsoft Hololens: hand tracking, mixed reality features (automatic selected), motion controller model
	- Project Validation:
      - Fix what is needed
    - **MRTK3**:
      - Make sure that there is a profile assigned
      - Available MRTK Subsystems:
		- Dictation
		- Hands Aggregator
		- Hand Synthesis
		- OpenXR Hands API
		- Windows KeywordRecognition
		- Text-To-Speech ---> not needed

	- **Player**:
	  - Other Settings
        - Scripting Backend: Mono/IL2CPP
        - API Compatibility Level: .Net Framework