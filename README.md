# QuickTL

User-friendly GUI for translation on Windows. This project utilizes DeepL's translation API as well as Google's Cloud Vision API.
This project was built with C# using the WinForms framework.


## Usage
To use QuickTL, simply clone the repository with Visual Studio and build the project!
Once the project has been built, run the executable file and the GUI should show up. Keybinds can be edited by clicking their respective buttons and pressing the new keybind.
### Clipboard Translation
Upon pressing the keybind, whatever text is stored in the user's clipboard will be translated and the result will be displayed.
### Image Translation
Upon pressing the keybind, a transparent white overlay will be visible. From here, the user can select the portion of the screen they want to translate using their mouse. To cancel, press escape or left click without selecting a region.

_Note:_ You will need to provide your own DeepL and Google Vision API keys. You can insert them into the code on lines 19 and 20 in the _MainForm.cs_ file.
## Showcase
### Initial View

![alt text](https://i.imgur.com/fq99Hi8.png)

### Supported Source Languages

![Source Languages](https://i.imgur.com/KJuHgvJ.png)

### Supported Target Languages
![Target Languages](https://i.imgur.com/k1lCfJC.png)

### Demo

![Video Demo](https://i.imgur.com/7qO4n5C.gif)
