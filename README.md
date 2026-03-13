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
Jingyi Bi:

During this phase, I handled the majority of scene construction tasks for the project. Utilizing the Tilemaps system and various GameObjects, I built the foundational environment including architecture and terrain. I manually configured the necessary colors and Materials for all previously untextured models, ensuring visual consistency. For specific project configurations, I meticulously adjusted component parameters for character models and various functional objects within scenes, while also optimizing the overall layout of the UI interface. Regarding scripting, I developed the core logic for the RestaurantOwner and Customer scripts, defining relevant methods and variables. I subsequently iterated and debugged these scripts in collaboration with team members.

Looking back at our project proposal developed in Week 7 (W7), I believe it demonstrated remarkable foresight. The proposal was so meticulously detailed that we encountered almost no last-minute changes or additional requirements during actual development and editing, significantly ensuring progress stability. However, I identified an area for improvement: while the plan was comprehensive, we failed to fully standardize all classes and interface specifications during the initial architecture phase. This led to significant time and effort spent resolving inconsistencies and debugging issues during later code integration. For future game development projects, I will propose establishing unified coding standards and a class architecture diagram before commencing work to further enhance team collaboration and development efficiency.


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

We actually revised some details based on the Proposal and Breakdown, but it is really useful to let us get back on track and operate logically. Our Proposal clearly outlined our intended architecture using the Model–View–Controller pattern and how OrderData and OrderUI could act. This is helpful when we start our project and divisions, because it clears the separate data, UI, and interaction logic, and our state transitions, such as Accepted → PickedUp → Delivered → Submitted. However, the proposal did not anticipate the level of state synchronization required between systems. For example, we did not foresee issues involving cursor locking, UI button residue, or order-state gating for pickup interactions. These were not conceptual design problems but implementation-level interaction problems that only emerged once systems began interacting in real time.


## Final Submission
### Group Devlog
#### Finite State Machine (FSM) Pattern with C# Enums
The Finite State Machine (FSM) pattern is a simple way to organize how game objects behave by splitting their actions into clear "states" (like "doing a burger order" or "doing a pizza order") and rules for switching between states. Using C# enums makes this easier because enums are a safe way to list all possible states (you can’t use a state that doesn’t exist).

Where We Used This in Our Game Code？

1. Enum Definition in DeliveryManager.cs
   
We made a `GameState` enum to track what part of the game the player is in:

    public enum GameState
    {
        FirstOrder,    // Player is doing Anton’s burger order
        Transition,    // Player finished the burger order (ready for pizza)
        SecondOrder,   // Player is doing Amy’s pizza order
        Finished       // All orders are done
    }
    
    public class DeliveryManager : MonoBehaviour
    {
        public static DeliveryManager Instance { get; private set; }
        public GameState currentGameState = GameState.FirstOrder; // Start with burger order
    
        // Switch to pizza order state
        public void StartPizzaPhase()
        {
            if (currentGameState == GameState.Transition)
            {
                currentGameState = GameState.SecondOrder;
            }
        }
    
        // Mark burger order as finished
        public void CompleteFirstDelivery()
        {
            currentGameState = GameState.Transition;
        }
    }

2. Using the Enum in RestaurantOwnerNPC.cs
The NPC (restaurant owner) uses the enum to decide what to say to the player:

        private void InteractWithOwner()
        {
            GameState state = DeliveryManager.Instance.currentGameState;
        
            // If player finished the burger order, show pizza order dialogue
            if (state == GameState.Transition)
            {
                startedPizzaDialogue = true;
                GameProgress.Instance.secondOrderAccepted = true;
                if (RO_PizzaOrder_Line1 != null) DialogueManager.Instance.StartDialogue(RO_PizzaOrder_Line1);
            }
            // If player is still doing the burger order, show burger dialogue
            else if (state == GameState.FirstOrder)
            {
                startedBurgerDialogue = true;
                GameProgress.Instance.hasTalkedToOwner = true;
                GameProgress.Instance.firstOrderAccepted = true;
                if (RO_BurgerOrder != null) DialogueManager.Instance.StartDialogue(RO_BurgerOrder);
            }
        }


Why This Pattern Helped Our Game？

1. Easy to understand: Instead of messy code with lots of `if` statements, we just check the `GameState` enum to know what the game should do. Even new coders can see "FirstOrder = burger, SecondOrder = pizza".

2. Hard to break: Enums only let us use the states we defined (like `FirstOrder`), so we can’t accidentally use a fake state (like "BurgerDone123") that would crash the game.
   
3. Easy to change: If we want to add a new state (like "BonusOrder"), we just add it to the enum and write one small piece of code—we don’t have to rewrite all the game logic.

This pattern made our game’s flow (burger order -> pizza order -> finished) clear and easy to fix.

### Team Member Name 1
Jingyi Bi:

### Team Member Name 2
Peiyi Xiong: 

### Team Member Name 3
Ruixuan Pan:

## Open-Source Assets
- [funky preparation](https://www.aigei.com) - background music
- [characters](https://www.mixamo.com/#/)- 3D characters model and animation
- [buildings](https://brokenvector.itch.io/low-poly-brick-houses) - 3D building model
- [characters](https://vinrax.itch.io/psx-casual-male-character) - 3D characters model
- [characters](https://vinrax.itch.io/psx-secretary-character)- 3D characters model
