# Random team generator
- Application that takes in account the evaluations of the players and tries to create balanced teams.

## Instalation
- Download TeamGenerator.zip archive from this repository -- now as build artifact
- Expand/Extract it wherever you see fit and the installation is complete

## How to use the app
- To start the application run the TeamGenerator.Shell.exe

![image](https://user-images.githubusercontent.com/9948892/145031949-4c085f00-3db2-4796-b94d-fb72ee474e66.png)

## Player pool
### Add player
![image](https://user-images.githubusercontent.com/9948892/145032292-1d17f4e8-2c68-409b-9bb8-f9da8992dfc7.png)
- Insert Nick and Rank in the "Add player" section, then click on the "Add" button.
- Troubleshoot
  - The "Add" button is disabled.
    - You already have player with this nick in the "Player pool".
    - The player has invalid "Nick" (at this moment the only invalid nick is null or an empty string).

### Delete player
![image](https://user-images.githubusercontent.com/9948892/145032488-92cbcf71-f42e-4e84-8d73-c7d775ae3649.png)
- Select player as shown in the picture and press DELETE key.

### Generate teams
- Press "Generate teams" button.
- You can press this button as many times as you want - it will create different & balanced teams from the players in the Player pool.
- Troubleshoot
  - The "Generate teams" button is disabled.
    - You need to have at least 2 players in the Player pool.

### Save player pool
- This feature allows you to save your current player pool. So you don't have to insert all your players manually all the time.
- Press "Save player pool" button, select file location and press "Save" button. On the selected location you will find <your_file_name>.tgpp file
which contains all your players. 

### Load player pool
- This feature allows you to load players from previously saved player pool.
- Troubleshoot
  - The "Loading error" message box shows up.

![image](https://user-images.githubusercontent.com/9948892/145034114-f405073b-419f-4217-92d8-53fba12ca700.png)

  - Message: "An exception occured while trying to load the player pool. Your player pool file contains invalid data" Exception message: Empty or white space only string are now allowed."
    - Check the file you're trying to import for missing values.
       
![image](https://user-images.githubusercontent.com/9948892/137003979-057b9878-dd02-44d0-b098-61d2b450b7af.png)

  - Message: "The selected file could not be loaded".
    - Your .tgpp file has incorrect syntax. Try to fix it or try to create a new .tgpp file using save player pool feature.
    - The best advice here is: do not try to update those files manually.


### Fill teams with bots
- Use this feature only if other players can start playing as bots when they die (i.e. CS-GO). The algorithm evaluates the bot accoring to the evaluation of players in the team
- Sometimes there is not enough live players and you need to add some bots.
- If you enable this feature the algorithm will try to add bots into one or both teams to find the most fair evaluation.
- The "Max player count" parameter is there just for bot evaluation calculations and if you have the "Max player count" or more live player, then no bots will be added.

![image](https://user-images.githubusercontent.com/9948892/145034691-2a66c2d4-cd21-4489-b71b-a363a9b61a36.png)


![image](https://user-images.githubusercontent.com/9948892/145034778-fd48d70f-39db-4384-9e00-0f6d29af166b.png)

