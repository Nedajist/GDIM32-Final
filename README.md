[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/dDoYVaRL)
# GDIM32-Final
## Check-In
### Nate
My primary contributions to In too deep so far have been level related. I created the scene, terrain, and level assets that the player jumps between. This includes the Checkpoint game object, which can be found on various platforms throughout the level, which function like a traditional checkpoint system. To set this system up, I also had to implement respawn. I created a Respawn() method, which resets the player to the transform of their most recent checkpoint, and reverts their health to its maximum state. From here I created SetRespawnPoint(), a method that stores the transform of the most recent checkpoint. This method would then be called by the CheckpointController, which first compares the tag of the collision to the Player, before calling SetRespawnPoint(). Consequently, whenever a player collides with the Checkpoint game object, the transform of that checkpoint is stored and applicable upon player death/respawn.
  
In regards to the Final Project Proposal, I feel as though the element that benefitted myself the most was the mechanical breakdown. Because my tasks were primarily level design related, I needed to understand how Kai (who came up with this idea) wanted jumping and health to function. Building a level around these design constraints posed new problems not present in most 3D platformers. I found the damage system to be very punishing, which made me implement the checkpoint system in the first place. Without outlining our creative intent from a mechanics standpoint, I would not have been able to find a solution that was tailored to Kai’s artistic vision. 

Going forward and in creating new game projects, I need to do a better job managing the team repository. I had made mistakes regarding forks, pushes, pulls, etc that set myself back and created frustration during this development process so far (plus my Git account got banned). While there haven’t been any irreversible consequences from my oversight so far, failing to properly maintain the repository in the future can be costly to me and my team.

//also the second light source can be hard to find but it is over a levitating mushroom man

### Kai

### Marcelo

## Final Submission
### Group Devlog
Put your group Devlog here.


### Team Member Name 1
Put your individual final Devlog here.
### Team Member Name 2
Put your individual final Devlog here.
### Team Member Name 3
Put your individual final Devlog here.


## Open-Source Assets
- [Mushroom NPC](https://assetstore.unity.com/packages/3d/characters/humanoids/lowpoly-mushroomman-character-287820)
- [Frog NPC](https://assetstore.unity.com/packages/3d/characters/frog-marauder-pixelated-texture-316487)
- [Mutant NPC](https://assetstore.unity.com/packages/3d/characters/creatures/creature-horror-mutant-113565)
- [Main Player Model, "The Boss"](https://www.mixamo.com/#/?page=1&type=Character)
- [Eyeball Model](https://assetstore.unity.com/packages/3d/characters/humanoids/humans/realistic-fantasy-eyes-67861)
- [Treasure Chest Model](https://assetstore.unity.com/packages/3d/props/interior/treasure-chest-pbr-72498) 
- [Food Consumable Models](https://assetstore.unity.com/packages/3d/props/food/rpg-fantasy-food-items-pack-280556)
- [Pie](https://www.vecteezy.com/png/19040583-an-8-bit-retro-styled-pixel-art-illustration-of-an-apple-pie)
- [Stew](https://www.vecteezy.com/png/60597586-pixel-art-cooking-pan-with-lid-and-handles)
- [Cheese Wheel](https://es.pixilart.com/art/cheese-wheel-a0f215ec5d94dfd)
- [Cheese](https://favpng.com/png_view/milk-milk-cheese-pixel-art-bead-png/8NT6fG2e)
