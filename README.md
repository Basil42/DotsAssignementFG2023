# DotsAssignement
 Dots assignement for the computer tech course FG2023


Overview of each system:

PlayerControlSystem
-This system runs on a singleton entity so I used system base or convenience and did not jobify it (not worth the overhead)
-I handle movement and the player shooting in it (it shares the same shooter component as the enemies though)

ShooterSystem
-Enableable, mostly for the player's sake
-once enabled auto fires at a rate and using a projectile fixed by the component (so future enemies or the player can have different projectile entities and fire rate)
-This system is jobified, there might be a lot of active shooter components at one time (at least one per enemy and player)

WaveSpawning
 -Similar to the spawner with a two staged cooldown to spawn in wave.
 -A good optimisation would be to spawn all the enemies of a wave in one batch and enable them progressively.
	-Unfortunately, the recommended way is to use a Disabled component, but it defeats the purpose as removing triggers a structural change
	-Maybe disabling specifically the sprite renderer, move component and shooter would work, but it's messy and probably still causes structural changes (DOTS likely stores disabled components in continuous chunks)  

MoveSystems
one for player, one for enemies, could probably be done in the same components managed by two different systems as player and enemies are tagged differently
-player move is handled by the control system
-enemy move system is of course jobified, there could be a lot of enemies
-very simple behavior, a pseudo orbit around the player, facing them

ProjectileSystem
very similar to move system, with a different behavior. It could use the same components as enemy movement and be filtered by a different tag but that provides nothing as is and we would probably want to extend them in very different ways.

Lifetime system

enemies and projectile have lifetimes after which they are automatically destroyed, this is jobified as a lot of entities might use it in a game of that kind.