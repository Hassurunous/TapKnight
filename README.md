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
Targets will indicate the areas and time in which certain areas should be tapped for the most effective attacks. The more accurate (time and space) your taps, the more effective the attacks. Missing or tapping too early results in a counterattack, in which the opponent can attack the player, causing damage to the player. Defeating enemies unlocks greater enemies and higher combat difficulties, simulated by increased speed and numbers of tap targets. Ideally, sound and music is incorporated so that audio-focused players can “feel” the rhythm with which they should be tapping.

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

[Back to top ^](#)

---

### MVP Milestones
[The overall milestones of first playable build, core gameplay, and polish are just suggestions, plan to finish earlier if possible. The last 20% of work tends to take about as much time as the first 80% so do not slack off on your milestones!]
* 

#### Week 1 
_planning your game_
* [goals for the week, should be build the game mechanics and objectives]
* Paper prototyping - [6 hours] : 
	* Initial paper prototyping - [1 hour]
* Validate idea with user testing of the paper prototype - [1 hour]
* Iterate on tests to update paper prototype, test again, ad infinitum - [4 hours]
* Layout a plan with the Design Document for development moving forward - [3 hours]
* Get a sense of the classes and behaviors we feel we need to implement - [3 hours]

#### Week 2
_finishing a playable build_
* [goals for the week, should be finishing a playable game]
* Implement the UI/UX design into XCode - [3 hours]
	* Create each of the screens we will need (Main Menu, Game Scene, Wardrobe) - [½ hour]
	* Lay out each scene according to paper prototype - [1 hour]
	* Extra time in case something takes longer - [1 hour]
* Code core game loop. Creating sushi stack, controls, and behavior of the hero character. - [2 days]
	* Create sushi base and stacking algorithm - [2 and ½ hours]
	* Create character and initial behavior - [1 hour]
	* Core game loop - [4 hours]
		* Get tapping to move down tower	- [2 hours]
		* Get character to move screens and flip z-location - [1 hour]
* Validate core gameplay is fun - [1 hour]
* Plan out the animations that we want to implement. Cat punching sushi, sushi flying to the other side, death animation. - [2 hours]
* Plan out short-term and long-term animation goals. Whether we need death animation now depending on if we have death yet or later. - [½ hour]

#### Week 3
* [goals for the week]
* Finalize UI/UX design and layout - [About 1 day (with testing)]
* Start on animation - [½ day to 1 day]
* Implement points and health bar - [2 hours]
* Implement death - [2 hours]
* Start on Golden Sushi (Always gives invincibility and a chance to get something else) and Wardrobe (Holds the cosmetic items you acquired thus far) - 1 day+
* Get Golden Sushi animation to be the same as the regular sushi but with sparkles [1 hour]
* Implement Golden Sushi boost/invincibility state (enum) in the gameplay.swift - [3 hours]
* Comment the implementation of Golden Sushi - [1 hour]
* Contain the probabilities of dropping health potions or wardrobe object. [1 hour]
* Comment the implementation of the Wardrobe class - [1 hour]
	* To contain cosmetic toggles 

#### Week 4
* [goals for the week, should be finishing all core gameplay]
* Bring in visual assets - [1 hour]
* Finish the golden sushi - [1-2 days]
	
* Finish wardrobe for basic coder art. (50% Opaque color sprite as “cosmetics”) - [1 days]
* Finish implementing the animations - [2 hours to ½ day]
* Get persisting user high scores working - [2-3 hours]

#### Week 5
_starting the polish_
* Adding Firebase/facebook login - [2-3 days]
	*Setting up firebase account and installing pods - [½ day]
	*creating database for highscore and linking it to the app - [½ day]
	*linking game with facebook sdk and getting login working - [½ day]
	*getting facebook accounts/score to the firebase database via the game- [½ day]
* Persistent “saved” high score load to firebase - [½ day]
* Display leaderboard with other friends high scores - [½ day]
* Add analytics - [½ day]

#### Week 6
_submitting to the App Store_
* [goals for the week, should be finishing the polish -- demo day on Saturday!]
* Finalize with user testing and make sure all features are working as intended. -1 day
	* Have at least 5 and most preferably 10 students use my game and give feedback - [½ day]
	* 
* Make sure all credits are attributed in a credit scene. -½ day
* Apple developer account and certificates are set up and ready for submission. -2 hours
* Get ready to upload to the App Store : [2 days+]
* Create screenshots for all device sizes supported - [½ day]
* Write out documentation for app store - [½ day]
* Work out a presentable icon for the iOS desktop screen - [½ day]
* Work out information/payment for developer account - [½ day]

[Back to top ^](#)
