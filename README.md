# Random team generator
- Wpf application which takes in account the ranks of the players and tries to create balanced teams.

## Instalation
- Download TeamGenerator.zip archive from this repository
- Expand/Extract it wherever you see fit and the installation is complete

## How to use the app
- To start the application run the TeamGenerator.Shell.exe
- The UI is pretty simple and workflow very straightforward


![image](https://user-images.githubusercontent.com/9948892/124349762-220fe380-dbf1-11eb-9af6-4d88057d20ba.png)

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


![image](https://user-images.githubusercontent.com/9948892/124350199-80d65c80-dbf3-11eb-97a8-4847ccd95eae.png)

![image](https://user-images.githubusercontent.com/9948892/124350247-d6126e00-dbf3-11eb-80bb-1377c73986cc.png)
