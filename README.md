# Game Launcher with Physical Media Support

With physical media being discarded in favor of digital licenses, this launcher aims to bring back the physical media expierence to people everywhere! After setting up a USB, CD/DVD, SD Card, or even a Floppy Disk (any drive that can be successfully read by your operating system is supported!) to follow the right structure this launcher expects, you can easily plug in the device and the launcher will display all the games on it! You can even carry that physical device over to another system, plug it in, and get them all to show up there! To learn more about how to set up the launcher, check out the [Installation guide here](https://github.com/WSU-4110/PhysicalMediaGameLauncher/blob/main/INSTALLATION.md)!

## Requirements for setting up the Project (Editor)
- Unity Hub
- Unity 6000.0.49f1

## Editor Setup Instructions
After installing and setting up both, Unity Hub and the specified Untiy version, follow these instructions to open up and edit the project:
1. Clone this repository somewhere on to your computer
2. Click "Open" at the top of Unity Hub
3. Navigate to the cloned repository folder
4. Press "Open" (or w/e your OS's confirmation button is)
5. Click on the project in the Unity Hub list
6. Enjoy!

## Requirements for setting up the Project (Manifest Creator)
- php 8.4.6
- Enable the following extensions: `curl`, `fileinfo`, `intl`, `openssl`
- Create and get an API key from [SteamGridDB](https://www.steamgriddb.com/)

## Physical Device Manifest Creator Setup Instructions
1. Clone this repository somewhere on to your computer
2. Create a new file in the root of the repository called `.env`
3. Insert the following in to the file: `STEAM_GRID_DB=Your SteamGridDB Key`
4. Navigate to `PhysicalMediaManifestCreator` inside the root of the repository
5. Run the following command: `php -S localhost:8081`
6. Open up your web browser and go to `http://localhost:8081`
7. Enjoy!
