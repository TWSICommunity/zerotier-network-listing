# ZeroTier Network Connector
This software allows anyone connected to the internet to connect to any of the zerotier's network id which are submitted by users. Want to host a party with just you and your friends you can do so without having them type or copy in their network id as the software does it for you. Pretend it's a network id roomlist which zerotier doesn't or lacks of it's feature for a room network list. One thing to note however is that I completely forgot to implement a warning but this software uses the internet to communicate with ztlist.wolf.mba so some data charges may apply depending on your ISP.

# How to Build
REQUIRES: Microsoft Visual Studio 2013, Knowladge of C# and understanding of how webclients work.

Once you downloaded the github repo or git cloned it unzip it or open the directory. Once you find the .sln file, open it with microsoft visual studio 2013 and compile it. Remember if you don't know anything about microsoft visual studio 2013 I recommend you find tutorials on how to compile/build.

After that go into bin folder and there should be folders named "Release" -> x64/x86 or "Debug" -> x64/x86. Open those till you find a single .exe file. Ignore the rest of the files as they are unused even the .dll file.

NOTE: Do not complain about not being able to run it as normal user as this uses Administrative permissions to join networks.

# Plans for next upcoming projects
I'm still thinking as far as opening this project up to the public.
