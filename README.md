## Table of Contents
  * [Game Design](#game-design)
    * [Objective](#objective)
    * [Gameplay Mechanics](#gameplay-mechanics)
    * [Level Design](#level-design)
  * [Technical](#technical)
    * [Scenes](#scenes)
    * [Controls/Input](#controlsinput)
    * [Classes](#classessks)
  * [MVP Milestones](#mvp-milestones)
    * [Week 1](#week-1)
    * [Week 2](#week-2)
    * [Week 3](#week-3)
    * [Week 4](#week-4)
    * [Week 5](#week-5)
    * [Week 6](#week-6)

---

### Game Design

#### Objective
Tap the circles at the proper time in order to strike with brutal effectiveness and vanquish your foes!

#### Gameplay Mechanics
Targets will indicate the areas and time in which certain areas should be tapped for the most effective attacks. The more accurately you time your taps, the more effective the attacks. Missing or tapping too early results in a counterattack, in which the opponent can attack the player, causing damage to the player. Defeating enemies unlocks greater enemies and higher combat difficulties, simulated by increased speed and numbers of tap targets. Ideally, sound and music is incorporated so that audio-focused players can “feel” the rhythm with which they should be tapping.

#### Level Design
To start, there is one continuous level, with foes simply replacing their fallen comrades until the player falls to their onslaught. Outside of the MVP, the goal is to build several levels, on a map, where the player can choose to upgrade their abilities and skills, choose new levels, and progress throughout the world to harder and more powerful foes.

[Back to top ^](#)

---

### Technical

#### Scenes
* Main Menu
* Combat Screen
* Map Screen
* Shop Screen
* ...

#### Controls/Input
Single finger tap on the tap obstacles. Touchscreen interface to shop and travel the map.  

#### Classes/SKS
* Classes
  * [list classes needed and some basic information about required implementation]
  * ...
* Scenes
  * [list Scene files you will need to create]
  * MainMenu
  * Combat
  * Shop
  * Map

[Back to top ^](#)

---

### MVP Milestones
[The overall milestones of first playable build, core gameplay, and polish are just suggestions, plan to finish earlier if possible. The last 20% of work tends to take about as much time as the first 80% so do not slack off on your milestones!]
* Player can tap on the target to attack
* Player can intuitively determine the positive and negative effects of tap timing
* Player can see their health decrease as they miss or tap too early
* Enemies will die after a certain amount of damage has been done
* New enemies will spawn and run onto the screen when the Player has defeated an enemy
* Score will be tracked to show the player how successful their run has been
* Players will be able to upgrade various aspects (Damage, Health, Armor) of their character
* Players will be able to track their high score

#### Week 1
_MVP pre-setup_
* [goals for the week, should be build the game mechanics and objectives]
* Building the initial functionality for TapTargets
* Creating the sprite sheet for the first Player sprite
* Creating the sprite sheet for the first Enemy sprite
* Creating animations for the player and the first enemy
* Set up the default animation for the player and enemy

#### Week 2
_finishing a playable build_
* [goals for the week, should be finishing a playable game]
* Implement the UI/UX design into Unity - [3 hours]
	* Create each of the screens we will need (Main Menu, Combat, Shop, Map) - [½ hour]
	* Lay out each scene according to paper prototype - [1 hour]
	* Extra time in case something takes longer - [1 hour]
* Code core game loop. Creating TapTargets, controls, and behavior of the hero character. - [2 days]
	* Create TapTarget scoring functionality - [1 hour]
	* Create character and initial behavior - [1 hour]
    * Create enemy and initial behavior - [1 hour]
	* Core game loop - [4 hours]
		* Get tapping damage the enemies	- [2 hours]
		* Get enemies to spawn and approach the player - [1 hour]
* Validate core gameplay is fun - [1 hour]
* Plan out the animations that we want to implement. Idle, Attack, Run, Hurt, and Die for each character used. - [2 hours]
* Plan out short-term and long-term animation goals. - [½ hour]

#### Week 3
* [goals for the week]
* Finalize UI/UX design and layout - [About 1 day (with testing)]
* Get Animations implemented and triggered correctly - [½ day to 1 day]
* Implement points and health bar - [2 hours]
* Implement death - [2 hours]
* Start on Increasing Difficulty Levels, Shop, and Map - 1 day+
* Contain the probabilities of dropping health potions and armor refresh objects. [1 hour]
* Comment the implementation of the Shop and Map class - [1 hour]

#### Week 4
_polish and extra features_
* [goals for the week, should be finishing all core gameplay]
* Finish the Difficulty scaling, Shop, and Map - [1-2 days]
* Get persisting user high scores working - [2-3 hours]
* Adding Facebook login - [2-3 days]
 * Setting up online database account - [½ day]
 * creating database for highscore and linking it to the app - [½ day]
 * linking game with facebook sdk and getting login working - [½ day]
 * getting facebook accounts/score to the database via the game- [½ day]
* Persistent “saved” high score load to database - [½ day]
* Display leaderboard with other friends high scores - [½ day]
* Add analytics - [½ day]

#### Week 5
_submitting to the App Store_
* [goals for the week, should be finishing the polish -- demo day on Saturday!]
* Finalize with user testing and make sure all features are working as intended. -1 day
	* Have at least 5 and most preferably 10 students use my game and give feedback - [½ day]
* Make sure all credits are attributed in a credit scene. -½ day
* Apple developer account and certificates are set up and ready for submission. -2 hours
* Android Google Play Store account set up and tested
* Get ready to upload to the App Store / Google Play Store : [2 days+]
* Create screenshots for all device sizes supported - [½ day]
* Write out documentation for app stores - [½ day]
* Work out a presentable icon for the desktop screen - [½ day]
* Work out information/payment for developer account - [½ day]

[Back to top ^](#)
