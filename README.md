# GDIM32-Final
## Check-In
### Team Member Name 1
Jingyi Bi
### Team Member Name 2
Peiyi Xiong
### Team Member Name 3
Ruixuan Pan

## Final Submission
### Group Devlog

Our project uses raycasting to implement the mouse-click hamburger pickup feature. Players can left-click a 3D hamburger object in the scene to add it to their inventory UI. The burger will appear in the lower right corner of the screen when picked up. This feature relies on raycasting because we need to connect 2D mouse input from screen coordinates to a 3D game object. Rays are defined by origin and direction vectors and are the only way to accurately detect this kind of interaction.

The main idea behind this feature is raycasting, which needs two key vectors: a starting origin and a direction. In our HamburgerInteract.cs script, we first create a ray with Camera.main.ScreenPointToRay(Input.mousePosition). This method automatically finds the ray’s origin which is the camera’s 3D position and the direction vector. The direction vector is a normalized line pointing from the camera to the spot in the 3D world that matches the mouse’s position on screen. We then use Physics.Raycast(ray, out RaycastHit hit) to check if this ray touches the hamburger’s collider. If the object the ray hits is the hamburger, we check this with hit.collider.gameObject == gameObject, and then start the pickup steps. We turn off the 3D hamburger model, make a UI icon for it in the inventory and mark the hamburger as picked up.

This code works because raycasting uses vector math to connect 2D and 3D spaces. Without raycasting, we could not reliably link mouse clicks to 3D objects. Simple position checks would not work if the camera moves or the hamburger is blocked by something else. The ray’s direction vector makes sure we only detect the object that is right under the mouse cursor. This makes the interaction accurate and easy to use. The RaycastHit result also tells us the exact point where the ray hits the object, a vector3 value, which lets us add visual feedback like a highlight to the hamburger later if we want to.

### Team Member Name 1
Put your individual final Devlog here.
### Team Member Name 2
Peiyi Xiong: 

At this project stage, I wrote the initial versions of the code for PlayerMovement, OrderUI, RestaurantOwner, Customer, EarningsUI, DeliveryManager, and HamburgerInteract (these scripts were later refined by team members). Beyond coding, I sourced and imported external assets into the project, including 3D character models (for which I created animation controllers) and background music (along with configuring audio tracks). I also set up two distinct lighting types within the Unity scene, implemented the hamburger interaction feature (by creating the corresponding interaction script and associated UI elements), and created over three custom materials specifically for the character models.

Our project proposal gave a clear basic plan for Order Up!, including core game mechanics, UI systems, NPC interactions and main architecture like MVC pattern. It guided our development well but missed key technical details, such as how to code the hamburger pickup feature and what Unity components to use. So I had to figure out these details while developing, like writing raycasting code for 3D object interaction and setting colliders for interactive items.

Our original architecture plans stayed mostly the same. We only made small changes to make code more modular, like splitting interaction logic into separate scripts for easier team optimization.

For future planning, I’ll add technical details like core scripts and Unity components to proposals. I’ll also make a detailed task checklist early on to set clear milestones and plan better code modularity for easier team work.

### Team Member Name 3
Ruixuan Pan:

In this final check-in project, I mainly focused on creating dialogue with NPCs and the interactive UI designs. 

For one thing, I built the dialogue system architectures of our two NPCs, Restaurant Owner and Consumer Anton, by writing and modifying the script of Delivery Manager and so on. We designed the choice-based branching dialogue for the Restaurant Owner with the Player, and added the dialogue UI of buttons, titles, and texts. I use the core methods of StartDialogue(DialogueNode startNode), ChooseOption(int index), and DisplayNode to realize it. I also design the interaction between the player and the NPC. For instance,  I use the 'public float interactionDistance = 8f; if (isInRange && Input.GetKeyDown(interactKey))' to create the interaction loop that, when the player approaches the Restaurant Owner, the hint on his head (as a world-space) appears. And when the player clicks 'I', the dialogue with the Restaurant Owner and the options appear.

For the other, I designed the Order UI with my teammates' help. To create the interactive UI structure, we used DeliveryManager.Instance.currentOrder, OrderData structure
OrderUI display logic and the OrderPanel. I created the OrderData class to store structured information about each delivery task. This class centralizes all order-related properties, which include "public string customerName; public string foodType; public int basePay; public string orderStatus." This OrderUI will change according to the player's actions shown in "Delivered State; Total Earning" on the screen. 

## Open-Source Assets
- [funky preparation](https://www.aigei.com) - background music
- [characters](https://www.mixamo.com/#/)- 3D characters model and animation
- 
