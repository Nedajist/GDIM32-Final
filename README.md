[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/dDoYVaRL)
# HW2
## Devlog
### Nate
Final Project: Check-In Devlog
My primary contributions to In too deep so far have been level related. I created the scene, terrain, and level assets that the player jumps between. This includes the Checkpoint game object, which can be found on various platforms throughout the level, which function like a traditional checkpoint system. To set this system up, I also had to implement respawn. I created a Respawn() method, which resets the player to the transform of their most recent checkpoint, and reverts their health to its maximum state. From here I created SetRespawnPoint(), a method that stores the transform of the most recent checkpoint. This method would then be called by the CheckpointController, which first compares the tag of the collision to the Player, before calling SetRespawnPoint(). Consequently, whenever a player collides with the Checkpoint game object, the transform of that checkpoint is stored and applicable upon player death/respawn.
In regards to the Final Project Proposal, I feel as though the element that benefitted myself the most was the mechanical breakdown. Because my tasks were primarily level design related, I needed to understand how Kai (who came up with this idea) wanted jumping and health to function. Building a level around these design constraints posed new problems not present in most 3D platformers. I found the damage system to be very punishing, which made me implement the checkpoint system in the first place. Without outlining our creative intent from a mechanics standpoint, I would not have been able to find a solution that was tailored to Kai’s artistic vision. 
Going forward and in creating new game projects, I need to do a better job managing the team repository. I had made mistakes regarding forks, pushes, pulls, etc that set myself back and created frustration during this development process so far (plus my Git account got banned). While there haven’t been any irreversible consequences from my oversight so far, failing to properly maintain the repository in the future can be costly to me and my team.


## Open-Source Assets
If you added any other outside assets, list them here!
- [Sprout Lands sprite asset pack](https://cupnooble.itch.io/sprout-lands-asset-pack) - rabbit and item sprites
- [Pixel Penguin 32x32 Asset pack](https://legends-games.itch.io/pixel-penguin-32x32-asset-pack) - penguin sprites
- [Coins 2D](https://artist2d3d.itch.io/2d) - coin sprites
- [Adventurous Music](https://fan-zoo.itch.io/adventure-music-pack) - ost
