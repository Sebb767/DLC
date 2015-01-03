DLC
===

Dynamic Libary(function) Caller - A small C# Libary to call functions in unmanaged libarys (.dll, .so). 

Features
--------

- System independent - works on Windows + Linux. OSX isn't tested, but should work, too.
- Unload libarys - if you don't need it any more, unload it. 
- No caching - if you have a new version, don't worry about an old, loaded version.
- Dynamic function search - you can call any function by name, no need to define them at compile time.

How to use it
-------------

Simply include the dlc.dll in your solution and create a new dlc-class.
