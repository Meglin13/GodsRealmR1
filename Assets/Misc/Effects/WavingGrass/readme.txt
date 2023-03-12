# Waving Interactive Grass

A shader that makes meshes waving in the wind. Can react to objects that bends the grass.
It doesn't use billboards or terrains, but normal 3d objects.
The wind effect is not global, but moves like waves over the area (different at each location).

by BadToxic (http://badtoxic.de)

— Content —

- *GrassWind* shader
- *Test Scenario*
- *Player* with simple movements
- *Low poly grass mesh*
- *GrassController* C# script for setting up the grass and interaction

— Requirements —

_Shader_: Works on all platforms supported by Unity. *DX9* shader model *2.0*.

— Usage —

Add all grass, flowers, mushrooms, etc. objects you want to use to the _GrassPrefabs_ list of the _GrassController_.
All these objects must use a material with the _GrassWind_ shader.
Add the _InteractionTag_ string defined in the _GrassController_ as tag to all objects that should be able to interact with the grass.
Adjust the shader settings for your needs.

— Sources —
- Gras mesh: [*Low Poly: Foliage*](https://assetstore.unity.com/packages/3d/vegetation/low-poly-foliage-66638)
- Shader tutorial: [*Waving Grass Shader in Unity*](https://lindenreid.wordpress.com/2018/01/07/waving-grass-shader-in-unity)

— Support —

Need help? Join my discord server: https://discord.gg/8QMCm2d

— Follow me —

Support me with likes and follows/subscriptions:
[Instagram](https://www.instagram.com/xybadtoxic)
[Twitter](https://twitter.com/BadToxic)
[YouTube](https://www.youtube.com/user/BadToxic)
[Facebook](https://www.facebook.com/XyBadToxic)