# HP Bar API

## What does it do?
Allows modders to define custom colors for all parts of the health bar GUI, as well as for the shield overlay on characters' bodies. This functions for both player and non-player characters!

## What does it NOT do?
This does not add the ability to implement custom health types (sorry if you wanted to add a second shield or something, can't do that :c). This also does not add the ability to change the image used to draw a healthbar, but this specific limitation could potentially be lifted in the future.

# How To Implement (for developers)
To use this, start by adding the DLL as a dependency to your mod.

## Step 1: Create a color override
The main component of this mod is an instance you create within your code that extends `ROR2HPBarAPI.API.AbstractColorProvider` - this class is basically what tells the system "here's the colors you *should* be using". This class can go anywhere, so put it where you think it should go in your code.

Upon extending this class, you will notice the two built in methods. There are two properties exposed to you as well. In the `UpdateBarColors` method, edit the contents of `BarColorData` (a property). Similarly, in `UpdateShieldOverrides`, you will edit the contents of `ShieldRenderData`.

Both of the objects exposed in these properties are straightforward and intuitive to use. If you want a custom color, you set the appropriate property. If you want vanilla colors, you set it to null. You only need to do as much as you want to do in these two methods.

The only weird edge case is for shields. If you want to change the color or brightness of the shield on the fly, namely so that they are different between two people (i.e. I have two commandos, I want one with a red shield and one with a blue shield), you'll need to set `ShieldIsDynamic` (of class `DesiredShieldRenderData`) to `true`. 

By default, the system allocates one shield material to the entire `CharacterBody` ID itself (more on that in registering) by making them share the material. This means if you change the properties of the material, they all see that change. Setting `ShieldIsDynamic` to `true` instead causes it to allocate a shield material *to the applicable `CharacterBody` instance* rather than using a shared one. Naturally, this is not entirely performance friendly, but you can mix and match this by conditionally enabling or disabling `ShieldIsDynamic` based on the incoming body parameter. Try to use the global material (`ShieldIsDynamic=false`) when possible though.

Once your methods have been written to your satisfaction, we move on.

## Step 2: Registering
Once you have your override all set up, it's time to register it. To do this, you will need **a reference to your plugin, the BodyIndex of your character, and *one* instance of your provider**. Go to `ROR2HPBarAPI.API.Registry` and call the `Register` function with these three arguments. And that's it! You are finished, and the mod does all of the heavy lifting for you.

(A bit of a footnote, but the reason it wants a reference to the plugin is so that it knows who to blame for any errors when executing a handler (mostly with the intent isolating one person registering a body from a different mod, or from vanilla, so that it doesn't look like that mod's fault). Any given BodyIndex can only be registered once, so editing vanilla monsters or characters might not be such a good idea either.)

# Changelog
## 1.0.0
#### Initial Release Simulator

### Updates
* Initial Release