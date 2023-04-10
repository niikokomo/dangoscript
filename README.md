# Dango Script
A small portable windows GUI application to quickly modify the scripts of Little Busters! English Edition.

# Credits
Dango Script relies on and includes binaries for both [LucaSystemTools](https://github.com/wetor/LucaSystemTools/tree/lb_en) and [LuckSystem](https://github.com/wetor/LuckSystem).

# Installation
Simply download dangoscript.zip and extract the contents to any location of the user's choosing, so long as it has user write permissions.

# Usage

![image](https://user-images.githubusercontent.com/93227270/231001282-62f467ed-32bf-4fe3-801f-5eab97695f47.png)

After running through a first time setup an locating Little Busters' installation directory, the user will have four options...

### Load SCRIPT.PAK
This will create a backup up Little Busters! English Edition's script file, extract the file's contents, then convert those contents to .json for easy editing. If a backup already exists, the user will be warned as to not unintentionally overwrite their backup with a modified file.

### Push Changes
This will apply any changes the user has made to the converted .json files directly to the SCRIPT.PAK file in Little Busters! English Editions directory. Afterwards, the user can start Little Busters! to see their script edits reflected in game.

### Change Directory
If for any reason the program is referencing the incorrect directory or no directory at all, the user may use this to fix the issue.

### Restore Backup
This will restore the scripts to their default state, prior to any edits.

# Script Editing

I may expand this section in the future as needed, but for now, here's how to freely change lines of dialogue.

Little Busters! English Edition has a debug mode that can be enabled by starting the game with any launch arguments. I simply put "-v" in my Steam launch options. This will display the file and index the current line is located on.

![image](https://user-images.githubusercontent.com/93227270/231005948-b4dcbbf1-5b85-479f-929c-4de30ea618b1.png)

SEEN#.org refers to the name of the .json file that contains the line, and the number refers to the index of this line. Each json should have many entries of many different opcodes, but only "MESSAGE" opcodes contain dialogue. The first value under the "data" section of a "MESSAGE" opcode is the same as the index that's shown ingame. 

![image](https://user-images.githubusercontent.com/93227270/231006672-11c74e84-ba78-4b03-9401-5822293b2d46.png)

Lines are structured as "Speaker@'Dialogue'". Make any change you desire, save your changes, then press "Push Changes" to apply those to the game.

Script files are powerful, and control much more than just dialogue. A well informed editor might very well be able to make changes to the game logic itself.



# Disclaimer
This tool was almost entirely made using Microsoft's Bing Chat. I wanted a GUI application but knew it would be quite the undertaking at my non-existent skill level. This combined with a morbid curiosity of Bing Chat's capabilities and well... needless to say the future is gonna be wild.

This program is largely untested, so it's advised that you use it at your own risk.
