# Random team generator
- Application that takes in account the evaluations of the players and tries to create balanced teams.

## Instalation
- Download TeamGenerator.zip archive from this repository -- now as build artifact
- Expand/Extract it wherever you see fit and the installation is complete

## How to use the app
- To start the application run the TeamGenerator.Shell.exe

![image](https://user-images.githubusercontent.com/9948892/137002980-6db6c403-b53f-4a27-af18-8dad12247c24.png)

### Add player
- Insert Nick and Rank in the "Add player" section, then click on the "Add" button.
- Troubleshoot
  - The "Add" button is disabled
    - You already have player with this nick in "Available players" list
    - The player has invalid "Nick" (at this moment the only invalid nick is an empty string)
    - You still have focus in the "Nick" text box (press TAB or click outside the text box)

### Delete player
- Select player in "Available players" section and press DELETE key

### Generate teams
- Press "Generate teams" button
- You can press this button as many times as you want - it will create different & balanced teams from the "Available players" list
- Troubleshoot
  - The "Generate teams" button is disabled
    - There are no available players in the "Available players" list

### Save player pool
- This feature allows you to save your current player pool. So you don't have to insert all your players manually all the time.
- Press "Save player pool" button, select file location and press "Save" button. On the selected location you will find <your_file_name>.tgpp file
which contains all your players. 

### Load player pool
- This feature allows you to load players from previously saved player pool.
- Troubleshoot
  - The "Loading error" message box shows up.
    - Your .tgpp file has incorrect syntax. Try to fix it or try to create a new .tgpp file using save player pool feature.
    - The best advice here is: do not try to update those files manually.

![image](https://user-images.githubusercontent.com/9948892/137003979-057b9878-dd02-44d0-b098-61d2b450b7af.png)

### Fill teams with bots
- Use this feature only if other players can start playing as bots when they die (i.e. CS-GO). The algorithm evaluates the bot accoring to the evaluation of players in the team
- Sometimes there is not enough live players and you need to add some bots.
- If you enable this feature the algorithm will try to add bots into one or both teams to find the most fair evaluation.
- The "Max player count" parameter is there just for bot evaluation calculations and if you have the "Max player count" or more live player, then no bots will be added.


![image](https://user-images.githubusercontent.com/9948892/137007523-4b89ec5f-271d-483b-aa57-f47def1ba3c0.png)


![image](https://user-images.githubusercontent.com/9948892/137007574-99a7f802-fbf6-4449-b773-a8f9a36e82fc.png)


![image](https://user-images.githubusercontent.com/9948892/137007637-d1bb11ee-cc8f-4bfe-b37a-ee2da2b0b88b.png)

