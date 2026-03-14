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
#### 1. Finite State Machine (FSM) Pattern with C# Enums
The Finite State Machine pattern organizes a system into different states and defines how it moves from one state to another. We used C# enums to make clear game states and cut down on error-prone conditional logic.

Where We Used This in Our Game Code？

DeliveryManager.cs
      
      // Defines the game’s core states as an enum (FSM state definition)
      public enum GameState { FirstOrder, Transition, SecondOrder, Finished }
      
      public class DeliveryManager : MonoBehaviour
      {
          public static DeliveryManager Instance { get; private set; }
          public GameState currentGameState = GameState.FirstOrder; // Active state
      
          // State transition methods
          public void CompleteFirstDelivery()
          {
              currentGameState = GameState.Transition; // Transition to post-burger delivery state
              if (pizzaOrder != null)
                  pizzaOrder.isUnlocked = true;
          }
      
          public void StartPizzaPhase()
          {
              currentGameState = GameState.SecondOrder; // Transition to pizza delivery state
              OrderManager.Instance.AcceptOrder(pizzaOrder);
          }
      
          public void CompleteSecondDelivery()
          {
              currentGameState = GameState.Finished; // Transition to game completion state
              GameProgress.Instance.secondDeliveryCompleted = true;
          }
      }
State-Driven Logic: RestaurantOwnerNPC.cs

The InteractWithOwner() method uses currentGameState to determine which dialogue to trigger and which game logic to execute:
      
      private void InteractWithOwner()
      {
          GameState state = DeliveryManager.Instance.currentGameState;
          // State-based conditional logic
          if (state == GameState.Finished || isPizzaFinished)
          {
              DeliveryManager.Instance.currentGameState = GameState.Finished;
              GameProgress.Instance.secondDeliveryCompleted = true;
              if (RO_Finish_PizzaOrder != null) DialogueManager.Instance.StartDialogue(RO_Finish_PizzaOrder);
          }
          else if (state == GameState.Transition)
          {
              startedPizzaDialogue = true;
              GameProgress.Instance.secondOrderAccepted = true;
              if (RO_PizzaOrder_Line1 != null) DialogueManager.Instance.StartDialogue(RO_PizzaOrder_Line1);
          }
          else if (state == GameState.FirstOrder)
          {
              // First order (burger) logic
              startedBurgerDialogue = true;
              GameProgress.Instance.hasTalkedToOwner = true;
              GameProgress.Instance.firstOrderAccepted = true;
              if (RO_BurgerOrder != null) DialogueManager.Instance.StartDialogue(RO_BurgerOrder);
          }
      }
Why This Pattern Was Useful

Easier State Management: The GameState enum gives us one clear variable to show the game's current state, instead of having to deal with a lot of boolean flags like isBurgerPhase or isPizzaPhase. This makes it a lot easier to keep track of how the game is going and find any bugs that have to do with the state.

Easy to Extend: Adding a new state, like a BonusLevel, is simple. All you have to do is change the enum and add the code that goes with it. You don't have to change any code that isn't related to the checks.

Clear Separation of Behavior: FirstOrder, Transition, SecondOrder, and Finished are all states that handle their own logic. Each phase has its own logic. For instance, the way pizzas and burgers are brought to customers is different. This stops the different parts of the game from working together by mistake.

#### 2. Inheritance with Polymorphism
Inheritance allows subclasses to reuse code from a parent class, while polymorphism enables subclasses to override parent methods to implement unique behavior. We used an abstract base class (InteractableBase) to define a common interface for all interactable objects (like hamburgers, pizzas, NPCs), then subclassed it to implement object's own interaction logic.

Where We Used This in Our Game Code？

Abstract Base Class: InteractableBase.cs
This class defines a common structure for all interactable objects, including an abstract Interact() method (enforcing implementation in subclasses):

      public abstract class InteractableBase : MonoBehaviour
      {
          [Header("Interaction Settings")]
          public float interactionDistance = 5f;
          public GameObject interactionPrompt;
      
          // Abstract method: subclasses MUST implement this
          public abstract void Interact();
      
          // Shared helper method (inherited by all subclasses)
          protected bool IsPlayerInRange()
          {
              GameObject player = GameObject.FindWithTag("Player");
              if (player == null) return false;
              return Vector3.Distance(transform.position, player.transform.position) <= interactionDistance;
          }
      
          // Virtual method: subclasses can override (optional)
          protected virtual void Update()
          {
              if (IsPlayerInRange() && Input.GetKeyDown(KeyCode.I))
              {
                  Interact();
              }
          }
      }
Subclass 1: HamburgerInteract.cs (Overrides Interact())

      public class HamburgerInteract : InteractableBase
      {
          // Override abstract Interact() method to implement burger's own logic
          public override void Interact()
          {
              if (isPicked) return;
      
              if (OrderManager.Instance.currentOrder != null &&
                  OrderManager.Instance.currentOrder.foodType == "Burger")
              {
                  isPicked = true;
                  gameObject.SetActive(false);
                  inventoryIcon.SetActive(true);
                  OrderManager.Instance.PickUpOrder();
                  GameProgress.Instance.burgerPickedUp = true;
              }
          }
      
          // Override Update() to add burger's own range checks
          private override void Update()
          {
              if (isPicked || player == null || !GameProgress.Instance.firstOrderAccepted)
              {
                  interactionPrompt?.SetActive(false);
                  return;
              }
              float distance = Vector3.Distance(transform.position, player.position);
              interactionPrompt.SetActive(distance <= burgerInteractDistance);
          }
      }
Subclass 2: PizzaInteract.cs (Overrides Interact())

      public class PizzaInteract : InteractableBase
      {
          // Override abstract Interact() method to implement pizza-specific logic
          public override void Interact()
          {
              if (isPicked) return;
      
              if (OrderManager.Instance.currentOrder != null && 
                  OrderManager.Instance.currentOrder.foodType == "Pizza")
              {
                  isPicked = true;
                  gameObject.SetActive(false);
                  inventoryIcon.SetActive(true);
                  OrderManager.Instance.PickUpOrder();
                  GameProgress.Instance.pizzaPickedUp = true;
              }
          }
      
          // Override Update() to add pizza-specific range checks
          private override void Update()
          {
              if (isPicked || player == null || !GameProgress.Instance.secondOrderAccepted)
              {
                  interactionPrompt?.SetActive(false);
                  return;
              }
              float distance = Vector3.Distance(transform.position, player.position);
              interactionPrompt.SetActive(distance <= pizzaInteractDistance);
          }
      }
Subclass 3: CustomerAnton.cs (Overrides Interact())

      public class CustomerAnton : InteractableBase
      {
          // Override abstract Interact() method to implement NPC's own dialogue logic
          public override void Interact()
          {
              bool hasBurger = GameProgress.Instance.burgerPickedUp;
              bool hasPizza = GameProgress.Instance.pizzaPickedUp;
      
              if (isDeliveryCompleted)
              {
                  DialogueManager.Instance.StartDialogue(anton_Nothing_on_hand);
                  return;
              }
      
              if (hasBurger && !hasPizza)
              {
                  DialogueManager.Instance.StartDialogue(anton_Burger_on_hand);
                  deliveryDialoguePlayed = true;
                  waitingForClickDelivery = true;
              }
          }
      }
      
Why This Pattern Was Useful
Better Code Reuse Across Classes: The InteractableBase class contains shared logic like IsPlayerInRange() and the interaction prompt. Because of this, the subclasses don’t have to repeat the same code. This helped reduce repeated code in HamburgerInteract, PizzaInteract, and CustomerAnton.

Using Polymorphism to Allow Different Behaviors:
The abstract Interact() method makes sure that all interactable objects follow the same structure, but each subclass can still have its own behavior. For example, one object can pick up a burger while another can start an NPC conversation. This also makes it easier to add new interactable objects later, like a soda can, by just creating a new subclass and overriding Interact().

Making the Code Easier to Update and Maintain:
If we want to change the core interaction logic, like changing the interaction key from I to E, we only need to update it in the base class instead of every subclass. This makes the code easier to manage and reduces the chance of bugs.

#### 3. Singleton Pattern
We used the Singleton pattern so different scripts could easily access important managers like DeliveryManager and GameProgress without needing to repeatedly search for them.

Where We Used This in Our Game Code？

GameProgress.cs

      public class GameProgress : MonoBehaviour
      {
          public static GameProgress Instance { get; private set; }
      
          private void Awake()
          {
              if (Instance == null) Instance = this;
              else Destroy(gameObject); // Ensure only one instance exists
          }
      }
      
Why It Was Useful
Global State Access: The Singleton pattern lets any class access game progress (like GameProgress.Instance.burgerPickedUp) or state (like DeliveryManager.Instance.currentGameState) without passing references between objects. This was critical for FSM state checks and polymorphic interaction logic (like HamburgerInteract checking if the first order is accepted).



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
