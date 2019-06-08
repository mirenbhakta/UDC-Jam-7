If you want to download and run this project in Unity Editor, you will need the paid asset [Odin Inspector](https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041).

It is best to copy Odin Inspector's `Sirenx` folder from another project before opening this project for the first time on your computer.

If you open the project without Odin Inspector installed, all serialized data that was written by Odin Inspector's Serializer will be lost due to the reimport of all assets and you will have to pull from the repo again.

Project version is Unity Engine 2019.1.5f1

Some notes for the future.

As nice as it seems to separate Item objects from ScriptableObjects using Odin Serializer's polymorphism support, it ended up being more of a headache to work with all things considered. The system works fairly well and it is very easy to create lots of content (provided you have the required art assets :p). The main issue is if you want to reference other items, you cannot use Item because it is not serialized as a reference and as such what was once separated code became connected so it was worthless to try in the first place. For the future I would not have a separation between the two, it is far easier to have just Item be a ScriptableObject directly so that editor references are easier to work with.

UI is a mess of UnityEvent stuff, will definitely be worth it to make generic state scripts that I can control better and easier.
