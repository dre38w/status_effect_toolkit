Welcome to the Status Effect Toolkit!  This is a versitile tool that will allow you to easily implement global status effects and can be used in any genre.
The Demo Scene showcases an Amnesia Dark Descent style Sanity System where looking at monsters and being in the dark increase the sanity level (status level) and turning on a light or completing the bonus objective decrease the level.
The example status effects showcase how these global effects can be implemented.  Included is a screen frost effect, screen red damage effect, tunnel vision, and hallucinations.


***How to use the Status Effect Tool***

Open the tool by going to Tools/Status Effect Tool
When you first open this window you will see three fields:

-Status Effect Selection Settings-
This is used to modify what happens when a status level changes.  For example, if you want to require a wait time before displaying the effect to allow other furture transition logic to run you select Require Wait Time Between Effects and set either a set duration or a randomized time.

-Max Status Level-
This is the amount of elements in the Status Levels list.  This is simply for readability and must be the same value as the number of Status Levels

-Status Levels-
This is the core of the system.  When populating this field you will see several options.
- Choose Random Status Effect: This simply chooses a random effect if you have two or more in the Status Effects Data list.
- Is Periodic Status Level: Selecting this causes the entire Status Level to run periodically.  You can use a set duration or a random duration, which is how long the effect will run.  You will also notice a Restart Wait Time which is how long the effect is turned off before running again.
- Status Effects Data: This holds the actual data of the effect.  When populating this list you will see several more options.
-- Status Effect: This is a drop down with the names of the effects you want to use.
-- Severity: This is useful if you want to add a severity feature in the future.  Example: one status level could have mild hallucinations.  Another you could pass a higher severity and set data to allow more aggressive hallucinations.
-- Require Wait Time To Start: This is similar to the Status Effect Selection settings but local to the individual effect rather than the entire status level.
-- Is Periodic Effect: This is similar to Is Periodic Status Level, but allows you to run a specific effect as periodic or even run both the level and the effect as periodic.

Updating these values will automatically save them.


In the Managers prefab you will see a child named StatusEffects.  Select one of its children to view the data in the Inspector.
The key fields are Status Effect Data, Status Effect, and Severity.  These must match what is in the Status Effect Tool.  This is what allows the system to match and run the specific effects.
Under the Actions object, you will see two example actions.  These are the actions the player takes or more broadly the actions that can influence the status level.


There is a MainUI that has a debug menu.  It tells you what status level you are on, what status effect is currently running, and has two buttons that allow you to manually increase or decrease the status level.


***For developers***

Review the scripts to get familiar with how the architecture is built.  There is documentation throughout the code.

To expand this system and add a new Status Effect you will create a script and derive from StatusEffect class.  Then you will add any effect specific logic in this new class.

To add a new action you will create a script and derive from ActionHandler.  Then you will add any specific logic into this new class.

Lastly, if you added a new effect you will need to go into the StatusEffectsData class and add to the StatusEffects enum.  This will add to the drop down in the Status Effect Tool window and to the Status Effect object.  (Refer to the above to learn how matching and choosing an effect works and why this step is important.)

The system is designed to make it easy for you to integrate even further Framework level logic if you need to.