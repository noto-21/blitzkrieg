<hr>

# blitzkrieg
Blitzkrieg is a four-player battle-royale tank game that was developed for a CS group project in secondary school, using C# and the XNA Framework.  Compatible with both Windows OS and the Xbox 360 (untested).  You can only use Xbox controllers to play (and only if you plug them into your PC's USB slots), but K/M support may be incorporated in the future.

<hr>

# XNA 4.0 Framework
This project requires that you have the XNA 4.0 Framework installed.  A copy of all files needed to run XNA with VS2019 are included, and alongside an installation guide were found [here](https://flatredball.com/visual-studio-2019-xna-setup/#:~:text=Introduction%201%20Download%20a%20modified%20version%20of%20MXA,XNA%20Game%20Studio%204.0.vsix%20.%20...%20More%20items).  Installing the XNA Framework is the **FIRST** thing you should do before attempting to run the game.  When you are told to create the folder 'XNA Game Studio' in 'C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Microsoft\' and copy the files to it, note that you should replace 'Community' with the version of VS that matches the one you've installed (mine is 'Enterprise', for example).  This is also explained in the tutorial itself, but I figured that I should reiterate it here since it's easy to miss.

If you are using a newer version of VS, then for the purposes of this project you can find the XNA 4.0 files needed [over here](https://www.microsoft.com/en-us/download/details.aspx?id=27598) and the resources needed to incorporate them [right here](https://docs.monogame.net/articles/migrate_xna.html).

## MonoGame API
Alternatively, you can consider utilizing the [MonoGame API](https://docs.monogame.net/index.html), a re-implementation of XNA (which has technically been discontinued) that is largely similar to XNA (save for a few [minor differences](https://docs.monogame.net/articles/migrate_xna.html#missingremoved-api)), for Blitzkrieg.  However, please note that this has not been tested on my part.

## .NET
Please ensure that your version of [.NET](https://dotnet.microsoft.com/en-us/download) is up to date.

<hr>

# Pipeline Assembly Loading Error
Finally, if you run into a *pipeline assembly loading error* when attempting to run/compile, [this article](https://learn.microsoft.com/en-us/answers/questions/814163/microsoft-xna-failure-to-load-pipeline) should help you out.  Note that you should have access to the VS Developer Command Prompt for your version of VS for the purposes of this solution, and that it should be run with administrator privileges to allow for 'Microsoft.Build.Framework.dll' to be registered into the Global Assembly Cache.  Also note that your file path may be slightly different for your version of VS compared to what is mentioned in the Q&A reply.  For example, you may have to change directories to 'Program Files (x86)' as opposed to 'Program Files' in your path, and instead of 'Preview', your path may contain 'BuildTools'.

<hr>
