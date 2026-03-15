[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/dDoYVaRL)
# GDIM32-Final
## Check-In
### Group Devlog
I (Kai) used version control to resolve a series of merge conflicts affecting our main Level scene. Nathan (as mentioned in his devlog) had accidentally been working on a fork of the team's repository rather than directly on it. At first this did not cause any problems, as he would regularly merge upstream branches into his fork. However, because the rest of our branches were upstream, Github allowed him to make changes to his branch without checking if they conflicted with changes we had made. Github would detect the changes made upstream, but not tell him that he needed to pull those changes; he had to do so manually.

At some point, after Nathan had created a large portion of the Level scene's platforms and added a checkpoint system to the player, he would attempt to merge the upstream branches into his. However, because we had pushed several changes during that time and because GitHub did not tell Nathan to pull each change as it was made (due to him being downstream), all of those changes stacked up and resulted in over 60 merge conflicts.

I diagnosed the problem by first checking Nathan's branch on the team's repository and seeing that he had not made any changes to it for several days. I then saw a screenshot Nathan provided of his Github application, which showed that me and Marcelo's branches were shown as being "upstream". This indicated to me that Nathan was working on a fork of this repository, and a quick check on Github confirmed my suspicions. 

Nathan's fork is now deleted, so I am not fully sure what happened next, but we managed to merge his Level scene into my branch at the cost of reverting the Player and UI game objects to older versions of themselves (versions that existed in Nathan's fork). The level was there, but nearly everything else was out-of-date. 

To solve this issue, I used version control to create a branch from the latest commit I made before merging Nathan's fork over. On the new branch, I copy-pasted all of Nathan's level elements (the terrain, lighting, models, textures, materials, audio, skybox) over to my branch. I used version control because it was the only way I knew of undoing changes I made to the game files, which was how I could bring the player and UI back to their present states.



### Nate
My primary contributions to In too deep so far have been level related. I created the scene, terrain, and level assets that the player jumps between. This includes the Checkpoint game object, which can be found on various platforms throughout the level, which function like a traditional checkpoint system. To set this system up, I also had to implement respawn. I created a Respawn() method, which resets the player to the transform of their most recent checkpoint, and reverts their health to its maximum state. From here I created SetRespawnPoint(), a method that stores the transform of the most recent checkpoint. This method would then be called by the CheckpointController, which first compares the tag of the collision to the Player, before calling SetRespawnPoint(). Consequently, whenever a player collides with the Checkpoint game object, the transform of that checkpoint is stored and applicable upon player death/respawn.
  
In regards to the Final Project Proposal, I feel as though the element that benefitted myself the most was the mechanical breakdown. Because my tasks were primarily level design related, I needed to understand how Kai (who came up with this idea) wanted jumping and health to function. Building a level around these design constraints posed new problems not present in most 3D platformers. I found the damage system to be very punishing, which made me implement the checkpoint system in the first place. Without outlining our creative intent from a mechanics standpoint, I would not have been able to find a solution that was tailored to Kai’s artistic vision. 

Going forward and in creating new game projects, I need to do a better job managing the team repository. I had made mistakes regarding forks, pushes, pulls, etc that set myself back and created frustration during this development process so far (plus my Git account got banned). While there haven’t been any irreversible consequences from my oversight so far, failing to properly maintain the repository in the future can be costly to me and my team.

//also the second light source can be hard to find but it is over a levitating mushroom man

### Kai
I created the Player game object found in the Kai Movement Testing Scene, and wrote most of the player script attached. I handled player WASD movement and charging jumps with space in the player's Update() method, the player animator component, the inventory system (HandSelected and InventoryUpdated signals, _ClearInteractable(), _DisplayInteratable(), _AddInteractable() methods in player), picking up and eating food, player collision detection (OnCollisionEnter(), OnCollisionExit() in player), the lazy health bar (LazySlider child of UIController), the depth meter, the UIController script, and the GameController singleton. 

Our proposal was detailed enough on the basic game mechanics that we all generally knew what to implement, but left out plenty of blanks for us to fill in as we developed. For example, we knew that the player would be able to charge their jump with space, but not how far they would jump or how long they could charge it. We knew the player would pick up health-raising consumables, but not what those consumables would look like or how much they would heal. The proposal described the base mechanics, and we were free to tune their specifics. Our biggest change was making the game a one-way trip. The player no longer has to climb back up out of the hole. Part of this is change is due to scope limitations, but it is also because we would have to make all of the vertical jumps possible from down to up as well as from up to down. We created a Trello board, but only used Discord messages and calls to track our progress. For small-scale projects like this, I find informal Discord messages and announcements to work better than Trello. In the future, I will definitely plan more team meetings to ensure that we are all on the same page. 

### Marcelo

I created the different NPC's found in Marcelo Testing Scene, and created the dialogue classes (Multiple Dialogue, DialogueUI, worked on parts of the player script, DialogueNode, and NPC inheritance scripts). Within Unity, I created the NPC gameobjects, as well as all the working UI regarding the dialogue (buttons, canvas for the NPC thought bubble, Dialogue text options). Methods and scirpting was inspired heavily on the W9 pre-learing activity after realizing that system was the requirement for NPC dialogue. Methods including EndDialogue(), AdvanceDialogue(), SelectedOption(), and using ScriptableObjects (as was required). The scriptable objects have lines that can be changed whenever (making it convenient, especially for this system).

In terms of the NPC Dialogue in the project proposal, there were changes that were made to the original plan. Originally our plan was to have quest content that started with, "Given by NPC #1, turn in to NPC #3. Find the mysterious treasure somewhere near the bottom of the hole. Given by NPC #2, turn in to NPC #3. Escape the hole, and return to the surface. Given by NPC #3, turn in to NPC #1." Recent changes were made and now the player is supposed to reach NPC #2 given by the NPC #1, and after that must give a treasure to NPC #3. We used Trello, and discord messages to communitcate with eachother, and keep track of progress as a group. In the future, I will definitely try to meet up in person more often as it keeps me and the group productive, plus ensures that everyone is working together.

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
- [Main Player Model, Reze](https://www.cgtrader.com/3d-models/character/woman/reze-chainsaw)
- [Food Consumable Models](https://assetstore.unity.com/packages/3d/props/food/rpg-fantasy-food-items-pack-280556)
- [Coffee](https://sketchfab.com/3d-models/low-poly-coffee-cup-6e6432980e0944219434b81cbc67eacb#download)
- [Basic 3D shapes](https://assetstore.unity.com/packages/3d/primitives-basic-shapes-collection-8198)
- [Lowpoly Asset Collection](https://craigsnedeker.itch.io/classic64-asset-library)
- [Bomb](https://www.cgtrader.com/items/149595/download-page)
- [Ruined Tower](https://assetstore.unity.com/packages/3d/environments/ruined-tower-free-66495)
  
- [Pie](https://www.vecteezy.com/png/19040583-an-8-bit-retro-styled-pixel-art-illustration-of-an-apple-pie)
- [Stew](https://www.vecteezy.com/png/60597586-pixel-art-cooking-pan-with-lid-and-handles)
- [Cheese Wheel](https://es.pixilart.com/art/cheese-wheel-a0f215ec5d94dfd)
- [Cheese](https://favpng.com/png_view/milk-milk-cheese-pixel-art-bead-png/8NT6fG2e)
- [Dark Metal](https://www.freepik.com/free-photo/dark-grunge-texture-background-with-scratches-stains_45349905.htm#fromView=keyword&page=1&position=9&uuid=d33523ec-26bf-4a34-b48f-0182fbc8df83&query=Black+metal+texture)
- [Coffee Sprite](https://www.shutterstock.com/search/coffee-geek?image_type=illustration)

- [Grenade Explosion SFX](https://assetstore.unity.com/packages/audio/sound-fx/grenade-sound-fx-147490)
- [Nuclear Explosion VFX](https://assetstore.unity.com/packages/vfx/particles/fire-explosions/nuclear-explosion-atomic-bomb-215191)
- [Adventure Music](https://fan-zoo.itch.io/adventure-music-pack)
- [Falling Impact SFX](https://pixabay.com/sound-effects/film-special-effects-fast-body-fall-impact-352725/)
- [Siren and Nuke SFX](https://pixabay.com/sound-effects/film-special-effects-siren-and-nuke-326461/)
- [Iris Out Piano Arrangement](https://onlinesequencer.net/5227672)
- [The First Layer Piano Arrangement](https://onlinesequencer.net/3628612)
- [Footsteps on Gravel](https://pixabay.com/sound-effects/film-special-effects-running-on-gravel-301880/)
- [Ambient Music](https://crowshade.itch.io/liminal-horror-dreamcore-ambient-pack-post-dream)
