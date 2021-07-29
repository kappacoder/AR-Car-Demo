# AR-Car-Demo
A car demo with AR Foundation for Unity


Project explanation:

There is only one scene in the project - Game.scene

The GameCore game object has 2 scripts attached:

GameCore.cs 
GameContainer.cs

GameContainer.cs

	The project utilises Adic - https://github.com/intentor/adic - a Unity plugin for dependency injection. This monobehaviour binds interfaces to classes and generates a singleton for the EntityGeneratorService.
	It is executed first with script execution order of -300.

GameCore.cs
	
	This is the single entry point for the scene. All needed game object references are passed to this Monobehaviour and the necessary services are initialised with them. ( Example: entityGenerationService is initialised with the Vehicle prefab )

Why do I use this approach ( dependency injection ) ?

	For small projects like this one it might look like an overkill. However, using this approach has several benefits:
Reduce the number of monobehaviour components that are attached in the scene - in big projects you may have hundreds of scripts attached in one scene - it’s hard to maintain and keep track of all of them
Allows you to have a single entry point for the scene - the injection container script - everything is initialised there
Allows you to actually use interfaces in Unity
Allows you to bind different implementations to the injected interfaces ( Example: if you want to test functionality A, you can Bind EntityGenerationServiceTestA to the interface IEntityGenerationService and it will be automatically injected in all scripts that reference it )
Allows you to have an MVC approach to Unity
Allows you to have different controllers and bind them to different objects at runtime

There is also the UICanvas which has 2 views - default view and play view

The AR implementation is in AR Session and AR Origin

So, what happens?

When the user touches an AR plane surface, a car is spawned ( UserTouchController.cs is subscribed for the touch event and calls the EntityGeneratorService to spawn the car )
EntityGeneratorService.cs spawns the car and attaches to it the VehicleController.cs script - the car can now be moved with the joystick
If you press the X button - the GameUIComponent calls the EntityGeneratorService to destroy the car, everything is reset and you can spawn a new car



Note: This project utilises UniRX - https://github.com/neuecc/UniRx
It is a handy plugin for events which allows them to have LINQ queries and subscribe to them easily
