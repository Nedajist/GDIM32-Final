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
### Nathan Hernandez
Final Devlog
Individual Prompt
For the remainder of the making of In Too Deep, I focused on finalizing the level design process. I implemented unique gameplay elements, (with the help of Kai) through the use of slopes. Kai also created consumables that provided unique abilities which I kept in mind to take advantage of in the rest of the level. As for specific game objects, I created platform variations for the lower, Abyss portion of the game (they were initially called rock variation 1,2,3, however in the final version they were renamed to platform1,2,3). These objects are prefabs that could be used in the scene to construct the level. I also added decorations such as the giant ice pillars (IceSpike1,2) at the very bottom of the level. Overall I focused mostly on just designing the level through prefabs I created and pasted throughout the level, also taking into account the many new mechanics Kai added into the game. 
Another minor contribution I made to the final version was the respawn button. I barely touched visual studio during the back half of the project, due to the fact that I was tasked with finishing the level, however I did add in a call to the Respawn() method whenever R is pressed. This isn’t impressive, just something I happened to add in.
Marcelo
Contributions: 3
Perseverance: 3
Communication: 3
Marcelo put a lot of effort into creating the dialogue system throughout the development process. While implementing the NPC interaction was a very difficult process, he was able to get it done and work properly. He worked primarily within the scripts, focusing on using polymorphism to implement NPC behaviours. His perseverance was also a notable element of his contributions, due to the fact that he struggled a lot working out the bugs of the dialogue system. He reached out for help and worked late in the night to not let the team down. Lastly his communication was on point too. He showed up to every team meeting, and checked in to make sure that his work in visual studio reflected the artistic vision of In Too Deep.
Kai
Contributions: 3
Perseverance: 3
Communication: 3
Kai contribution wise proved to me that this man is already industry ready. He implemented so many cool and unique features at such a rapid pace, making it clear that he was passionate about the development process. His skill was a huge help and made In Too Deep a genuinely fun experience. He added in many unique mechanics such as the caffeine, added in cool jump sound effects, crazy particles, etc. This bleeds into his perseverance as well, as he puts a ton of effort constantly into improving the experience. He absolutely went above and beyond with the features he added. As for communication, Kai was not only a major contributor for his commits, but also as a teacher for Marcelo and I. He helped me deal with my strange GitHub situation, and he helped Marcelo in creating the dialogue system. Overall if there was anyone deserving of a perfect score it is absolutely Kai.
Me (Nathan)
Contributions: 1
Perseverance: 2
Communication: 2
While I definitely made contributions to the project, overall most of my work came within the scene itself, rather than through visual studio. While I attempted to help with code and technical elements of the project, I don’t think I did enough to stand out in this area. Both Marcelo and Kai felt that they were able to handle their own tasks without much help from me, however I think that I could have contributed in other areas by adding new mechanics that wouldn’t necessarily interfere with what Kai and Marcelo were working on. As for perseverance, I feel as though I was decent in this area. There was never a task I was delegated that I failed to complete, but again I didn’t go above and beyond as I would have wanted to. And lastly, for communication, I think I was satisfactory as I did continuously check in with my team regarding the artistic vision of In Too Deep, however I did miss one meeting we had scheduled on our own due to the fact that I had a shift at my job that time. Overall I don’t think I provided as much support as I could have, and while I can accredit a lot of that to the fact that I had my GitHub account banned multiple times during the development process, I feel as though I should have done a better job taking initiative.


### Marcelo Tolosa
Since the Check-In, I've improved the NPC dialogue by adding every single node that uniquely correlates with the scripts that each team member created for the NPC's. I created all of Terri's dialogue and implemented one line start nodes. In AdvanceDialogue() method, it first checks whether the player's quest scores are less than or greater than the required quest scores in order to interact with the NPC. If not, then the player will only be able to interact with the NPC with a one liner. This is only changed until the player completes a previous quest in order to unlock the nodes with multiple dialogue. 

Addiitionally, created the logic for NPC's that talk to the player which increment the queststage values based on which NPC's you talk to and which option you choose seen in the SelectedOption() method. New methods, variables, and gameobjects I created include: SelectedOptionsfromUI(), (changing AdvanceDialogue()), adding _onlineNode variable, both requiredquest1stage and requiredquest2stage, as well as quest1stage and quest2stage in the player variable. The logic for the player quest stages below:     
public int quest1Stage = 0;
    // stage 0 = Quest not started
    // stage 1 = Accepted from Mushroom Man
    // stage 2 = Completed by talking to HOP HOP
public int quest2Stage = 0;
    // stage 0 = quest not started
    // stage 1 = accepted from HOP HOP
    // stage 2 = item collected
    // stage 3 = item delivered

Terri's dialogue:
([In Too Deep Dialogue] https://docs.google.com/document/d/1X34PPDPiaFY0Egpk9Z-WLCvEmmA_12GL5txa25ktmrA/edit?usp=sharing)


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
- [Footsteps on Gravel](https://pixabay.com/sound-effects/film-special-effects-running-on-gravel-301880/)
